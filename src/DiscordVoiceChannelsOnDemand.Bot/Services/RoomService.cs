using Discord;
using Discord.WebSocket;
using DiscordVoiceChannelsOnDemand.Bot.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Discord.Net;
using Microsoft.Extensions.Logging;

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
        private readonly ILogger<RoomService> _logger;

        public RoomService(IDiscordClient client, IRoomRepository roomRepository, IServerRepository serverRepository,
            ILogger<RoomService> logger)
        {
            _client = client;
            _roomRepository = roomRepository;
            _serverRepository = serverRepository;
            _logger = logger;
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
        public Task DeleteRoomAsync(IVoiceChannel voiceChannel) =>
            DeleteRoomAsync(voiceChannel.Id, voiceChannel.Guild.Id);

        /// <inheritdoc />
        public async Task DeleteRoomAsync(ulong voiceChannelId, ulong guildId)
        {
            var guild = await _client.GetGuildAsync(guildId);
            var voiceChannel = await guild.GetVoiceChannelAsync(voiceChannelId);

            if (voiceChannel is not null)
            {
                try
                {
                    await voiceChannel.DeleteAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Couldn't remove voice channel automatically");
                }
            }
            else
                _logger.LogWarning("Couldn't find voice channel {VoiceChannelId}. Removing from database...", voiceChannelId);

            await _roomRepository.RemoveAsync(voiceChannelId.ToString());
        }

        /// <inheritdoc />
        public async Task<IEnumerable<IVoiceChannel>> GetAllRoomVoiceChannelsAsync()
        {
            var results = new List<IVoiceChannel>();
            
            var rooms = await _roomRepository.GetAllAsync();
            var groupedRooms = rooms.GroupBy(x => x.GuildId);
            
            foreach (var groupedRoom in groupedRooms)
            {
                var guildId = Convert.ToUInt64(groupedRoom.Key);
                var guild = await _client.GetGuildAsync(guildId);

                foreach (var room in groupedRoom)
                {
                    var id = Convert.ToUInt64(room.ChannelId);

                    var channel = await guild.GetVoiceChannelAsync(id);
                    if (channel is null)
                    {
                        await DeleteRoomAsync(id, guildId);
                        continue;
                    }

                    results.Add(channel);
                }
            }

            return results;
        }

        /// <inheritdoc />
        public Task<bool> ExistsAsync([NotNull] IVoiceChannel voiceChannel)
        {
            return _roomRepository.ExistsAsync(voiceChannel.Id.ToString());
        }
    }
}