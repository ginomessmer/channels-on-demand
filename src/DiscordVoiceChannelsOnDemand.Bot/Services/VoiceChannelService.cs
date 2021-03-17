using Discord;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordVoiceChannelsOnDemand.Bot.Services
{
    public class VoiceChannelService : IVoiceChannelService
    {
        private readonly IDiscordClient _client;

        public VoiceChannelService(IDiscordClient client)
        {
            _client = client;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<IVoiceChannel>> GetAllAsync()
        {
            var guilds = await _client.GetGuildsAsync();
            var guildChannels = await Task.WhenAll(guilds.Select(x => x.GetVoiceChannelsAsync()));
            var allChannels = guildChannels.SelectMany(x => x);

            return allChannels;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<IVoiceChannel>> GetAllAsync(ulong guildId)
        {
            var guild = await _client.GetGuildAsync(guildId);
            return await guild.GetVoiceChannelsAsync();
        }

        public async Task<bool> CanManageChannelAsync(ulong voiceChannelId, ulong guildId)
        {
            var guild = await _client.GetGuildAsync(guildId);
            var channel = await guild.GetVoiceChannelAsync(voiceChannelId);
            var permissions = channel.GetPermissionOverwrite(_client.CurrentUser);

            return permissions.Value.ManageChannel is PermValue.Allow;
        }
    }
}