using CommandService.Models;

namespace CommandService.Interfaces
{
    public interface ICommandRepository
    {
        //platforms
        void CreatePlatform(Platform platform);
        IEnumerable<Platform> GetAllPlatforms();
        bool PlatformExists(int platformId);

        //commands
        IEnumerable<Command> GetCommandsForPlatform(int platformId);
        void CreateCommand(int platformId, Command command);
        Command GetCommand(int platformId, int commandId);

        bool SaveChanges();
    }
}
