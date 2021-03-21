using System;
using Discord;
using Discord.Commands;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordVoiceChannelsOnDemand.Bot.Commands
{
    [Group("help")]
    [Summary("Retrieves a list of all modules")]
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
            var modules = _commandService.Modules
                .Select(x => new EmbedFieldBuilder()
                .WithName(x.Name)
                .WithValue(x.Summary ?? "N/A")
                .WithIsInline(true));

            var embed = new EmbedBuilder()
                .WithTitle("Discord Voice Channels on Demand")
                .WithDescription("Type `.help <module>` to inspect the module")
                .WithUrl("https://github.com/ginomessmer/discord-vcod/")
                .WithFooter("Like what you are using? Consider contributing or buying a coffee")
                .WithFields(modules);

            await ReplyAsync(embed: embed.Build());
        }

        [Command]
        public async Task Help(string moduleName)
        {
            var module = _commandService.Modules.FirstOrDefault(x =>
                x.Name.Equals(moduleName, StringComparison.InvariantCultureIgnoreCase));

            if (module is null)
                return;

            var fields = module.Commands.Select(x => new EmbedFieldBuilder()
                .WithName(string.Format("`{0}`", $"{x.Name} " +
                          $"{string.Join(' ', x.Parameters.Select(p => p.IsOptional ? $"<{p.Name}>" : $"[{p.Name.ToUpper()}]"))}"))
                .WithValue(x.Summary ?? "N/A"));

            var embed = new EmbedBuilder()
                .WithTitle($"Commands for `.{module.Name}`")
                .WithFields(fields);

            await ReplyAsync(embed: embed.Build());
        }
    }
}