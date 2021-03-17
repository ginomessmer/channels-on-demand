using System;
using Discord;
using Discord.WebSocket;
using DiscordVoiceChannelsOnDemand.Bot.Infrastructure;
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
        private readonly ITenantRepository _tenantRepository;

        public RoomService(IDiscordClient client, IRoomRepository roomRepository, ITenantRepository tenantRepository)
        {
            _client = client;
            _roomRepository = roomRepository;
            _tenantRepository = tenantRepository;
        }

        /// <inheritdoc />
        public async Task<IVoiceChannel> CreateNewRoomAsync(IGuildUser user)
        {
            var slot = await _tenantRepository.FindSlotAsync(user.VoiceChannel.Id.ToString());
            var guild = await _client.GetGuildAsync(user.Guild.Id);

            var roomVoiceChannel = await guild.CreateVoiceChannelAsync($"Test {user.Nickname}",
                p => { p.CategoryId = Convert.ToUInt64(slot.CategoryId); });

            // Move user
            await user.ModifyAsync(x => x.ChannelId = roomVoiceChannel.Id);

            await _roomRepository.AddAsync(roomVoiceChannel.Id.ToString(), user.Id.ToString(),
                guild.Id.ToString());

            return roomVoiceChannel;
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