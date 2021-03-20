using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Discord;
using DiscordVoiceChannelsOnDemand.Bot.Infrastructure;
using DiscordVoiceChannelsOnDemand.Bot.Models;
using DiscordVoiceChannelsOnDemand.Bot.Services;
using Moq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Internal;
using Xunit;

namespace DiscordVoiceChannelsOnDemand.Tests
{
    public class ServerServiceUnitTests
    {
        [Fact]
        public async Task ServerService_GetAllGuilds_RetrieveCorrectly()
        {
            // Arrange
            var guild1 = new Mock<IGuild>().Object;
            var guild2 = new Mock<IGuild>().Object;

            var discordClientMock = new Mock<IDiscordClient>();
            discordClientMock.Setup(x => x.GetGuildsAsync(It.IsAny<CacheMode>(), It.IsAny<RequestOptions>())).ReturnsAsync(() => new[] {guild1, guild2});
            
            var serverRepositoryMock = new Mock<IServerRepository>();

            var serverService = new ServerService(discordClientMock.Object, serverRepositoryMock.Object);

            // Act
            var guilds = await serverService.GetAllGuildsAsync();

            // Assert
            Assert.Collection(guilds,
                x => Assert.Equal(guild1, x),
                x => Assert.Equal(guild2, x));
        }

        [Fact]
        public async Task ServerService_IsRegistered_EvaluateCorrectly()
        {
            // Arrange
            // Mock => IDiscordClient
            var discordClientMock = new Mock<IDiscordClient>();
            
            // Mock => IGuild
            var guildMock = new Mock<IGuild>();

            // Mock => IServerRepository
            var serverRepositoryMock = new Mock<IServerRepository>();
            serverRepositoryMock.Setup(x => x.QueryAsync(It.IsAny<Expression<Func<Server, bool>>>()))
                .ReturnsAsync(() => new List<Server>() {new()}.AsQueryable());

            var service = new ServerService(discordClientMock.Object,
                serverRepositoryMock.Object);

            // Act
            var result = await service.IsRegisteredAsync(guildMock.Object);

            // Assert
            Assert.True(result);
        }

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