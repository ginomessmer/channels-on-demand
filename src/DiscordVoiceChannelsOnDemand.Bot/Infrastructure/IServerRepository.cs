using System.Collections.Generic;
using System.Threading.Tasks;
using DiscordVoiceChannelsOnDemand.Bot.Models;

namespace DiscordVoiceChannelsOnDemand.Bot.Infrastructure
{
    public interface IServerRepository : IRepository<Server>
    {
        Task<IEnumerable<Lobby>> QueryAllLobbiesAsync();

        Task<Lobby> FindLobbyAsync(string voiceChannelId);

        Task DeleteLobbyAsync(string voiceChannelId);
    }
}