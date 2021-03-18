using System;
using Discord.Commands;
using System.Threading.Tasks;

namespace DiscordVoiceChannelsOnDemand.Bot.Commands
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
