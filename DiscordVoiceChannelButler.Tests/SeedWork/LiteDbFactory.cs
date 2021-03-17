using System;
using LiteDB;

namespace DiscordVoiceChannelButler.Tests.SeedWork
{
    public class LiteDbFactory
    {
        public LiteDatabase CreateDatabase() => new($"{Guid.NewGuid()}.db");
    }
}