using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using DiscordVoiceChannelsOnDemand.Bot.Infrastructure;
using DiscordVoiceChannelsOnDemand.Bot.Models;
using DiscordVoiceChannelsOnDemand.Bot.Options;
using Microsoft.Extensions.Options;

namespace DiscordVoiceChannelsOnDemand.Bot.Services
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

        /// <inheritdoc />
        public Task DeleteRoomAsync(ulong voiceChannelId, ulong guildId)
        {
            var channel = _client.GetGuild(guildId).GetVoiceChannel(voiceChannelId);
            return DeleteRoomAsync(channel);
        }
    }
}