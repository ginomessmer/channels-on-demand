using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using DiscordChannelsOnDemand.Bot.Events;
using DiscordChannelsOnDemand.Bot.Features.Rooms;
using DiscordChannelsOnDemand.Bot.Features.Servers;
using MediatR;

namespace DiscordChannelsOnDemand.Bot.Features.Spaces.Handlers
{
    public class RoomAutoCreateSpaceEventHandler : INotificationHandler<RoomCreatedEvent>
    {
        private readonly IDiscordClient _client;
        private readonly IServerService _serverService;
        private readonly IRoomService _roomService;

        public RoomAutoCreateSpaceEventHandler(IDiscordClient client,
            IServerService serverService, IRoomService roomService)
        {
            _client = client;
            _serverService = serverService;
            _roomService = roomService;
        }

        [Obsolete]
        public async Task Handle(RoomCreatedEvent notification, CancellationToken cancellationToken)
        {
            return;
            var voiceChannel = await _client.GetChannelAsync(notification.roomVoiceChannelId) as IVoiceChannel;
            var room = await _roomService.GetRoomAsync(voiceChannel);
            var lobby = await _serverService.GetLobbyAsync(voiceChannel);

            // TODO!
            //if (lobby.AutoCreateSpace)
            //{
            //    var space = await unitOfWork.SpaceService.CreateSpaceAsync(user);
            //    await unitOfWork.RoomService.LinkSpaceAsync(lobby.TriggerVoiceChannelId, space.TextChannelId);
            //}
        }
    }
}
