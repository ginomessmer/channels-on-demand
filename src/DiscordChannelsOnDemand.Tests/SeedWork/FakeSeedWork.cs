using Discord;
using System.Collections.Generic;
using System.Linq;

namespace DiscordChannelsOnDemand.Tests.SeedWork
{
    public static class FakeSeedWork
    {
        public static IList<IGuildUser> GuildUsers { get; }

        static FakeSeedWork()
        {
            var faker = new GuildUserFake();
            faker.UseSeed(1);

            GuildUsers = faker.Generate(1000).Cast<IGuildUser>().ToList();
        }
    }
}