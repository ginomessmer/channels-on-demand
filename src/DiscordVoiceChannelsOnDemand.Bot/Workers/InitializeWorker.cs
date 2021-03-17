using DiscordVoiceChannelsOnDemand.Bot.Services;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using Discord.WebSocket;
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

    public class InitializeWorker : BackgroundService
    {
        private readonly IServerService _serverService;

        public InitializeWorker(IServerService serverService)
        {
            _serverService = serverService;
        }

        /// <inheritdoc />
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var guilds = await _serverService.GetAllGuildsAsync();
            foreach (var guild in guilds)
            {
                if (await _serverService.IsRegisteredAsync(guild))
                    continue;

                await _serverService.RegisterAsync(guild);
            }
        }
    }
}