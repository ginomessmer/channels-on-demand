using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;

namespace DiscordVoiceChannelsOnDemand.Bot.Commands
{
    public class PingCommand : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        public async Task Ping()
        {
            await ReplyAsync("Hello world");
        }
    }
}
