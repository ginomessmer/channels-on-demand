using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;

namespace DiscordChannelsOnDemand.Bot.Extensions
{
    public static class UserExtensions
    {
        public static string GetPreferredName(this IGuildUser user) => 
            string.IsNullOrEmpty(user.Nickname) ? user.Username : user.Nickname;
    }
}
