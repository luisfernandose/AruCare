using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace AruCareApi
{
    public class Program
    {
        public static System.Net.WebSockets.WebSocket wb = null;
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
