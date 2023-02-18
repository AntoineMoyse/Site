using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Site.Data;
using Site.Models.ApplicationUser;

[assembly: HostingStartup(typeof(Site.Areas.Identity.IdentityHostingStartup))]
namespace Site.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                /*services.AddDbContext<SiteContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("DbContext")));*/

                services.AddDefaultIdentity<Client>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<SiteContext>();
            });
        }
    }
}