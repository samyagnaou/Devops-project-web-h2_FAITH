using Faith.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Faith.Server.Utilities
{
    public static class DbInitializer
    {
        private const string Email = "admin@admin.com";
        private const string Password = "Password11!";
        private static string[] RolesArr
            = new string[] { Roles.Admin, Roles.Mentor, Roles.Student };

        public static async Task Seed(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;
                var dbContext = services.GetRequiredService<FaithPlatformContext>();
                var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                await dbContext.Database.MigrateAsync();

                foreach (var role in RolesArr)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                        await roleManager.CreateAsync(new IdentityRole { Name = role });
                }

                var admin = await userManager.FindByEmailAsync(Email);
                if (admin == null)
                {
                    admin = new IdentityUser { UserName = Email, Email = Email };
                    await userManager.CreateAsync(admin, Password);
                }
                if (!await userManager.IsInRoleAsync(admin, Roles.Admin))
                    await userManager.AddToRoleAsync(admin, Roles.Admin);
            }
        }
    }
}