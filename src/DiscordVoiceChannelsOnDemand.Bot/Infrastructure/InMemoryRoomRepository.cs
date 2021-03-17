using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiscordVoiceChannelsOnDemand.Bot.Models;

namespace DiscordVoiceChannelsOnDemand.Bot.Infrastructure
{
    public class InMemoryRoomRepository : IRoomRepository
    {
        private readonly List<Room> _rooms = new List<Room>();

        #region Implementation of IRoomRepository

        /// <inheritdoc />
        public Task<Room> AddAsync(string voiceChannelId, string hostUserId, string guildId)
        {
            var room = new Room(voiceChannelId, hostUserId, guildId);
            _rooms.Add(room);
            return Task.FromResult(room);
        }

        /// <inheritdoc />
        public Task RemoveAsync(string voiceChannelId) => 
            Task.FromResult(_rooms.RemoveAll(x => x.ChannelId == voiceChannelId));

        /// <inheritdoc />
        public Task<bool> ExistsAsync(string voiceChannelId) =>
            Task.FromResult(_rooms.Exists(x => x.ChannelId == voiceChannelId));

        /// <inheritdoc />
        public Task<IEnumerable<Room>> GetAllAsync() =>
            Task.FromResult(_rooms as IEnumerable<Room>);

        /// <inheritdoc />
        public Task<IEnumerable<Room>> GetAllAsync(string guildId) =>
            Task.FromResult(_rooms.Where(x => x.GuildId == guildId));

        #endregion
    }
}