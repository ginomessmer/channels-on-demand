using System;
using System.Collections.Generic;
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
        private readonly IServerRepository _serverRepository;

        public RoomService(IDiscordClient client, IRoomRepository roomRepository, IServerRepository serverRepository)
        {
            _client = client;
            _roomRepository = roomRepository;
            _serverRepository = serverRepository;
        }

        /// <inheritdoc />
        public async Task<IVoiceChannel> CreateNewRoomAsync(IGuildUser user)
        {
            var lobby = await _serverRepository.FindLobbyAsync(user.VoiceChannel.Id.ToString());
            var guild = await _client.GetGuildAsync(user.Guild.Id);

            var name = string.Format(lobby.GetSuggestedName(), user.Nickname);
            var roomVoiceChannel = await guild.CreateVoiceChannelAsync(name,
                p =>
                {
                    p.CategoryId = Convert.ToUInt64(lobby.CategoryId);
                    p.PermissionOverwrites = new Optional<IEnumerable<Overwrite>>(new[]
                    {
                        new Overwrite(user.Id, PermissionTarget.User,
                            new OverwritePermissions(manageChannel: PermValue.Allow))
                    });
                });

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