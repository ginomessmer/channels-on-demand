using System.Threading;
using System.Threading.Tasks;
using Discord.WebSocket;
using DiscordVoiceChannelsOnDemand.Bot.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DiscordVoiceChannelsOnDemand.Bot.Workers
{
    public class ServerRegistrationWorker : BackgroundService
    {
        private readonly DiscordSocketClient _client;
        private readonly IServerService _serverService;
        private readonly ILogger<ServerRegistrationWorker> _logger;

        public ServerRegistrationWorker(DiscordSocketClient client,
            IServerService serverService,
            ILogger<ServerRegistrationWorker> logger)
        {
            _client = client;
            _serverService = serverService;
            _logger = logger;
        }

        /// <inheritdoc />
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _client.JoinedGuild += ClientOnJoinedGuild;
            _client.LeftGuild += ClientOnLeftGuild;
            return Task.CompletedTask;
        }

        private async Task ClientOnJoinedGuild(SocketGuild guild)
        {
            await _serverService.RegisterAsync(guild);
            _logger.LogInformation("Joined new server {Guild}", guild);
        }

        private async Task ClientOnLeftGuild(SocketGuild guild)
        {
            await _serverService.DeregisterAsync(guild);
            _logger.LogInformation("Left server {Guild}", guild);
        }
    }
}