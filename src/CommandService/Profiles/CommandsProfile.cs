using AutoMapper;
using CommandService.DTOs;
using CommandService.Models;
using PlatformService;
//using PlatformService;

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

            CreateMap<GrpcPlatformModel, Platform>()
                .ForMember(x => x.ExternalId, opt => opt.MapFrom(x => x.PlatformId))
                .ForMember(x => x.Name, opt => opt.MapFrom(x => x.Name))
                .ForMember(x => x.Commands, opt => opt.Ignore())
                .ForMember(x => x.Publisher, opt => opt.MapFrom(x => x.Publisher));
        }
    }
}
