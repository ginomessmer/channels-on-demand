using Discord;
using DiscordVoiceChannelsOnDemand.Bot.Data;
using DiscordVoiceChannelsOnDemand.Bot.Infrastructure;
using DiscordVoiceChannelsOnDemand.Bot.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordVoiceChannelsOnDemand.Bot.Services
{
    public class ServerService : IServerService
    {
        private readonly IDiscordClient _client;
        private readonly IServerRepository _serverRepository;

        public ServerService(IDiscordClient client,
            IServerRepository serverRepository)
        {
            _client = client;
            _serverRepository = serverRepository;
        }

        #region Implementation of IServerService

        /// <inheritdoc />
        public async Task<IEnumerable<IGuild>> GetAllGuildsAsync()
        {
            var guilds = await _client.GetGuildsAsync();
            return guilds;
        }

        /// <inheritdoc />
        public async Task<bool> IsLobbyRegisteredAsync([NotNull] IVoiceChannel voiceChannel)
        {
            var lobby = await _serverRepository.FindLobbyAsync(voiceChannel.Id.ToString());
            return lobby is not null;
        }

        /// <inheritdoc />
        public async Task<bool> IsRegisteredAsync(IGuild guild)
        {
            var result = await _serverRepository.QueryAsync(x => x.GuildId == guild.Id.ToString());
            return result.SingleOrDefault() is not null;
        }

        /// <inheritdoc />
        public async Task RegisterAsync(IGuild guild)
        {
            await _serverRepository.AddAsync(new Server
            {
                GuildId = guild.Id.ToString(),
                Lobbies = new List<Lobby>()
            });

            await _serverRepository.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task DeregisterAsync(IGuild guild)
        {
            await _serverRepository.RemoveAsync(guild.Id.ToString());

            await _serverRepository.SaveChangesAsync();
        }

        /// <inheritdoc />
        public Task<Server> GetAsync(IGuild guild)
        {
            return _serverRepository.GetAsync(guild.Id.ToString());
        }

        /// <inheritdoc />
        public async Task<Lobby> RegisterLobbyAsync([NotNull] IVoiceChannel voiceChannel, ICategoryChannel categoryChannel = null)
        {
            var server = await GetAsync(voiceChannel.Guild);
            var lobby = new Lobby
            {
                TriggerVoiceChannelId = voiceChannel.Id.ToString(),
                CategoryId = categoryChannel?.Id.ToString()
            };

            server.Lobbies.Add(lobby);
            await _serverRepository.UpdateAsync(server);

            await _serverRepository.SaveChangesAsync();

            return lobby;
        }

        /// <inheritdoc />
        public async Task DeregisterLobbyAsync(IVoiceChannel voiceChannel)
        {
            var lobby = await _serverRepository.FindLobbyAsync(voiceChannel.Id.ToString());
            await _serverRepository.DeleteLobbyAsync(voiceChannel.Id.ToString());

            await _serverRepository.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<LobbyResult>> ListLobbiesAsync(IGuild guild)
        {
            var server = await _serverRepository.GetAsync(guild.Id.ToString());
            var lobbies = server.Lobbies;
            var categories = await guild.GetCategoriesAsync();

            var results = new List<LobbyResult>();
            foreach (var lobby in lobbies)
            {
                var voiceChannel = await guild.GetVoiceChannelAsync(Convert.ToUInt64(lobby.TriggerVoiceChannelId));
                var categoryChannel = categories.SingleOrDefault(x => x.Id == Convert.ToUInt64(lobby.CategoryId));

                results.Add(new LobbyResult(voiceChannel, categoryChannel));
            }

            return results;
        }

        /// <inheritdoc />
        public async Task ConfigureLobbySuggestedNamesAsync(IVoiceChannel voiceChannel, params string[] names)
        {
            var server = await _serverRepository.GetAsync(voiceChannel.Guild.Id.ToString());
            var lobby = server.GetLobby(voiceChannel.Id.ToString());

            lobby.RoomNames = names;

            await _serverRepository.UpdateAsync(server);

            await _serverRepository.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task<Lobby> GetLobbyAsync(IVoiceChannel userVoiceChannel)
        {
            if (userVoiceChannel is null)
                return null;

            var lobby = await _serverRepository.FindLobbyAsync(userVoiceChannel.Id.ToString());
            return lobby;
        }

        /// <inheritdoc />
        public async Task EnableSpacesAsync(ICategoryChannel parentCategoryChannel)
        {
            var id = parentCategoryChannel.Id.ToString();
            await ToggleSpacesAsync(parentCategoryChannel.Guild, id);
        }
        
        /// <inheritdoc />
        public async Task DisableSpacesAsync(IGuild guild)
        {
            await ToggleSpacesAsync(guild);
        }

        /// <summary>
        /// Toggles the spaces feature.
        /// </summary>
        /// <param name="guild">The guild server to operate on</param>
        /// <param name="spaceCategoryId">The category to use for new spaces. Leave blank to disable the feature</param>
        /// <returns></returns>
        private async Task ToggleSpacesAsync(IGuild guild, string spaceCategoryId = "")
        {
            var server = await _serverRepository.GetAsync(guild.Id.ToString());
            server.SpaceConfiguration.SpaceCategoryId = spaceCategoryId;
            await _serverRepository.UpdateAsync(server);
            await _serverRepository.SaveChangesAsync();
        }

        #endregion
    }
}