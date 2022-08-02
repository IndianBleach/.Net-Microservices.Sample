using PlatformService.Models;

namespace PlatformService.Data
{
    public static class PrepareDbContext
    {
        public static void PrepareDb(IApplicationBuilder app)
        {
            using (IServiceScope scope = app.ApplicationServices.CreateScope())
            {
                SeedData(scope.ServiceProvider.GetRequiredService<AppDbContext>());
            }
        }

        private static void SeedData(AppDbContext ctx)
        {
            if (!ctx.Platforms.Any())
            {
                Console.WriteLine("--> Seeding data");

                ctx.Platforms.AddRange(
                    new Platform() { Cost = "Free", Name = "SqlServer", Publisher = "Microsoft" },
                    new Platform() { Cost = "Free", Name = "Azure", Publisher = "Microsoft" },
                    new Platform() { Cost = "Free", Name = "Kubernetes", Publisher = "CloudSys" });

                ctx.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> Already have data");
            }
        }
    }
}
