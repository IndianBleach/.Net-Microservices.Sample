using AutoMapper;
using Grpc.Core;
using PlatformService.Interfaces;
using PlatformService.Models;

namespace PlatformService.SyncDataServices.Grpc
{
    public class GrpcPlatformService : GrpcPlatform.GrpcPlatformBase
    {
        private readonly IPlatformRepository _platformRepo;
        private readonly IMapper _mapper;

        public GrpcPlatformService(IPlatformRepository platformRepo, IMapper mapper)
        {
            _platformRepo = platformRepo;
            _mapper = mapper;
        }

        public override Task<PlatformResponse> GetAllPlatforms(GetAllRequests request, ServerCallContext context)
        {
            var response = new PlatformResponse();

            var platforms = _platformRepo.GetAllPlatforms();

            foreach (Platform item in platforms)
            {
                response.Platform.Add(_mapper.Map<GrpcPlatformModel>(item));
            }

            return Task.FromResult(response);
        }
    }
}
