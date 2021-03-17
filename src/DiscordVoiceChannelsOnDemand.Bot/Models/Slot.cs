using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using LiteDB;

namespace DiscordVoiceChannelsOnDemand.Bot.Models
{
    public class Slot
    {
        [Key]
        [BsonId]
        public string TriggerVoiceChannelId { get; set; }

        public string CategoryId { get; set; }

        public string NameFormat { get; set; } = "{0}";

        public ICollection<string> RandomNames { get; set; } = new List<string>();

        public string GetSuggestedName()
        {
            return !RandomNames.Any() ? NameFormat : RandomNames.OrderBy(_ => Guid.NewGuid().ToString()).FirstOrDefault();
        }
    }
}