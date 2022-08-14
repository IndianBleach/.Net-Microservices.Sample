using AutoMapper;
using CommandService.Models;
using Grpc.Net.Client;
using PlatformService;

namespace CommandService.SyncDataServices.Grpc
{
    public class PlatformDataClient : IPlatformDataClient
    {
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public PlatformDataClient(IConfiguration config, IMapper mapper)
        {
            _config = config;
            _mapper = mapper;
        }

        public IEnumerable<Platform> ReturnAllPlatforms()
        {
            Console.WriteLine($"Calling gRPC service: {_config["GrpcPlatform"]}");

            var channel = GrpcChannel.ForAddress(_config["GrpcPlatform"]);
            
            var client = new GrpcPlatform.GrpcPlatformClient(channel);

            var request = new GetAllRequests();

            try
            {
                var reply = client.GetAllPlatforms(request);

                return _mapper.Map<IEnumerable<Platform>>(reply.Platform);

            }
            catch (Exception exp)
            {
                Console.WriteLine("Could't call gRPC: " + exp.Message);

                return null;
            }
        }
    }
}
