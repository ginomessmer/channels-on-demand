using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

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
        public Task<IEnumerable<IVoiceChannel>> GetAllAsync()
        {
            return Task.FromResult(_client.Guilds.SelectMany(x => x.VoiceChannels.Cast<IVoiceChannel>()));
        }

        /// <inheritdoc />
        public Task<IEnumerable<IVoiceChannel>> GetAllAsync(ulong guildId)
        {
            return Task.FromResult(_client.GetGuild(guildId).VoiceChannels.Cast<IVoiceChannel>());
        }
    }
}