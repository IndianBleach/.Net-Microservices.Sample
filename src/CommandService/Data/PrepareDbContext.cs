using CommandService.Interfaces;
using CommandService.Models;
using CommandService.SyncDataServices.Grpc;

namespace CommandService.Data
{
    public static class PrepareDbContext
    {
        public static void PrepareDb(IApplicationBuilder builder)
        {
            using (var scope = builder.ApplicationServices.CreateScope())
            {
                var grpcClient = scope.ServiceProvider.GetRequiredService<IPlatformDataClient>();

                IEnumerable<Platform> platforms = grpcClient.ReturnAllPlatforms();

                SeedData(scope.ServiceProvider.GetRequiredService<ICommandRepository>(), platforms);
            };
        }

        private static void SeedData(ICommandRepository commandRepo, IEnumerable<Platform> platforms)
        {
            Console.WriteLine("Seeding data with gRPC");

            foreach (Platform item in platforms)
            {
                if (!commandRepo.ExternalPlatformExist(item.ExternalId))
                    commandRepo.CreatePlatform(item);
                
                commandRepo.SaveChanges();
            }            
        }
    }
}
