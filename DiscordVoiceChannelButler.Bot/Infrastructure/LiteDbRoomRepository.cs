using System;
using System.Data;
using System.Threading.Tasks;
using DiscordVoiceChannelButler.Bot.Models;
using LiteDB;

namespace DiscordVoiceChannelButler.Bot.Infrastructure
{
    public class LiteDbRoomRepository : IRoomRepository
    {
        private readonly LiteDatabase _database;

        public LiteDbRoomRepository(LiteDatabase database)
        {
            _database = database;
        }

        /// <inheritdoc />
        public Task<Room> AddAsync(ulong voiceChannelId, ulong hostUserId)
        {
            if (Collection.Exists(x => x.ChannelId == voiceChannelId.ToString()))
                throw new DuplicateNameException("The voice channel was already inserted");

            var room = new Room(voiceChannelId, hostUserId);
            Collection.Insert(room);

            Collection.EnsureIndex(x => x.ChannelId);
            Collection.EnsureIndex(x => x.HostUserId);

            return Task.FromResult(room);
        }

        /// <inheritdoc />
        public Task RemoveAsync(ulong voiceChannelId) => Task.FromResult(Collection.Delete(voiceChannelId));

        /// <inheritdoc />
        public Task<bool> ExistsAsync(ulong voiceChannelId) => Task.FromResult(Collection.FindById(voiceChannelId) is not null);

        protected ILiteCollection<Room> Collection => _database.GetCollection<Room>();
    }
}