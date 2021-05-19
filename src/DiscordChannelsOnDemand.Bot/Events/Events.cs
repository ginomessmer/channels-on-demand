using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace DiscordChannelsOnDemand.Bot.Events
{
    // COD
    public record RoomCreatedEvent(ulong roomVoiceChannelId) : INotification;

    // Discord
    public record GuildJoinedEvent(ulong GuildId) : INotification;
    public record GuildLeftEvent(ulong GuildId) : INotification;

    public record UserJoinedVoiceChannelEvent(ulong UserId, ulong VoiceChannelId) : INotification;
    public record UserLeftVoiceChannelEvent(ulong UserId, ulong VoiceChannelId) : INotification;
}
