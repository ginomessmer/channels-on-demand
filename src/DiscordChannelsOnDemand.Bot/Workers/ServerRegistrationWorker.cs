using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using DiscordChannelsOnDemand.Bot.Core.Services;

namespace DiscordChannelsOnDemand.Bot.Workers
{
    public class ServerRegistrationWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly DiscordSocketClient _client;
        private readonly ILogger<ServerRegistrationWorker> _logger;
        private IServerService _serverService;

        public ServerRegistrationWorker(IServiceProvider serviceProvider,
            DiscordSocketClient client,
            ILogger<ServerRegistrationWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _client = client;
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
            using var scope = _serviceProvider.CreateScope();
            _serverService = scope.ServiceProvider.GetRequiredService<IServerService>();

            await _serverService.RegisterAsync(guild);
            _logger.LogInformation("Joined new server {Guild}", guild);
        }

        private async Task ClientOnLeftGuild(SocketGuild guild)
        {
            using var scope = _serviceProvider.CreateScope();
            _serverService = scope.ServiceProvider.GetRequiredService<IServerService>();

            await _serverService.DeregisterAsync(guild);
            _logger.LogInformation("Left server {Guild}", guild);
        }
    }
}