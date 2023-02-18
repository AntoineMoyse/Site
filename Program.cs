using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

//dotnet dev-certs https --clean/trust
//Server=(localdb)\\mssqllocaldb;Database=DBSite;Trusted_Connection=True;MultipleActiveResultSets=true

namespace Site
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
