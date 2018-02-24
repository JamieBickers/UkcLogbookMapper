using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
//using LocationsMapper.Database;
using Microsoft.Extensions.DependencyInjection;

namespace LocationMapper
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //var host = BuildWebHost(args);

            //using (var scope = host.Services.CreateScope())
            //{
            //    var services = scope.ServiceProvider;
            //    var context = services.GetRequiredService<CragLocationContext>();
            //    DbInitialiser.Initialize(context);
            //}

            //host.Run();
        }

        //public static IWebHost BuildWebHost(string[] args) =>
        //    WebHost.CreateDefaultBuilder(args)
        //        .UseStartup<Startup>()
        //        .Build();
    }
}
