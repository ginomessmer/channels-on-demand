using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using DiscordVoiceChannelsOnDemand.Bot.Services;

namespace DiscordVoiceChannelsOnDemand.Bot.Commands
{
    [Group("space")]
    public class SpaceCommands : ModuleBase<SocketCommandContext>
    {
        private readonly IServerService _serverService;
        private readonly ISpaceService _spaceService;

        public SpaceCommands(IServerService serverService, ISpaceService spaceService)
        {
            _serverService = serverService;
            _spaceService = spaceService;
        }

        [Command("create")]
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

            // Create new text channel
            var channel = await _spaceService.CreateSpaceAsync(Context.User as IGuildUser, users);

            // Reply
            await ReplyAsync($"Your space is ready to go: {channel.Mention}");
        }
    }
}