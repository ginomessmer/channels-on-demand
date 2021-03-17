using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;

namespace DiscordVoiceChannelsOnDemand.Bot.Services
{
    public class ServerService : IServerService
    {
        private readonly IDiscordClient _client;

        public ServerService(IDiscordClient client)
        {
            _client = client;
        }

        #region Implementation of IServerService

        /// <inheritdoc />
        public async Task<IEnumerable<IGuild>> GetAllGuildsAsync()
        {
            var guilds = await _client.GetGuildsAsync();
            return guilds;
        }

        #endregion
    }
}