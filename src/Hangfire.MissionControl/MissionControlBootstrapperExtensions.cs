using System;
using System.Linq;
using System.Reflection;
using Hangfire.Annotations;
using Hangfire.Dashboard;
using Hangfire.MissionControl.Dashboard.Content;
using Hangfire.MissionControl.Dashboard.Pages;

namespace Hangfire.MissionControl
{
    public static class MissionControlBootstrapperExtensions
    {
        [PublicAPI]
        public static IGlobalConfiguration UseMissionControl(
            this IGlobalConfiguration configuration,
            params Assembly[] missionAssemblies)
            => configuration.UseMissionControl(new MissionControlOptions { RequireConfirmation = true }, missionAssemblies);

        [PublicAPI]
        public static IGlobalConfiguration UseMissionControl(
            this IGlobalConfiguration configuration,
            MissionControlOptions options,
            params Assembly[] missionAssemblies)
        {
            var map = MissionMapBuilder.BuildMap(missionAssemblies);

            DashboardRoutes.Routes.AddRazorPage("/missions", x => new MissionsOverviewPage(map.MissionCategories.FirstOrDefault().Key ?? "default", map, options));
            DashboardRoutes.Routes.AddRazorPage("/missions/(?<categoryId>.+)", x => new MissionsOverviewPage(x.Groups["categoryId"].Value, map, options));
            DashboardRoutes.Routes.Add("/mission/launch", new MissionLauncher(map));

            NavigationMenu.Items.Add(page => new MenuItem(MissionsOverviewPage.Title, page.Url.To(MissionsOverviewPage.PageRoute))
            {
                Active = page.RequestPath.StartsWith(MissionsOverviewPage.PageRoute),
                Metric = new DashboardMetric("missions-count", x => new Metric(map.Missions.Count))
            });

            DashboardRoutes.Routes.Add(
                "/mission-control/jspage",
                new ContentDispatcher("application/js", "Hangfire.MissionControl.Dashboard.Content.missions.js", TimeSpan.FromDays(1)));

            DashboardRoutes.Routes.Add(
                "/mission-control/cssstyles",
                new ContentDispatcher("text/css", "Hangfire.MissionControl.Dashboard.Content.missions.css", TimeSpan.FromDays(1)));

            return configuration;
        }
    }
}
