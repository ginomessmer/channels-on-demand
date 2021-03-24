using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DiscordVoiceChannelsOnDemand.Bot.Models
{
    public class Server
    {
        [Key]
        public string GuildId { get; set; }

        public ICollection<Lobby> Lobbies { get; set; } = new List<Lobby>();

        public ICollection<Space> Spaces { get; set; } = new List<Space>();

        public Lobby GetLobby(string id) => Lobbies.FirstOrDefault(x => x.TriggerVoiceChannelId == id);
    }
}