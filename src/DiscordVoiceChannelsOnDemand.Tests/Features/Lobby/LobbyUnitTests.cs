using System.Collections.Generic;
using Moq;
using Xunit;

namespace DiscordVoiceChannelsOnDemand.Tests.Features.Lobby
{
    public class LobbyUnitTests
    {
        [Fact]
        public void Lobby_SuggestName_Single()
        {
            // Arrange
            var lobby = new Bot.Models.Lobby(It.IsAny<string>(), It.IsAny<string>());
            const string name = "Martha";

            // Act
            lobby.RoomNames = new List<string> { name };
            var suggestedName = lobby.SuggestRoomName();

            // Assert
            Assert.Equal(name, suggestedName);
        }

        [Fact]
        public void Lobby_SuggestName_ContainsFromList()
        {
            // Arrange
            var lobby = new Bot.Models.Lobby(It.IsAny<string>(), It.IsAny<string>());
            var names = new[] { "Martha", "Gino" };

            // Act
            lobby.RoomNames = names;
            var name = lobby.SuggestRoomName();

            // Assert
            Assert.Contains(name, names);
        }

        [Fact]
        public void Lobby_SetNames_Correctly()
        {
            // Arrange
            var lobby = new Bot.Models.Lobby();
            var names = new[] {"Martha", "Gino"};

            // Act
            lobby.RoomNames = names;

            // Assert
            Assert.Equal(2, lobby.RoomNames.Count);
            Assert.Collection(lobby.RoomNames,
                x => Assert.Equal("Martha", x),
                x => Assert.Equal("Gino", x));
        }

        [Theory]
        [InlineData("1234", true)]
        [InlineData("", false)]
        [InlineData(null, false)]
        public void Lobby_Category_Truthful(string category, bool result)
        {
            // Arrange
            var lobby = new Bot.Models.Lobby();

            // Act
            lobby.CategoryId = category;

            // Assert
            Assert.Equal(result, lobby.HasCategory);
            Assert.Equal(category, lobby.CategoryId);
        } 
    }
}