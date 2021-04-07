using System.Threading.Tasks;
using Discord.Commands;

namespace DiscordChannelsOnDemand.Bot.Commands.Modules.Utils
{
    public class PingCommands : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        public async Task Ping()
        {
            await ReplyAsync("Hello world");
        }
    }
}
