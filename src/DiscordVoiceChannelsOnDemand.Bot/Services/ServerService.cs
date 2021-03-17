using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using DiscordVoiceChannelsOnDemand.Bot.Infrastructure;
using DiscordVoiceChannelsOnDemand.Bot.Models;

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
        }

        #endregion
    }
}