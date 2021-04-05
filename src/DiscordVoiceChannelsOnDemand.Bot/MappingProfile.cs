using AutoMapper;
using DiscordVoiceChannelsOnDemand.Bot.Abstractions;
using DiscordVoiceChannelsOnDemand.Bot.Models;

namespace DiscordVoiceChannelsOnDemand.Bot
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Lobby, LobbySpaceConfiguration>()
                .ForMember(x => x.AutoCreate, 
                    x => 
                        x.MapFrom(z => z.AutoCreateSpace))
                .ReverseMap()
                .ForAllOtherMembers(x => x.Ignore());
        }
    }
}