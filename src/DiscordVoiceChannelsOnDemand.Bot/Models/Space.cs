using System.ComponentModel.DataAnnotations;

namespace DiscordVoiceChannelsOnDemand.Bot.Models
{
    public class Space
    {
        [Key]
        public string TextChannelId { get; set; }

        public string CreatorId { get; set; }
    }
}