using System.Collections.Generic;
using DiscordVoiceChannelButler.Bot.Infrastructure;
using DiscordVoiceChannelButler.Bot.Models;
using LiteDB;

namespace DiscordVoiceChannelButler.Tests.SeedWork
{
    public class LiteDbRoomRepositoryTraversal : LiteDbRoomRepository
    {
        /// <inheritdoc />
        public LiteDbRoomRepositoryTraversal(LiteDatabase database) : base(database)
        {
        }

        public IEnumerable<Room> GetAll() => Collection.FindAll();
    }
}