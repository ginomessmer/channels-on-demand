using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using DiscordChannelsOnDemand.Bot.Events;
using MediatR;

namespace DiscordChannelsOnDemand.Bot.Features.Servers.Handlers
{
    public class ServerRegistrationEventHandler : INotificationHandler<GuildJoinedEvent>, INotificationHandler<GuildLeftEvent>
    {
        private readonly IDiscordClient _client;
        private readonly IServerService _serverService;

        public ServerRegistrationEventHandler(IDiscordClient client, IServerService serverService)
        {
            _client = client;
            _serverService = serverService;
        }

        public async Task Handle(GuildJoinedEvent notification, CancellationToken cancellationToken)
        {
            var guild = await _client.GetGuildAsync(notification.GuildId);
            await _serverService.RegisterAsync(guild);
        }

        public async Task Handle(GuildLeftEvent notification, CancellationToken cancellationToken)
        {
            var guild = await _client.GetGuildAsync(notification.GuildId);
            await _serverService.DeregisterAsync(guild);
        }
    }
}
