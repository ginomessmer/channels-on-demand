using System.Collections.Generic;
using System.Data;
using System.Linq;
using DiscordVoiceChannelButler.Bot.Models;

namespace DiscordVoiceChannelButler.Bot
{
    public class BotState
    {
        private readonly List<Room> _rooms = new();

        public IReadOnlyCollection<Room> Rooms => _rooms.ToList();

        public void AddRoom(ulong voiceChannelId, ulong hostUserId)
        {
            if (_rooms.Exists(x => x.ChannelId == voiceChannelId))
                throw new DuplicateNameException("The voice channel was already inserted");

            _rooms.Add(new Room(voiceChannelId, hostUserId));
        }

        public bool RemoveRoom(ulong voiceChannelId)
        {
            var room = _rooms.FirstOrDefault(x => x.ChannelId == voiceChannelId);
            return room is not null && _rooms.Remove(room);
        }

        public bool ExistsRoom(ulong voiceChannelId) => _rooms.Exists(x => x.ChannelId == voiceChannelId);
    }
}