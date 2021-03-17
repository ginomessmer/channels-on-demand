using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
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

        [Command("register")]
        [RequireBotPermission(GuildPermission.ManageChannels)]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        [RequireContext(ContextType.Guild)]
        public async Task Register(IVoiceChannel voiceChannel, ICategoryChannel categoryChannel = null)
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

            await ReplyAsync($"Voice channel `{voiceChannel.Name}` was successfully registered as a slot");
        }

        [Command("unregister")]
        [RequireBotPermission(GuildPermission.ManageChannels)]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        [RequireContext(ContextType.Guild)]
        public async Task Unregister(IVoiceChannel voiceChannel)
        {
            var slot = await _tenantRepository.FindSlotAsync(voiceChannel.Id.ToString());
            await _tenantRepository.DeleteSlotAsync(voiceChannel.Id.ToString());

            await ReplyAsync($"Voice channel `{voiceChannel.Name}` was successfully unregistered");
        }

        [Command("list")]
        [RequireBotPermission(GuildPermission.ManageChannels)]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        [RequireContext(ContextType.Guild)]
        public async Task List()
        {
            var guild = await _tenantRepository.GetAsync(Context.Guild.Id.ToString());
            var slots = guild.Slots;

            var channels = slots.Select(x => Context.Guild.GetVoiceChannel(Convert.ToUInt64(x.TriggerVoiceChannelId)));
            var list = string.Join("\n", channels.Select(x => $"{x.Name}\t#{x.Id}"));
            await ReplyAsync(list);
        }

        [Command("set names")]
        [RequireBotPermission(GuildPermission.ManageChannels)]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        [RequireContext(ContextType.Guild)]
        public async Task SetNames(IVoiceChannel voiceChannel, params string[] names)
        {
            var tenant = await _tenantRepository.GetAsync(Context.Guild.Id.ToString());
            var slot = tenant.GetSlot(voiceChannel.Id.ToString());

            slot.RandomNames = names;
            await _tenantRepository.UpdateAsync(tenant);
        }
    }
}