using System;
using System.Threading.Tasks;
using Discord;
using DiscordVoiceChannelsOnDemand.Bot.Infrastructure;
using DiscordVoiceChannelsOnDemand.Bot.Services;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace DiscordVoiceChannelsOnDemand.Tests
{
    public class RoomServiceUnitTests
    {
        [Fact]
        public async Task Test()
        {
            // Arrange
            var repository = new InMemoryRoomRepository();

            // Mock => Voice Channel
            var voiceChannelMock = new Mock<IVoiceChannel>();
            {
                voiceChannelMock.SetupGet(x => x.Id).Returns(2222);
            }

            // Mock => Guild
            var guildMock = new Mock<IGuild>();
            {
                guildMock.Setup(x => x.Id).Returns(1111);
                guildMock.Setup(x => x.CreateVoiceChannelAsync(It.IsAny<string>(), It.IsAny<Action<VoiceChannelProperties>>(), It.IsAny<RequestOptions>()))
                    .ReturnsAsync(() => voiceChannelMock.Object);
            }

            // Mock => User
            var userMock = new Mock<IGuildUser>();
            {
                userMock.Setup(x => x.Guild).Returns(guildMock.Object);
                userMock.Setup(x => x.Id).Returns(guildMock.Object.Id);
                userMock.Setup(x => x.Nickname).Returns("Martha");
            }

            // Mock => Client
            var discordClientMock = new Mock<IDiscordClient>();
            {
                discordClientMock.Setup(x => x.GetGuildAsync(
                        It.IsAny<ulong>(), 
                        It.IsAny<CacheMode>(), 
                        It.IsAny<RequestOptions>()))
                    .ReturnsAsync(guildMock.Object);
            }

            // Service
            var service = new RoomService(discordClientMock.Object, repository, null);

            // Act
            var room = await service.CreateNewRoomAsync(userMock.Object);

            // Assert
            Assert.Single(await repository.GetAllAsync());
        }
    }
}