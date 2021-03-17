using System.ComponentModel.DataAnnotations;
using LiteDB;

namespace DiscordVoiceChannelsOnDemand.Bot.Models
{
    public class Slot
    {
        [Key]
        [BsonId]
        public string TriggerVoiceChannelId { get; set; }

        public string CategoryId { get; set; }
    }
}