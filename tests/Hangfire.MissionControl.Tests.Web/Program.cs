using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Hangfire.MissionControl.Tests.Web
{
    public static class Program
    {
        public static void Main(string[] args) => 
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build()
                .Run();
    }
}
