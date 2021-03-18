using DiscordVoiceChannelsOnDemand.Bot.Models;
using LiteDB;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordVoiceChannelsOnDemand.Bot.Infrastructure.LiteDb
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
        public Task<IEnumerable<Lobby>> QueryAllLobbiesAsync() => 
            Task.FromResult(Collection.FindAll().SelectMany(x => x.Lobbies));

        /// <inheritdoc />
        public async Task<bool> LobbyExistsAsync(string voiceChannelId)
        {
            var lobby = await FindLobbyAsync(voiceChannelId);
            return lobby is not null;
        }

        /// <inheritdoc />
        public async Task<Lobby> FindLobbyAsync(string voiceChannelId)
        {
            var lobbies = await QueryAllLobbiesAsync();
            var lobby = lobbies.FirstOrDefault(x => x.TriggerVoiceChannelId == voiceChannelId);
            return lobby;
        }

        /// <inheritdoc />
        public async Task DeleteLobbyAsync(string voiceChannelId)
        {
            var server = Collection.Find(x => x.Lobbies.SingleOrDefault(x => x.TriggerVoiceChannelId == voiceChannelId) != null).Single();
            var lobby = server.Lobbies.Single(x => x.TriggerVoiceChannelId == voiceChannelId);

            var isSuccess = server.Lobbies.Remove(lobby);
            await UpdateAsync(server);
        }

        #endregion
    }
}