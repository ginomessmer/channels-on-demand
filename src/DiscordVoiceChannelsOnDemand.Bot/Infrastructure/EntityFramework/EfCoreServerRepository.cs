using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DiscordVoiceChannelsOnDemand.Bot.Models;

namespace DiscordVoiceChannelsOnDemand.Bot.Infrastructure.EntityFramework
{
    public class EfCoreServerRepository : EfCoreRepository<Server>, IServerRepository
    {
        /// <inheritdoc />
        public EfCoreServerRepository(BotDbContext botDbContext) : base(botDbContext)
        {
        }
        
        /// <inheritdoc />
        public Task<IEnumerable<Lobby>> QueryAllLobbiesAsync()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<Lobby> FindLobbyAsync(string voiceChannelId)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task DeleteLobbyAsync(string voiceChannelId)
        {
            throw new NotImplementedException();
        }
    }
}