using System.ComponentModel.DataAnnotations;
using LiteDB;

namespace DiscordVoiceChannelsOnDemand.Bot.Models
{
    public class Slot
    {
        [Key]
        [BsonId]
        public string GatewayChannelId { get; set; }

        public string CategoryId { get; set; }
    }
}