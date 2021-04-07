using Discord;

namespace DiscordChannelsOnDemand.Bot.Core.Extensions
{
    public static class UserExtensions
    {
        public static string GetPreferredName(this IGuildUser user) => 
            string.IsNullOrEmpty(user.Nickname) ? user.Username : user.Nickname;
    }
}
