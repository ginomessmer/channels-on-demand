using Discord;
using DiscordVoiceChannelsOnDemand.Bot.Infrastructure;
using DiscordVoiceChannelsOnDemand.Bot.Models;
using DiscordVoiceChannelsOnDemand.Bot.Services;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace DiscordVoiceChannelsOnDemand.Tests
{
    public class ServerServiceUnitTests
    {
        [Fact]
        public async Task ServerService_RegisteredServers_FindSuccessfully()
        {
            // Arrange
            // Mock => IVoiceChannel
            var voiceChannelMock = new Mock<IVoiceChannel>();

            // Mock => IDiscordClient
            var discordClientMock = new Mock<IDiscordClient>();

            // Mock => IServerRepository
            var serverRepositoryMock = new Mock<IServerRepository>();
            serverRepositoryMock.Setup(x => x.FindLobbyAsync(voiceChannelMock.Object.Id.ToString()))
                .ReturnsAsync(() => new Lobby());

            // Server service
            var serverService = new ServerService(discordClientMock.Object, serverRepositoryMock.Object);


            // Act
            var result = await serverService.IsLobbyRegisteredAsync(voiceChannelMock.Object);


            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task ServerService_RegisteredServers_MissingSuccessfully()
        {
            // Arrange
            // Mock => IVoiceChannel
            var voiceChannelMock = new Mock<IVoiceChannel>();
            voiceChannelMock.Setup(x => x.Id).Returns(1);

            var placeboVoiceChannelMock = new Mock<IVoiceChannel>();
            placeboVoiceChannelMock.Setup(x => x.Id).Returns(2);

            // Mock => IDiscordClient
            var discordClientMock = new Mock<IDiscordClient>();

            // Mock => IServerRepository
            var serverRepositoryMock = new Mock<IServerRepository>();
            serverRepositoryMock.Setup(x => x.FindLobbyAsync(placeboVoiceChannelMock.Object.Id.ToString()))
                .ReturnsAsync(() => new Lobby());

            // Server service
            var serverService = new ServerService(discordClientMock.Object, serverRepositoryMock.Object);


            // Act
            var result = await serverService.IsLobbyRegisteredAsync(voiceChannelMock.Object);


            // Assert
            Assert.False(result);
        }
    }
}