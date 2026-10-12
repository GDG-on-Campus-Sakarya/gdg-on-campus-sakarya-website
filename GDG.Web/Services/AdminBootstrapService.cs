using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GDG.Web.Services
{
    public static class AdminBootstrapService
    {
        public static async Task EnsureAdminBootstrapAsync(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            var adminEmail = configuration["AdminBootstrap:Email"];
            var adminPassword = configuration["AdminBootstrap:Password"];

            // No bootstrap secrets means the operator may provision accounts another way.
            if (string.IsNullOrWhiteSpace(adminEmail) && string.IsNullOrWhiteSpace(adminPassword))
                return;

            if (string.IsNullOrWhiteSpace(adminEmail) || string.IsNullOrWhiteSpace(adminPassword))
                throw new InvalidOperationException("AdminBootstrap:Email ve AdminBootstrap:Password birlikte yapılandırılmalıdır.");

            adminEmail = adminEmail.Trim();
            using var scope = serviceProvider.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("AdminBootstrap");

            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                var roleResult = await roleManager.CreateAsync(new IdentityRole("Admin"));
                if (!roleResult.Succeeded && !await roleManager.RoleExistsAsync("Admin"))
                    throw BuildIdentityFailure("Admin rolü oluşturulamadı", roleResult);
            }

            var user = await userManager.FindByEmailAsync(adminEmail);
            var userCreated = false;
            if (user is null)
            {
                user = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var createResult = await userManager.CreateAsync(user, adminPassword);
                if (!createResult.Succeeded)
                    throw BuildIdentityFailure("İlk admin kullanıcısı oluşturulamadı", createResult);

                userCreated = true;
            }

            var alreadyAdmin = await userManager.IsInRoleAsync(user, "Admin");
            if (!userCreated && !alreadyAdmin && !await userManager.CheckPasswordAsync(user, adminPassword))
            {
                // Never elevate an existing non-admin account solely because its email was
                // supplied in bootstrap configuration; prove control of that account first.
                throw new InvalidOperationException(
                    "Mevcut kullanıcı Admin rolüne yükseltilemedi. Bootstrap parolasının mevcut hesapla eşleştiğini doğrulayın.");
            }

            if (!alreadyAdmin)
            {
                var roleResult = await userManager.AddToRoleAsync(user, "Admin");
                if (!roleResult.Succeeded)
                    throw BuildIdentityFailure("Admin rolü kullanıcıya atanamadı", roleResult);
            }

            if (userCreated)
                logger.LogInformation("İlk admin kullanıcısı güvenli yapılandırmayla oluşturuldu.");
            else
                logger.LogInformation("Yapılandırılmış admin hesabının rol durumu doğrulandı.");
        }

        private static InvalidOperationException BuildIdentityFailure(string action, IdentityResult result)
        {
            var errorCodes = string.Join(", ", result.Errors.Select(error => error.Code));
            return new InvalidOperationException($"{action}. Identity hata kodları: {errorCodes}");
        }
    }
}
