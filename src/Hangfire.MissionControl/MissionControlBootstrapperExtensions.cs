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

            AddDashboardRouteToEmbeddedResource(
                "/mission-control/js/page",
                "application/js",
                "Hangfire.MissionControl.Dashboard.Content.missions.js");

            AddDashboardRouteToEmbeddedResource(
                "/mission-control/js/moment",
                "application/js",
                "Hangfire.MissionControl.Dashboard.Content.moment.min.js");

            AddDashboardRouteToEmbeddedResource(
                "/mission-control/js/bootstrap-datetimepicker",
                "application/js",
                "Hangfire.MissionControl.Dashboard.Content.bootstrap-datetimepicker.min.js");

            AddDashboardRouteToEmbeddedResource(
                "/mission-control/css/styles",
                "text/css",
                "Hangfire.MissionControl.Dashboard.Content.missions.css");

            AddDashboardRouteToEmbeddedResource(
                "/mission-control/css/bootstrap-datetimepicker",
                "text/css",
                "Hangfire.MissionControl.Dashboard.Content.bootstrap-datetimepicker.min.css");

            return configuration;
        }

        private static void AddDashboardRouteToEmbeddedResource(string route, string contentType, string resourceName)
            => DashboardRoutes.Routes.Add(route, new ContentDispatcher(contentType, resourceName, TimeSpan.FromDays(1)));
    }
}
