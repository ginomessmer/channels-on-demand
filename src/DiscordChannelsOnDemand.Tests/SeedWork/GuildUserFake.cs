using Bogus;

namespace DiscordChannelsOnDemand.Tests.SeedWork
{
    public sealed class GuildUserFake : Faker<GuildUserStub>
    {
        public GuildUserFake()
        {
            RuleFor(x => x.Id, f => f.Random.ULong());
            RuleFor(x => x.Username, f => f.Internet.UserName());
        }
    }
}
