using Clinic.Infrastructure.UserModels;
using Microsoft.AspNetCore.Identity;

namespace Clinic.UI.Services
{
    public static class DbSeeder
    {
        public static async Task SeedAdminAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, string password)
        {
            // 1. إنشاء الدور Admin لو مش موجود
            var adminRoleExists = await roleManager.RoleExistsAsync("Admin");
            if (!adminRoleExists)
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            var doctorRoleExists = await roleManager.RoleExistsAsync("Doctor");
            if (!doctorRoleExists)
            {
                await roleManager.CreateAsync(new IdentityRole("Doctor"));
            }

            var reciptionistRoleExists = await roleManager.RoleExistsAsync("Reciptionist");
            if (!reciptionistRoleExists)
            {
                await roleManager.CreateAsync(new IdentityRole("Reciptionist"));
            }

            var developerRoleExists = await roleManager.RoleExistsAsync("Developer");
            if (!developerRoleExists)
            {
                await roleManager.CreateAsync(new IdentityRole("Developer"));
            }

            // 2. إنشاء حساب Admin لو مش موجود
            var adminUser = await userManager.FindByNameAsync("admin");
            if (adminUser == null)
            {
                var user = new AppUser
                {
                    UserName = "admin",
                    FirstName = "Admin",
                    LastName = "User",
                    Email = "admin@dreamcenter.com",
                    PhoneNumber = "01000000000",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, password); // ضع كلمة مرور قوية

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }

            var devUser = await userManager.FindByNameAsync("developer");
            if (devUser == null)
            {
                var user = new AppUser
                {
                    UserName = "developer",
                    FirstName = "Developer",
                    LastName = "User",
                    Email = "developer@dreamcenter.com",
                    PhoneNumber = "01111111111",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Developer");
                }
            }
        }
    }
}
