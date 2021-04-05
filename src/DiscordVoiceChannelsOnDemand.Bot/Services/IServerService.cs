using System;
using Discord;
using DiscordVoiceChannelsOnDemand.Bot.Data;
using DiscordVoiceChannelsOnDemand.Bot.Models;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using DiscordVoiceChannelsOnDemand.Bot.Abstractions;

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
        Task<Lobby> RegisterLobbyAsync([NotNull] IVoiceChannel voiceChannel, ICategoryChannel categoryChannel = null);

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userVoiceChannel"></param>
        /// <returns></returns>
        Task<Lobby> GetLobbyAsync(IVoiceChannel userVoiceChannel);

        /// <summary>
        /// Enables spaces on the server.
        /// </summary>
        /// <param name="parentCategoryChannel"></param>
        /// <returns></returns>
        Task EnableSpacesAsync(ICategoryChannel parentCategoryChannel);

        /// <summary>
        /// Disables the spaces feature on the server.
        /// </summary>
        /// <param name="guild"></param>
        /// <returns></returns>
        Task DisableSpacesAsync(IGuild guild);

        /// <summary>
        /// Configures space options for a server.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        Task ConfigureSpaceAsync(ILobby lobby, Action<LobbySpaceConfiguration> configuration);
    }
}