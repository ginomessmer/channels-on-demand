using System.ComponentModel.DataAnnotations;
using LiteDB;

namespace DiscordVoiceChannelButler.Bot.Models
{
    public class Room
    {
        [Key]
        [BsonId]
        public string ChannelId { get; set; }

        public string HostUserId { get; set; }

        public Room(ulong channelId, ulong hostUserId) : this(channelId.ToString(), hostUserId.ToString())
        {
        }

        public Room(string channelId, string hostUserId)
        {
            ChannelId = channelId;
            HostUserId = hostUserId;
        }

        public Room()
        {
        }
    }
}