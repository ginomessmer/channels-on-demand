using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiscordChannelsOnDemand.Bot.Models;
using Microsoft.EntityFrameworkCore;

namespace DiscordChannelsOnDemand.Bot.Core.Infrastructure.EntityFramework
{
    public class EfCoreServerRepository : EfCoreRepository<Server>, IServerRepository
    {
        /// <inheritdoc />
        public EfCoreServerRepository(BotDbContext botDbContext) : base(botDbContext)
        {
        }

        /// <inheritdoc />
        public override Task<Server> GetAsync(string id) => Set
            .Include(x => x.Lobbies)
            .FirstOrDefaultAsync(x => x.GuildId == id);

        /// <inheritdoc />
        public async Task<IEnumerable<Lobby>> QueryAllLobbiesAsync()
        {
            var lobbies = await Set.AsQueryable().SelectMany(x => x.Lobbies).ToListAsync();
            return lobbies;
        }

        /// <inheritdoc />
        public async Task<Lobby> FindLobbyAsync(string voiceChannelId)
        {
            var lobby = await Set.Include(x => x.Lobbies)
                .SelectMany(x => x.Lobbies)
                .SingleOrDefaultAsync(x => x.TriggerVoiceChannelId == voiceChannelId);
            
            return lobby;
        }

        /// <inheritdoc />
        public Task DeleteLobbyAsync(string voiceChannelId)
        {
            throw new NotImplementedException();
        }
    }
}