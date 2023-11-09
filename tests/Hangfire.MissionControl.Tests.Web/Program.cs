using Hangfire;
using Hangfire.MemoryStorage;
using Hangfire.MissionControl;
using Hangfire.MissionControl.Tests.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<ITestSuite4, TestSuite4>();
builder.Services.AddHangfire(configuration =>
{
    configuration.UseMemoryStorage();
    configuration.UseMissionControl(
        new MissionControlOptions
        {
            RequireConfirmation = false,
            HideCodeSnippet = false
        },
        typeof(TestSuite).Assembly);
});
builder.Services.AddHangfireServer();

var app = builder.Build();
app.UseDeveloperExceptionPage();
app.UseHangfireDashboard("/readonly",
    new DashboardOptions
    {
        IsReadOnlyFunc = _ => true
    });
app.UseHangfireDashboard("",
    new DashboardOptions
    {
        AppPath = "/readonly"
    });

app.Run();
