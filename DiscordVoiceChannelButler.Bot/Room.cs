using Discord;

namespace DiscordVoiceChannelButler.Bot
{
    public class Room
    {
        public IVoiceChannel Channel { get; set; }

        public IUser Host { get; set; }
    }
}