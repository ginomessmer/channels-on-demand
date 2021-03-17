using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using DiscordVoiceChannelsOnDemand.Bot.Infrastructure;
using DiscordVoiceChannelsOnDemand.Bot.Services;
using Microsoft.Extensions.Hosting;

namespace DiscordVoiceChannelsOnDemand.Bot.Workers
{
    /// <summary>
    /// Restores the bot in case of shutdown and purges dangling channels and cleans the database.
    /// </summary>
    public class RestoreWorker : BackgroundService
    {
        private readonly IRoomService _roomService;
        private readonly IVoiceChannelService _voiceChannelService;

        public RestoreWorker(IRoomService roomService, IVoiceChannelService voiceChannelService)
        {
            _roomService = roomService;
            _voiceChannelService = voiceChannelService;
        }

        /// <inheritdoc />
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Purge dangling channels
            var allChannels = await _voiceChannelService.GetAllAsync();
        }
    }

    public interface IVoiceChannelService
    {
        Task<IEnumerable<IVoiceChannel>> GetAllAsync();

        Task<IEnumerable<IVoiceChannel>> GetAllAsync(ulong guildId);
    }
    
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

    /// <summary>
    /// This worker removes empty voice channels created by the butler.
    /// </summary>
    public class CleanUpWorker : BackgroundService
    {
        private readonly DiscordSocketClient _client;
        private readonly IRoomService _roomService;
        private readonly IRoomRepository _roomRepository;

        public CleanUpWorker(DiscordSocketClient client, IRoomService roomService, IRoomRepository roomRepository)
        {
            _client = client;
            _roomService = roomService;
            _roomRepository = roomRepository;
        }

        /// <inheritdoc />
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _client.UserVoiceStateUpdated += ClientOnUserVoiceStateUpdated;
            return Task.CompletedTask;
        }

        private async Task ClientOnUserVoiceStateUpdated(SocketUser arg1, SocketVoiceState previousState, SocketVoiceState newState)
        {
            if (previousState.VoiceChannel is null)
                return;

            // Check if its part of Rooms
            if (!await _roomRepository.ExistsAsync(previousState.VoiceChannel.Id.ToString()))
                return;

            // Handle disconnect
            if (previousState.VoiceChannel.Users.Count > 0)
                return;

            await _roomService.DeleteRoomAsync(previousState.VoiceChannel);
        }
    }
}