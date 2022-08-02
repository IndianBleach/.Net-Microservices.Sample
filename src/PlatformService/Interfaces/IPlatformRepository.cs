using PlatformService.Models;

namespace PlatformService.Interfaces
{
    public interface IPlatformRepository
    {
        bool SaveChanges();

        IEnumerable<Platform> GetAllPlatforms();

        void CreatePlatform(Platform platform);

        Platform GetPlatformById(int id);
    }
}
