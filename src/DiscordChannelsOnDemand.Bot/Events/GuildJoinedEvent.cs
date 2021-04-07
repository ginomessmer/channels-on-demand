using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace DiscordChannelsOnDemand.Bot.Events
{
    public record GuildJoinedEvent(ulong GuildId) : INotification;
    public record GuildLeftEvent(ulong GuildId) : INotification;
}
