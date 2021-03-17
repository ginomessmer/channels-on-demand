using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LiteDB;

namespace DiscordVoiceChannelsOnDemand.Bot.Models
{
    public class Tenant
    {
        [Key]
        [BsonId]
        public string GuildId { get; set; }

        public ICollection<Slot> Slots { get; set; } = new List<Slot>();



        /// <summary>
        /// The room's naming scheme.
        /// </summary>
        public string RoomNameFormat { get; set; } = "Meeting Room {0}";
    }
}