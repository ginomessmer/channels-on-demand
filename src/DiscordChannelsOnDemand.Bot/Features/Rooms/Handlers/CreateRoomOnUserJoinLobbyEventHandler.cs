using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using DiscordChannelsOnDemand.Bot.Events;
using DiscordChannelsOnDemand.Bot.Features.Servers;
using DiscordChannelsOnDemand.Bot.Features.Spaces;
using EnsureThat;
using MediatR;

namespace DiscordChannelsOnDemand.Bot.Features.Rooms.Handlers
{
    public class CreateRoomOnUserJoinLobbyEventHandler : INotificationHandler<UserJoinedVoiceChannelEvent>
    {
        private readonly IDiscordClient _client;
        private readonly IServerService _serverService;
        private readonly IRoomService _roomService;
        private readonly ISpaceService _spaceService;
        private readonly IMediator _mediator;

        public CreateRoomOnUserJoinLobbyEventHandler(IDiscordClient client,
            IServerService serverService, IRoomService roomService,
            ISpaceService spaceService,
            IMediator mediator)
        {
            _client = client;
            _serverService = serverService;
            _roomService = roomService;
            _spaceService = spaceService;
            _mediator = mediator;
        }

        public async Task Handle(UserJoinedVoiceChannelEvent notification, CancellationToken cancellationToken)
        {
            var (userId, voiceChannelId) = notification;

            var channel = await _client.GetChannelAsync(voiceChannelId) as IVoiceChannel;
            Ensure.That(channel).IsNotNull();

            var user = await channel.Guild.GetUserAsync(userId);
            Ensure.That(user).IsNotNull();


            if (!await _serverService.IsLobbyRegisteredAsync(channel))
                return;

            // Create new room
            var lobby = await _serverService.GetLobbyAsync(channel);
            var roomChannel = await _roomService.CreateNewRoomAsync(user, lobby);

            await _mediator.Publish(new RoomCreatedEvent(roomChannel.Id), cancellationToken);
        }
    }
}
