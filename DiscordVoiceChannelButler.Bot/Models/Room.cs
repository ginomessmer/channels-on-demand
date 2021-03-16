namespace DiscordVoiceChannelButler.Bot.Models
{
    public class Room
    {
        public ulong ChannelId { get; set; }

        public ulong HostUserId { get; set; }

        public Room(ulong channelId, ulong hostUserId)
        {
            ChannelId = channelId;
            HostUserId = hostUserId;
        }
    }
}