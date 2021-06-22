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
                configuration.UseMissionControl(new MissionControlOptions { RequireConfirmation = false }, typeof(Startup).Assembly);
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseHangfireServer();
            app.UseHangfireDashboard("", new DashboardOptions { Authorization = new List<IDashboardAuthorizationFilter>() });
        }
    }
}
