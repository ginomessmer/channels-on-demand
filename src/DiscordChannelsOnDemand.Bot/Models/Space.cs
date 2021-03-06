using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiscordChannelsOnDemand.Bot.Models
{
    public class Space
    {
        [Key]
        public string TextChannelId { get; set; }

        public string CreatorId { get; set; }

        // Server

        [ForeignKey(nameof(ServerId))]
        public Server Server { get; set; }

        public string ServerId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}