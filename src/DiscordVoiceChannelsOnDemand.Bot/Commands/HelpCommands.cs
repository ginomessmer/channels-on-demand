using Discord;
using Discord.Commands;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordVoiceChannelsOnDemand.Bot.Commands
{
    [Group("help")]
    public class HelpCommands : ModuleBase<SocketCommandContext>
    {
        private readonly CommandService _commandService;

        public HelpCommands(CommandService commandService)
        {
            _commandService = commandService;
        }

        [Command]
        public async Task Help()
        {
            var commands = _commandService.Commands.Select(x => new EmbedFieldBuilder()
                .WithName($"{x.Module.Name} {x.Name}")
                .WithValue("Test"));

            var embed = new EmbedBuilder().WithFields(commands);
            await ReplyAsync(embed: embed.Build());
        }
    }
}