using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using DiscordVoiceChannelButler.Bot.Infrastructure;
using DiscordVoiceChannelButler.Bot.Options;
using Microsoft.Extensions.Options;

namespace DiscordVoiceChannelButler.Bot.Services
{
    /// <summary>
    /// <inheritdoc />
    /// Based on <seealso cref="DiscordSocketClient"/>.
    /// </summary>
    public class SocketRoomService : IRoomService
    {
        private readonly DiscordSocketClient _client;
        private readonly IRoomRepository _roomRepository;
        private readonly BotOptions _options;

        public SocketRoomService(DiscordSocketClient client, IRoomRepository roomRepository,
            IOptions<BotOptions> options)
        {
            _client = client;
            _roomRepository = roomRepository;
            _options = options.Value;
        }

        /// <inheritdoc />
        public async Task<IVoiceChannel> CreateNewRoomAsync(SocketGuildUser user)
        {
            var guild = _client.GetGuild(user.Guild.Id);
            var voiceChannel = await guild.CreateVoiceChannelAsync(string.Format(_options.RoomNameFormat, user.Nickname),
                p => { p.CategoryId = _options.CategoryId; });

            // Move user
            await user.ModifyAsync(x => x.Channel = voiceChannel);

            await _roomRepository.AddAsync(voiceChannel.Id.ToString(), user.Id.ToString());

            return voiceChannel;
        }

        /// <inheritdoc />
        public async Task DeleteRoomAsync(SocketVoiceChannel voiceChannel)
        {
            await voiceChannel.DeleteAsync();
            await _roomRepository.RemoveAsync(voiceChannel.Id.ToString());
        }
    }
}