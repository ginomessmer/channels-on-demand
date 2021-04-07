using System.Threading;
using System.Threading.Tasks;
using Discord.WebSocket;
using DiscordChannelsOnDemand.Bot.Events;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DiscordChannelsOnDemand.Bot.Features.Servers
{
    public class ServerRegistrationWorker : BackgroundService
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ServerRegistrationWorker> _logger;
        private readonly DiscordSocketClient _client;

        public ServerRegistrationWorker(DiscordSocketClient client,
            IMediator mediator,
            ILogger<ServerRegistrationWorker> logger)
        {
            _client = client;
            _mediator = mediator;
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
            await _mediator.Send(new GuildJoinedEvent(guild.Id));
            _logger.LogInformation("Joined new server {Guild}", guild);
        }

        private async Task ClientOnLeftGuild(SocketGuild guild)
        {
            await _mediator.Send(new GuildLeftEvent(guild.Id));
            _logger.LogInformation("Left server {Guild}", guild);
        }
    }
}