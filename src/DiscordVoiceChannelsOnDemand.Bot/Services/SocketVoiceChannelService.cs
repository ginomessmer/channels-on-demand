using Discord;
using Discord.WebSocket;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordVoiceChannelsOnDemand.Bot.Services
{
    public class SocketVoiceChannelService : IVoiceChannelService
    {
        private readonly DiscordSocketClient _client;

        public SocketVoiceChannelService(DiscordSocketClient client)
        {
            _client = client;
        }

        /// <inheritdoc />
        public Task<IEnumerable<IVoiceChannel>> GetAllAsync() => 
            Task.FromResult(_client.Guilds.SelectMany(x => x.VoiceChannels.Cast<IVoiceChannel>()));

        /// <inheritdoc />
        public Task<IEnumerable<IVoiceChannel>> GetAllAsync(ulong guildId) => 
            Task.FromResult(_client.GetGuild(guildId).VoiceChannels.Cast<IVoiceChannel>());
    }
}