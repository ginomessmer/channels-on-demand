using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiscordVoiceChannelsOnDemand.Bot.Models
{
    public class Space
    {
        [Key]
        public string TextChannelId { get; set; }

        public string CreatorId { get; set; }


        [ForeignKey(nameof(ServerId))]
        public Server Server { get; set; }

        public string ServerId { get; set; }
    }
}