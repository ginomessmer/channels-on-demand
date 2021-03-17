using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using DiscordVoiceChannelsOnDemand.Bot.Infrastructure;
using DiscordVoiceChannelsOnDemand.Bot.Models;
using DiscordVoiceChannelsOnDemand.Bot.Services;

namespace DiscordVoiceChannelsOnDemand.Bot.Commands
{
    [Group("slot")]
    public class SlotCommands : ModuleBase<SocketCommandContext>
    {
        private readonly ITenantService _tenantService;
        private readonly ITenantRepository _tenantRepository;

        public SlotCommands(ITenantService tenantService, ITenantRepository tenantRepository)
        {
            _tenantService = tenantService;
            _tenantRepository = tenantRepository;
        }

        [Command("setup")]
        [RequireBotPermission(GuildPermission.ManageChannels)]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        [RequireContext(ContextType.Guild)]
        public async Task Setup(IVoiceChannel voiceChannel, ICategoryChannel categoryChannel = null)
        {
            categoryChannel ??= await voiceChannel.GetCategoryAsync();

            // Get server
            var tenant = await _tenantRepository.GetAsync(categoryChannel.GuildId.ToString());

            // Check if slot already exists
            var id = voiceChannel.Id.ToString();
            var slots = await _tenantRepository.QueryAllSlotsAsync();
            if (slots.ToList().Exists(x => x.TriggerVoiceChannelId == id))
            {
                await ReplyAsync($"Voice channel `{voiceChannel.Name}` is already configured as a slot");
                return;
            }

            var slot = new Slot
            {
                TriggerVoiceChannelId = id,
                CategoryId = categoryChannel.Id.ToString()
            };

            tenant.Slots.Add(slot);
            await _tenantRepository.UpdateAsync(tenant);
        }
    }
}