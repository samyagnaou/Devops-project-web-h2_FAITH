using Faith.Shared;
using Microsoft.AspNetCore.Identity;

namespace Faith.Server.Utilities;

public static class DbInitializer
{
    private const string Email = "admin@admin.com";
    private const string Password = "Password11!";

    public static async Task SeedAdminUser(IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var services = scope.ServiceProvider;
            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
            var admin = await userManager.FindByEmailAsync("admin@admin.com");
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