using AutoMapper;
using AutoMapper.Configuration.Annotations;
using DiscordVoiceChannelsOnDemand.Bot.Models;

namespace DiscordVoiceChannelsOnDemand.Bot.Abstractions
{
    [AutoMap(typeof(Lobby), ReverseMap = true)]
    public class LobbySpaceConfiguration
    {
        [SourceMember(nameof(Lobby.AutoCreateSpace))]
        public bool AutoCreate { get; set; }
    }
}