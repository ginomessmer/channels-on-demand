using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DiscordVoiceChannelsOnDemand.Bot.Models
{
    public class Server
    {
        /// <summary>
        /// The guild's ID. This is the primary identifier of the server.
        /// </summary>
        [Key]
        public string GuildId { get; set; }

        // Lobbies

        /// <summary>
        /// All active lobbies created on the server.
        /// </summary>
        public ICollection<Lobby> Lobbies { get; set; } = new List<Lobby>();

        /// <summary>
        /// Space configuration for this server.
        /// </summary>
        public ServerSpaceConfiguration SpaceConfiguration { get; set; } = new ServerSpaceConfiguration();

        /// <summary>
        /// All active spaces created on the server.
        /// </summary>
        public ICollection<Space> Spaces { get; set; } = new List<Space>();

        public Lobby GetLobby(string id) => Lobbies.FirstOrDefault(x => x.TriggerVoiceChannelId == id);
    }
}