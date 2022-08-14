using CommandService.Models;
using Microsoft.EntityFrameworkCore;

namespace CommandService.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Platform> Platforms { get; set; }
        public DbSet<Command> Commands { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Platform>()
                .HasMany(x => x.Commands)
                .WithOne(x => x.Platform!)
                .HasForeignKey(x => x.PlatformId);

            builder.Entity<Command>()
                .HasOne(x => x.Platform)
                .WithMany(x => x.Commands)
                .HasForeignKey(x => x.PlatformId);
        }
    }
}
