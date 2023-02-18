using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Site.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using Site.Services;
using Site.Models.Panier;
using Microsoft.AspNetCore.Http;
using Site.Models;
using Site.Models.ApplicationUser;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Site.PuppeteerSite;

namespace Site
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IProduct, ProductService>();
            services.AddTransient<ICommande, CommandeService>();
            services.AddTransient<ITemplate, RazorViewsTemplateService>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			services.AddScoped(sp => Cart.GetCart(sp));

            services.AddDbContext<SiteContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DbContext")));

            services.AddIdentityCore<Client>().AddEntityFrameworkStores<SiteContext>().AddDefaultUI().AddDefaultTokenProviders().AddSignInManager<SignInManager<Client>>();

            services.AddSession(options => options.IdleTimeout = TimeSpan.FromDays(1));

            services.AddAuthorization(options => 
                {
                    options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                }
            );

            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    IConfigurationSection googleAuthNSection =
                        Configuration.GetSection("Authentication:Google");

                    options.ClientId = "";
                    options.ClientSecret = "";
                });

            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserManager<Client> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseSession();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            ApplicationUserInitializer.SeedUsers(app, userManager, roleManager);
            ModelBuilderExtensions.Seed(app);

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.PreparePuppeteerAsync(env).GetAwaiter().GetResult();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
