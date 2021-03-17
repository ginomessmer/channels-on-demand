using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiscordVoiceChannelsOnDemand.Bot.Models;
using LiteDB;

namespace DiscordVoiceChannelsOnDemand.Bot.Infrastructure
{
    public class LiteDbServerRepository : LiteDbRepositoryBase<Server>, IServerRepository
    {
        /// <inheritdoc />
        public LiteDbServerRepository(ILiteDatabase database) : base(database)
        {
        }

        #region Overrides of LiteDbRepositoryBase<Server>

        /// <inheritdoc />
        public override Task AddAsync(Server item)
        {
            Collection.EnsureIndex(x => x.GuildId);
            return base.AddAsync(item);
        }

        #endregion

        #region Implementation of IServerRepository

        /// <inheritdoc />
        public Task<IEnumerable<Lobby>> QueryAllLobbysAsync() => 
            Task.FromResult(Collection.FindAll().SelectMany(x => x.Lobbys));

        /// <inheritdoc />
        public async Task<bool> LobbysExistsAsync(string voiceChannelId)
        {
            var lobby = await FindLobbyAsync(voiceChannelId);
            return lobby is not null;
        }

        /// <inheritdoc />
        public async Task<Lobby> FindLobbyAsync(string voiceChannelId)
        {
            var lobbys = await QueryAllLobbysAsync();
            var lobby = lobbys.FirstOrDefault(x => x.TriggerVoiceChannelId == voiceChannelId);
            return lobby;
        }

        /// <inheritdoc />
        public async Task DeleteLobbyAsync(string voiceChannelId)
        {
            var server = Collection.Find(x => x.Lobbys.SingleOrDefault(x => x.TriggerVoiceChannelId == voiceChannelId) != null).Single();
            var lobby = server.Lobbys.Single(x => x.TriggerVoiceChannelId == voiceChannelId);

            var isSuccess = server.Lobbys.Remove(lobby);
            await UpdateAsync(server);
        }

        #endregion
    }
}