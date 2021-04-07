using AutoMapper;
using AutoMapper.Configuration.Annotations;
using DiscordChannelsOnDemand.Bot.Models;

namespace DiscordChannelsOnDemand.Bot.Core.Abstractions
{
    [AutoMap(typeof(Lobby), ReverseMap = true)]
    public class LobbySpaceConfiguration
    {
        [SourceMember(nameof(Lobby.AutoCreateSpace))]
        public bool AutoCreate { get; set; }
    }
}