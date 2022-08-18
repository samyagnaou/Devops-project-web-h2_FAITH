using Faith.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Roles = Faith.Shared.Roles;

namespace Faith.Server.Utilities;

public static class DbInitializer
{
    private const string Email = "admin@admin.com";
    private const string Password = "Password11!";

    public static async Task Seed(IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var services = scope.ServiceProvider;
            var dbContext = services.GetRequiredService<FaithDbContext>();
            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
            var rolManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            //var admin = await userManager.FindByEmailAsync("admin@admin.com");

            await dbContext.Database.MigrateAsync();

            var admin = await  userManager.FindByNameAsync(Email);
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