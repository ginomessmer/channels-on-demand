using LiteDB;
using System;

namespace DiscordVoiceChannelsOnDemand.Tests.SeedWork
{
    public class LiteDbFactory
    {
        public LiteDatabase CreateDatabase() => new($"{Guid.NewGuid()}.db");
    }
}