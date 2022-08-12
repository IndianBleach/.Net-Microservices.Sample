using CommandService.Data;
using CommandService.Interfaces;
using CommandService.Models;
using Microsoft.EntityFrameworkCore;

namespace CommandService.Repositories
{
    public class CommandRepository : ICommandRepository
    {
        private readonly AppDbContext _context;

        public CommandRepository(AppDbContext context)
        {
            _context = context;
        }

        public void CreateCommand(int platformId, Command command)
        {
            if (command == null)
                throw new ArgumentNullException();

            command.PlatformId = platformId;

            _context.Commands.Add(command);
        }

        public void CreatePlatform(Platform platform)
        {
            if (platform == null)
                throw new ArgumentNullException();

            _context.Platforms.Add(platform);
        }

        public IEnumerable<Platform> GetAllPlatforms()
            => _context.Platforms.ToList();

        public Command GetCommand(int platformId, int commandId)
            => _context.Commands
                .FirstOrDefault(x => x.Id == commandId && x.PlatformId == platformId);

        public IEnumerable<Command> GetCommandsForPlatform(int platformId)
            => _context.Commands
                .Include(x => x.Platform)
                .Where(x => x.PlatformId == platformId)
                .OrderBy(x => x.Platform.Name);

        public bool PlatformExists(int platformId)
            => _context.Platforms.Any(x => x.Id == platformId);

        public bool SaveChanges()
            => (_context.SaveChanges() >= 0);
    }
}
