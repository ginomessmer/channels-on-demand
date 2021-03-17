using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using LiteDB;

namespace DiscordVoiceChannelsOnDemand.Bot.Models
{
    public class Server
    {
        [Key]
        [BsonId]
        public string GuildId { get; set; }

        public ICollection<Lobby> Lobbies { get; set; } = new List<Lobby>();

        public Lobby GetLobby(string id) => Lobbies.FirstOrDefault(x => x.TriggerVoiceChannelId == id);
    }
}