using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using DiscordVoiceChannelButler.Bot.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DiscordVoiceChannelButler.Bot.Workers
{
    public class Worker : BackgroundService
    {
        private readonly DiscordSocketClient _client;
        private readonly BotState _botState;
        private readonly BotOptions _options;
        private readonly ILogger<Worker> _logger;

        public Worker(DiscordSocketClient client, BotState botState, IOptions<BotOptions> options, ILogger<Worker> logger)
        {
            _client = client;
            _botState = botState;
            _options = options.Value;
            _logger = logger;

            _client.UserVoiceStateUpdated += ClientOnUserVoiceStateUpdated;
        }

        private async Task ClientOnUserVoiceStateUpdated(SocketUser arg1, SocketVoiceState previousState, SocketVoiceState newState)
        {
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

            _botState.AddRoom(voiceChannel.Id, user.Id);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _client.SetActivityAsync(new Game("Hello world"));
        }
    }
}
