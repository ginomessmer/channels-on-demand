using Discord;
using Discord.Commands;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordChannelsOnDemand.Bot.Commands
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
                .WithDescription("Type `.help module <module>` to inspect the module")
                .WithUrl("https://github.com/ginomessmer/channels-on-demand/")
                .WithFooter("Like what you are using? Type `.help support` to contribute")
                .WithFields(modules);

            await ReplyAsync(embed: embed.Build());
        }

        [Command("module")]
        public async Task InspectModule(string moduleName)
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

        [Command("support")]
        [Summary("Shows you ways to support the bot")]
        public async Task Support()
        {
            await ReplyAsync(embed: new EmbedBuilder()
                .WithTitle("View the project on GitHub")
                .WithUrl("https://github.com/ginomessmer/channels-on-demand/")
                .WithFields(
                    new EmbedFieldBuilder().WithName("Contribute")
                        .WithValue("Contribute by suggesting new ideas, submitting issues and creating PRs: https://github.com/ginomessmer/discord-cod"),
                    new EmbedFieldBuilder().WithName("Buy a coffee")
                        .WithValue("Your donation is highly appreciated and keeps the bot running: https://ko-fi.com/ginomessmer"))
                .Build());
        }
    }
}