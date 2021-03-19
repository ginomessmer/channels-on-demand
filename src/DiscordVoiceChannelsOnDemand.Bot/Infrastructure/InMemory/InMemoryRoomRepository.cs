﻿using DiscordVoiceChannelsOnDemand.Bot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DiscordVoiceChannelsOnDemand.Bot.Infrastructure.InMemory
{
    [Obsolete]
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
        public Task AddAsync(Room item) => Task.Run(() => _rooms.Add(item));

        /// <inheritdoc />
        public Task RemoveAsync(string id) => 
            Task.FromResult(_rooms.RemoveAll(x => x.ChannelId == id));

        /// <inheritdoc />
        public Task<bool> ExistsAsync(string id) =>
            Task.FromResult(_rooms.Exists(x => x.ChannelId == id));

        /// <inheritdoc />
        public Task UpdateAsync(Room item)
        {
            _rooms.RemoveAll(x => x.ChannelId == item.ChannelId);
            _rooms.Add(item);
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task<IEnumerable<Room>> GetAllAsync() =>
            Task.FromResult(_rooms as IEnumerable<Room>);

        /// <inheritdoc />
        public Task<Room> GetAsync(string id) => Task.FromResult(_rooms.SingleOrDefault(x => x.ChannelId == id));

        /// <inheritdoc />
        public Task<IEnumerable<Room>> QueryAsync(Expression<Func<Room, bool>> expression) => Task.FromResult(_rooms.Where(expression.Compile()));

        /// <inheritdoc />
        public Task<IEnumerable<Room>> GetAllAsync(string guildId) =>
            Task.FromResult(_rooms.Where(x => x.GuildId == guildId));

        #endregion
    }
}