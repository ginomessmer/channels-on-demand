using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using DiscordVoiceChannelsOnDemand.Bot.Infrastructure;

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

        #endregion
    }
}