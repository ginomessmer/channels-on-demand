using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;

namespace DiscordVoiceChannelsOnDemand.Bot.Services
{
    public class TenantService : ITenantService
    {
        private readonly IDiscordClient _client;

        public TenantService(IDiscordClient client)
        {
            _client = client;
        }

        #region Implementation of ITenantService

        /// <inheritdoc />
        public async Task<IEnumerable<IGuild>> GetAllGuildsAsync()
        {
            var guilds = await _client.GetGuildsAsync();
            return guilds;
        }

        #endregion
    }
}