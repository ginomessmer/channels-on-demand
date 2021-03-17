using System.Collections.Generic;
using System.Threading.Tasks;
using DiscordVoiceChannelsOnDemand.Bot.Models;

namespace DiscordVoiceChannelsOnDemand.Bot.Infrastructure
{
    public interface IServerRepository : IGenericRepository<Server>
    {
        Task<IEnumerable<Lobby>> QueryAllLobbysAsync();

        Task<bool> LobbysExistsAsync(string voiceChannelId);
        
        Task<Lobby> FindLobbyAsync(string voiceChannelId);

        Task DeleteLobbyAsync(string voiceChannelId);
    }
}