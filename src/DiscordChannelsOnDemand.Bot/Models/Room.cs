using System.ComponentModel.DataAnnotations;

namespace DiscordChannelsOnDemand.Bot.Models
{
    public class Room
    {
        /// <summary>
        /// The voice channel, duh.
        /// </summary>
        [Key]
        public string ChannelId { get; set; }

        /// <summary>
        /// The user who requested the voice channel.
        /// </summary>
        public string HostUserId { get; set; }

        /// <summary>
        /// The voice channel's server ID.
        /// </summary>
        public string GuildId { get; set; }

        /// <summary>
        /// An optional space linked to the room.
        /// </summary>
        public Space LinkedSpace { get; set; }

        public Room(ulong channelId, ulong hostUserId, ulong guildId) 
            : this(channelId.ToString(), hostUserId.ToString(), guildId.ToString())
        {
        }

        public Room(string channelId, string hostUserId, string guildId)
        {
            ChannelId = channelId;
            HostUserId = hostUserId;
            GuildId = guildId;
        }

        public Room()
        {
        }

        public bool HasSpace => LinkedSpace is not null;
    }
}