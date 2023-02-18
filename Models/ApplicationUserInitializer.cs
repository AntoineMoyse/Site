using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Site.Data;
using Site.Models.ApplicationUser;

namespace Site.Models
{
    public static class ApplicationUserInitializer
    {
        public static void SeedUsers(IApplicationBuilder applicationBuilder,UserManager<Client> userManager, RoleManager<IdentityRole> roleManager)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                SiteContext context = serviceScope.ServiceProvider.GetService<SiteContext>();

                if (userManager.FindByEmailAsync("admin@admin.com").Result == null)
                {
                    if (roleManager.FindByNameAsync("Admin").Result == null)
                    {
                        var role = new IdentityRole
                        {
                            Name = "Admin"
                        };
                        var bonjour = roleManager.CreateAsync(role).Result;
                    }
                    Client user = new Client
                    {
                        UserName = "admin@admin.com",
                        Email = "admin@admin.com",
                        EmailConfirmed = true
                    };
                    string userpswrd = "Motdepasse!123";
                    var bonjoir = userManager.CreateAsync(user, userpswrd).Result;
                    var enculerdefilsdeputedecontextdesesmortsquestpascapabledefairedeuxtrucsenmemetemps = userManager.AddToRoleAsync(user, "Admin").Result;
                }
            }
        }
    }
}