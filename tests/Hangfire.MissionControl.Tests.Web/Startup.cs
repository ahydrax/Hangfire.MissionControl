using System.Collections.Generic;
using Hangfire.Dashboard;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Hangfire.MissionControl.Tests.Web
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddTransient<ITestSuite4, TestSuite4>();
            services.AddHangfire(configuration =>
            {
                configuration.UseMemoryStorage();
                configuration.UseMissionControl(new MissionControlOptions { RequireConfirmation = false },
                    typeof(Startup).Assembly);
            });
            services.AddHangfireServer();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseHangfireDashboard("/readonly",
                new DashboardOptions
                {
                    IsReadOnlyFunc = ctx => true
                });
            app.UseHangfireDashboard("",
                new DashboardOptions
                {
                    AppPath = "/readonly"
                });
        }
    }
}
