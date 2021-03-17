using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using DiscordVoiceChannelsOnDemand.Bot.Infrastructure;
using DiscordVoiceChannelsOnDemand.Bot.Models;
using DiscordVoiceChannelsOnDemand.Bot.Services;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace DiscordVoiceChannelsOnDemand.Tests
{
    public class RoomServiceUnitTests
    {
        [Fact]
        public async Task RoomService_CreateRoom_EndUpInRepository()
        {
            // Arrange
            var roomRepository = new InMemoryRoomRepository();
            
            // Mock => Server Repository
            var serverRepositoryMock = new Mock<IServerRepository>();
            {
                serverRepositoryMock.Setup(x => x.FindLobbyAsync(It.IsAny<string>()))
                    .ReturnsAsync(() => new Lobby {CategoryId = "1111"});
            }

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
                userMock.Setup(x => x.Id).Returns(guildMock.Object.Id);
                userMock.Setup(x => x.Nickname).Returns("Martha");
                userMock.Setup(x => x.Guild).Returns(guildMock.Object);
                userMock.Setup(x => x.VoiceChannel.Id).Returns(It.IsAny<ulong>());
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
            var service = new RoomService(discordClientMock.Object, roomRepository, serverRepositoryMock.Object);

            // Act
            var room = await service.CreateNewRoomAsync(userMock.Object);

            // Assert
            Assert.Single(await roomRepository.GetAllAsync());
        }
    }
}