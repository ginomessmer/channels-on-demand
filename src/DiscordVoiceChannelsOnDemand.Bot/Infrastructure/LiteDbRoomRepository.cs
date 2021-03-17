using DiscordVoiceChannelsOnDemand.Bot.Models;
using LiteDB;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DiscordVoiceChannelsOnDemand.Bot.Infrastructure
{
    public class LiteDbRoomRepository : IRoomRepository
    {
        private readonly ILiteDatabase _database;

        public LiteDbRoomRepository(ILiteDatabase database)
        {
            _database = database;
        }

        /// <inheritdoc />
        public Task<Room> AddAsync(string voiceChannelId, string hostUserId, string guildId)
        {
            if (Collection.Exists(x => x.ChannelId == voiceChannelId.ToString()))
                throw new DuplicateNameException("The voice channel was already inserted");

            var room = new Room(voiceChannelId, hostUserId, guildId);
            Collection.Insert(room);

            Collection.EnsureIndex(x => x.ChannelId);
            Collection.EnsureIndex(x => x.HostUserId);
            Collection.EnsureIndex(x => x.GuildId);

            return Task.FromResult(room);
        }

        /// <inheritdoc />
        public Task RemoveAsync(string voiceChannelId) => Task.FromResult(Collection.Delete(voiceChannelId));

        /// <inheritdoc />
        public Task<bool> ExistsAsync(string voiceChannelId) => Task.FromResult(Collection.FindById(voiceChannelId) is not null);

        /// <inheritdoc />
        public Task<IEnumerable<Room>> GetAllAsync() => Task.FromResult(Collection.FindAll());

        /// <inheritdoc />
        public Task<IEnumerable<Room>> GetAllAsync(string guildId) =>
            Task.FromResult(Collection.Find(x => x.GuildId == guildId));

        protected ILiteCollection<Room> Collection => _database.GetCollection<Room>();
    }
}