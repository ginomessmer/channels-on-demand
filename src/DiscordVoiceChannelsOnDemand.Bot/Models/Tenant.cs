using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using LiteDB;

namespace DiscordVoiceChannelsOnDemand.Bot.Models
{
    public class Tenant
    {
        [Key]
        [BsonId]
        public string GuildId { get; set; }

        public ICollection<Slot> Slots { get; set; } = new List<Slot>();

        public Slot GetSlot(string id) => Slots.FirstOrDefault(x => x.TriggerVoiceChannelId == id);
    }
}