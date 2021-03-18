using LiteDB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DiscordVoiceChannelsOnDemand.Bot.Models
{
    public class Lobby
    {
        /// <summary>
        /// The voice channel's ID that will create a new room on demand.
        /// </summary>
        [Key]
        [BsonId]
        [Required]
        public string TriggerVoiceChannelId { get; set; }

        /// <summary>
        /// The category ID that will be used to append new rooms to.
        /// </summary>
        public string CategoryId { get; set; }

        [Required]
        public string NameFormat { get; set; } = "{0}";

        public ICollection<string> RandomNames { get; set; } = new List<string>();

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

        public string GetSuggestedName()
        {
            return !RandomNames.Any() ? NameFormat : RandomNames.OrderBy(_ => Guid.NewGuid().ToString()).FirstOrDefault();
        }

        public bool HasCategory() => !string.IsNullOrEmpty(CategoryId);
    }
}