using System.Collections.Generic;
using System.Threading.Tasks;
using DiscordChannelsOnDemand.Bot.Models;

namespace DiscordChannelsOnDemand.Bot.Core.Infrastructure
{
    public interface IServerRepository : IRepository<Server>
    {
        /// <summary>
        /// Queries all lobbies across all servers.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Lobby>> QueryAllLobbiesAsync();

        /// <summary>
        /// Finds a lobby.
        /// </summary>
        /// <param name="voiceChannelId"></param>
        /// <returns></returns>
        Task<Lobby> FindLobbyAsync(string voiceChannelId);

        /// <summary>
        /// Deletes a lobby.
        /// </summary>
        /// <param name="voiceChannelId"></param>
        /// <returns></returns>
        Task DeleteLobbyAsync(string voiceChannelId);
    }
}