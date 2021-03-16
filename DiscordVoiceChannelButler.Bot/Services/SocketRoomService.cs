using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using DiscordVoiceChannelButler.Bot.Options;
using Microsoft.Extensions.Options;

namespace DiscordVoiceChannelButler.Bot.Services
{
    public class SocketRoomService : IRoomService
    {
        private readonly DiscordSocketClient _client;
        private readonly BotState _botState;
        private readonly BotOptions _options;

        public SocketRoomService(DiscordSocketClient client, BotState botState,
            IOptions<BotOptions> options)
        {
            _client = client;
            _botState = botState;
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

            _botState.AddRoom(voiceChannel.Id, user.Id);

            return voiceChannel;
        }

        /// <inheritdoc />
        public async Task DeleteRoomAsync(SocketVoiceChannel voiceChannel)
        {
            await voiceChannel.DeleteAsync();
            _botState.RemoveRoom(voiceChannel.Id);
        }
    }
}