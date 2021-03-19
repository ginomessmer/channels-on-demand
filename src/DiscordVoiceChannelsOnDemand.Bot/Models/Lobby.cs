using LiteDB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using DiscordVoiceChannelsOnDemand.Bot.Abstractions;

namespace DiscordVoiceChannelsOnDemand.Bot.Models
{
    public class Lobby : ILobby
    {
        /// <inheritdoc />
        [Key]
        [BsonId]
        [Required]
        public string TriggerVoiceChannelId { get; set; }

        /// <inheritdoc />
        public string CategoryId { get; set; }

        public ICollection<string> RoomNames { get; set; } = new List<string> {"{0}"};

        public Lobby()
        {
        }

        public Lobby(string triggerVoiceChannelId, string categoryId)
        {
            TriggerVoiceChannelId = triggerVoiceChannelId;
            CategoryId = categoryId;
        }

        public Lobby(string triggerVoiceChannelId) : this(triggerVoiceChannelId, string.Empty)
        {
        }

        /// <inheritdoc />
        public string SuggestRoomName() => RoomNames.OrderBy(_ => Guid.NewGuid().ToString()).FirstOrDefault();

        /// <inheritdoc />
        [IgnoreDataMember]
        [BsonIgnore]
        public bool HasCategory => !string.IsNullOrEmpty(CategoryId);
    }
}