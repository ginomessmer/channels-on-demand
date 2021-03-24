using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using DiscordVoiceChannelsOnDemand.Bot.Services;

namespace DiscordVoiceChannelsOnDemand.Bot.Commands
{
    [Group("space")]
    [RequireContext(ContextType.Guild)]
    public class SpaceCommands : ModuleBase<SocketCommandContext>
    {
        private readonly IServerService _serverService;
        private readonly ISpaceService _spaceService;

        public SpaceCommands(IServerService serverService, ISpaceService spaceService)
        {
            _serverService = serverService;
            _spaceService = spaceService;
        }

        [Command("enable")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetupSpace(ICategoryChannel categoryChannel)
        {
            await _serverService.EnableSpacesAsync(categoryChannel);
            await Context.Message.AddReactionAsync(new Emoji("✅"));
        }

        [Command("disable")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task DisableSpace()
        {
            await _serverService.DisableSpacesAsync(Context.Guild);
            await Context.Message.AddReactionAsync(new Emoji("✅"));
        }

        [Command("create")]
        [Alias("new")]
        [Summary("Creates a new private text channel for you and all users you mention")]
        public async Task CreateSpace(params IGuildUser[] users)
        {
            // Check whether space is set up/enabled on the server
            var server = await _serverService.GetAsync(Context.Guild);
            if (!server.IsSpacesEnabled)
            {
                // TODO: Reply
                return;
            }

            // Get category
            ulong? categoryId = null;

            if (!string.IsNullOrEmpty(server.SpaceCategoryId))
                categoryId = Convert.ToUInt64(server.SpaceCategoryId);

            // Create new text channel
            var channel = await _spaceService.CreateSpaceAsync(Context.User as IGuildUser, users, categoryId);

            // Reply
            await channel.SendMessageAsync($"{Context.User.Mention} :wave: Your space is ready to go");
            await Context.Message.AddReactionAsync(new Emoji("✅"));
        }
    }
}