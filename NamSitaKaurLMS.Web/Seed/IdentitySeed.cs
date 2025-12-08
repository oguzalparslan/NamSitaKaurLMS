using Microsoft.AspNetCore.Identity;
using NamSitaKaurLMS.Infrastructure.Identity;

namespace NamSitaKaurLMS.WebUI.Seed
{
    public class IdentitySeed
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

            string[] roles = { "Admin", "Instructor", "Student" };
            // Rolleri oluştur
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Admin kullanıcı
            var adminEmail = "lms@namsitakaur.com";
            var adminUserName = "lmsadmin1";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new AppUser
                {
                    UserName = adminUserName,
                    EmailConfirmed = true
                };
                await userManager.SetUserNameAsync(adminUser, "lmsadmin");
                await userManager.SetEmailAsync(adminUser, adminEmail);

                var result = await userManager.CreateAsync(adminUser, "Ruhi123");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}
