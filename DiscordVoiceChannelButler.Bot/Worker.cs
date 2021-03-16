using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Options;

namespace DiscordVoiceChannelButler.Bot
{
    public class Worker : BackgroundService
    {
        private readonly DiscordSocketClient _client;
        private readonly BotOptions _options;
        private readonly ILogger<Worker> _logger;

        public IList<Room> Rooms { get; set; } = new List<Room>();

        public Worker(DiscordSocketClient client, IOptions<BotOptions> options, ILogger<Worker> logger)
        {
            _client = client;
            _options = options.Value;
            _logger = logger;

            _client.UserVoiceStateUpdated += ClientOnUserVoiceStateUpdated;
        }

        private async Task ClientOnUserVoiceStateUpdated(SocketUser arg1, SocketVoiceState previousState, SocketVoiceState newState)
        {
            // Check rooms


            if (newState.VoiceChannel is null)
            {
                // Check if its part of Rooms
                if (!Rooms.ToList().Exists(x => x.Channel.Id == previousState.VoiceChannel.Id))
                    return;

                // Handle disconnect
                if (previousState.VoiceChannel.Users.Count > 0)
                    return;

                await ScheduleRemovalAsync(previousState.VoiceChannel);

                return;
            }

            // Check if user is typeof(SocketGuildUser)
            if (arg1 is not SocketGuildUser user)
                return;

            // Check voice channel
            if (user.VoiceChannel?.Id != _options.GatewayVoiceChannelId)
                return;

            // Create new voice channel
            var guild = _client.GetGuild(user.Guild.Id);
            var voiceChannel = await guild.CreateVoiceChannelAsync(_options.RoomName + $" {user.Nickname}", p =>
            {
                p.CategoryId = _options.CategoryId;
            });

            await user.ModifyAsync(x => x.Channel = voiceChannel);

            Rooms.Add(new Room
            {
                Channel = voiceChannel,
                Host = user
            });
        }

        private async Task ScheduleRemovalAsync(SocketVoiceChannel voiceChannel)
        {
            await voiceChannel.DeleteAsync();
            Rooms.Remove(Rooms.First(x => x.Channel.Id == voiceChannel.Id));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _client.SetActivityAsync(new Game("Hello world"));
        }
    }
}
