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

        // Spaces

        /// <summary>
        /// The category's ID for new spaces.
        /// </summary>
        public string SpaceCategoryId { get; set; }

        /// <summary>
        /// All active spaces created on the server.
        /// </summary>
        public ICollection<Space> Spaces { get; set; } = new List<Space>();

        /// <summary>
        /// Determines whether Spaces is enabled on the server.
        /// </summary>
        public bool IsSpacesEnabled => !string.IsNullOrEmpty(SpaceCategoryId);

        public Lobby GetLobby(string id) => Lobbies.FirstOrDefault(x => x.TriggerVoiceChannelId == id);
    }
}