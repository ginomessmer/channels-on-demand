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

        public SpaceCommands(IServerService serverService)
        {
            _serverService = serverService;
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

            // Set up permissions (add all users)

            // Modify by moving to space category

            // Insert in database

            // Reply
        }
    }
}