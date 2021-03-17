using Discord;
using Discord.WebSocket;
using DiscordVoiceChannelsOnDemand.Bot.Infrastructure;
using DiscordVoiceChannelsOnDemand.Bot.Options;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace DiscordVoiceChannelsOnDemand.Bot.Services
{
    /// <summary>
    /// <inheritdoc />
    /// Based on <seealso cref="DiscordSocketClient"/>.
    /// </summary>
    public class RoomService : IRoomService
    {
        private readonly IDiscordClient _client;
        private readonly IRoomRepository _roomRepository;
        private readonly BotOptions _options;

        public RoomService(IDiscordClient client, IRoomRepository roomRepository,
            IOptions<BotOptions> options)
        {
            _client = client;
            _roomRepository = roomRepository;
            _options = options.Value;
        }

        /// <inheritdoc />
        public async Task<IVoiceChannel> CreateNewRoomAsync(IGuildUser user)
        {
            var guild = await _client.GetGuildAsync(user.Guild.Id);
            var voiceChannel = await guild.CreateVoiceChannelAsync(string.Format(_options.RoomNameFormat, user.Nickname),
                p => { p.CategoryId = _options.CategoryId; });

            // Move user
            await user.ModifyAsync(x => x.ChannelId = voiceChannel.Id);

            await _roomRepository.AddAsync(voiceChannel.Id.ToString(), user.Id.ToString(),
                guild.Id.ToString());

            return voiceChannel;
        }

        /// <inheritdoc />
        public async Task DeleteRoomAsync(IVoiceChannel voiceChannel)
        {
            await voiceChannel.DeleteAsync();
            await _roomRepository.RemoveAsync(voiceChannel.Id.ToString());
        }

        /// <inheritdoc />
        public async Task DeleteRoomAsync(ulong voiceChannelId, ulong guildId)
        {
            var guild = await _client.GetGuildAsync(guildId);
            var channel = await guild.GetVoiceChannelAsync(voiceChannelId);
            await DeleteRoomAsync(channel);
        }
    }
}