using Discord;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord.WebSocket;
using DiscordVoiceChannelsOnDemand.Bot.Data;
using DiscordVoiceChannelsOnDemand.Bot.Models;

namespace DiscordVoiceChannelsOnDemand.Bot.Services
{
    public interface IServerService
    {
        Task<IEnumerable<IGuild>> GetAllGuildsAsync();

        /// <summary>
        /// Checks whether a lobby is registered that is linked to <paramref name="voiceChannel"/>.
        /// </summary>
        /// <param name="voiceChannel"></param>
        /// <returns></returns>
        Task<bool> IsLobbyRegisteredAsync(IVoiceChannel voiceChannel);

        /// <summary>
        /// Checks whether the guild is registered as a server.
        /// </summary>
        /// <param name="guild"></param>
        /// <returns></returns>
        Task<bool> IsRegisteredAsync(IGuild guild);

        /// <summary>
        /// Registers a new server from a guild.
        /// </summary>
        /// <param name="guild"></param>
        /// <returns></returns>
        Task RegisterAsync(IGuild guild);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guild"></param>
        /// <returns></returns>
        Task DeregisterAsync(IGuild guild);

        /// <summary>
        /// Returns the server that is linked to <paramref name="guild"/>.
        /// </summary>
        /// <param name="guild"></param>
        /// <returns></returns>
        Task<Server> GetAsync(IGuild guild);

        /// <summary>
        /// Registers a new lobby.
        /// </summary>
        /// <param name="voiceChannel"></param>
        /// <param name="categoryChannel"></param>
        /// <returns></returns>
        Task<Lobby> RegisterLobbyAsync(IVoiceChannel voiceChannel, ICategoryChannel categoryChannel);

        /// <summary>
        /// Deregisters a lobby.
        /// </summary>
        /// <param name="voiceChannel"></param>
        /// <returns></returns>
        Task DeregisterLobbyAsync(IVoiceChannel voiceChannel);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guild"></param>
        /// <returns></returns>
        Task<IEnumerable<LobbyResult>> ListLobbiesAsync(IGuild guild);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="voiceChannel"></param>
        /// <param name="names"></param>
        /// <returns></returns>
        Task ConfigureLobbySuggestedNamesAsync(IVoiceChannel voiceChannel, params string[] names);
    }
}