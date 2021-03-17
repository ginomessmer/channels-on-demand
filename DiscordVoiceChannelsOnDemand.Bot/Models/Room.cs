using System.ComponentModel.DataAnnotations;
using LiteDB;

namespace DiscordVoiceChannelsOnDemand.Bot.Models
{
    public class Room
    {
        /// <summary>
        /// The voice channel, duh.
        /// </summary>
        [Key]
        [BsonId]
        public string ChannelId { get; set; }

        /// <summary>
        /// The user who requested the voice channel.
        /// </summary>
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