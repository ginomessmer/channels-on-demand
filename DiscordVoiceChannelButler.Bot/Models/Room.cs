using System.ComponentModel.DataAnnotations;

namespace DiscordVoiceChannelButler.Bot.Models
{
    public class Room
    {
        [Key]
        public string ChannelId { get; set; }

        public string HostUserId { get; set; }

        public Room(ulong channelId, ulong hostUserId)
        {
            ChannelId = channelId.ToString();
            HostUserId = hostUserId.ToString();
        }

        public Room()
        {
        }
    }
}