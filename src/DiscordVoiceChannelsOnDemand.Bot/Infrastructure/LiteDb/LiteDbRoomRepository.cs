using DiscordVoiceChannelsOnDemand.Bot.Models;
using LiteDB;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DiscordVoiceChannelsOnDemand.Bot.Infrastructure.LiteDb
{
    public class LiteDbRoomRepository : LiteDbRepositoryBase<Room>, IRoomRepository
    {
        public LiteDbRoomRepository(ILiteDatabase database) : base(database)
        {
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
        public Task<IEnumerable<Room>> GetAllAsync(string guildId) =>
            Task.FromResult(Collection.Find(x => x.GuildId == guildId));
    }
}