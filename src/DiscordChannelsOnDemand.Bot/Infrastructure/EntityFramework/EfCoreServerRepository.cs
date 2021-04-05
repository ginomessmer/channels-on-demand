using DiscordChannelsOnDemand.Bot.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordChannelsOnDemand.Bot.Infrastructure.EntityFramework
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
        public Task<Lobby> FindLobbyAsync(string voiceChannelId)
        { 
            return Set.AsQueryable()
                .Include(x => x.Lobbies)
                .Where(x => x.Lobbies.FirstOrDefault(x => x.TriggerVoiceChannelId == voiceChannelId) != null)
                .SelectMany(x => x.Lobbies)
                .SingleOrDefaultAsync();
        }

        /// <inheritdoc />
        public Task DeleteLobbyAsync(string voiceChannelId)
        {
            throw new NotImplementedException();
        }
    }
}