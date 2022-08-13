using AutoMapper;
using CommandService.DTOs;
using CommandService.Models;

namespace CommandService.Profiles
{
    public class CommandsProfile : Profile
    {
        public CommandsProfile()
        {
            // src -> target
            CreateMap<Platform, PlatformReadDto>();

            CreateMap<Command, CommandReadDto>();

            CreateMap<CreateCommandDto, Command>();

            CreateMap<PlatformPublishedDto, Platform>()
                .ForMember(x => x.ExternalId, opt => opt.MapFrom(x => x.Id));
        }
    }
}
