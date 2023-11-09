global using Hangfire.Annotations;
global using System;
using Hangfire.Dashboard;
using Hangfire.MissionControl.Dashboard.Content;
using Hangfire.MissionControl.Dashboard.Pages;
using Hangfire.MissionControl.Launching;
using Hangfire.MissionControl.Mapping;
using Hangfire.MissionControl.Model;
using System.Collections.Generic;
using System.Reflection;

namespace Hangfire.MissionControl;

public static class MissionControlBootstrapperExtensions
{
    private static readonly MissionControlOptions _defaultOptions = new() { RequireConfirmation = true };

    [PublicAPI]
    public static IGlobalConfiguration UseMissionControl(
        this IGlobalConfiguration configuration,
        params Assembly[] missionAssemblies)
        => configuration.UseMissionControl(_defaultOptions, missionAssemblies);

    [PublicAPI]
    public static IGlobalConfiguration UseMissionControl(
        this IGlobalConfiguration configuration,
        MissionControlOptions options,
        params Assembly[] missionAssemblies)
     => configuration.UseMissionControl(options, MissionMapBuilder.BuildMap(missionAssemblies));

    [PublicAPI]
    public static IGlobalConfiguration UseMissionControl(
        this IGlobalConfiguration configuration,
        IDictionary<string, Mission> missions)
        => configuration.UseMissionControl(_defaultOptions, missions);

    [PublicAPI]
    public static IGlobalConfiguration UseMissionControl(
        this IGlobalConfiguration configuration,
        MissionControlOptions options,
        IDictionary<string, Mission> missions)
        => configuration.UseMissionControl(options, new MissionMap(missions));

    [PublicAPI]
    public static IGlobalConfiguration UseMissionControl(
        this IGlobalConfiguration configuration,
        MissionMap map)
        => configuration.UseMissionControl(_defaultOptions, map);

    [PublicAPI]
    public static IGlobalConfiguration UseMissionControl(
        this IGlobalConfiguration configuration,
        MissionControlOptions options,
        MissionMap map)
    {
        DashboardRoutes.Routes.AddRazorPage("/missions",
            x => new MissionsOverviewPage(map.MissionCategories.FirstOrDefault().Key ?? "default", map, options));
        DashboardRoutes.Routes.AddRazorPage("/missions/(?<categoryId>.+)",
            x => new MissionsOverviewPage(x.Groups["categoryId"].Value, map, options));
        DashboardRoutes.Routes.Add("/mission/launch", new MissionLaunchDispatcher(map));

        NavigationMenu.Items.Add(page => new MenuItem(MissionsOverviewPage.Title, page.Url.To(MissionsOverviewPage.PageRoute))
        {
            Active = page.RequestPath.StartsWith(MissionsOverviewPage.PageRoute, StringComparison.OrdinalIgnoreCase),
            Metric = new DashboardMetric("missions-count", x => new Metric(map.Missions.Count))
        });

        AddDashboardRouteToEmbeddedResource(
            "/mission-control/js/jspage",
            "application/javascript",
            "Hangfire.MissionControl.Dashboard.Content.missions.js");

        AddDashboardRouteToEmbeddedResource(
            "/mission-control/js/jsmoment",
            "application/javascript",
            "Hangfire.MissionControl.Dashboard.Content.moment.min.js");

        AddDashboardRouteToEmbeddedResource(
            "/mission-control/js/jsbootstrapdatetimepicker",
            "application/javascript",
            "Hangfire.MissionControl.Dashboard.Content.bootstrap-datetimepicker.min.js");

        AddDashboardRouteToEmbeddedResource(
            "/mission-control/css/cssstyles",
            "text/css",
            "Hangfire.MissionControl.Dashboard.Content.missions.css");

        AddDashboardRouteToEmbeddedResource(
            "/mission-control/css/cssbootstrapdatetimepicker",
            "text/css",
            "Hangfire.MissionControl.Dashboard.Content.bootstrap-datetimepicker.min.css");

        return configuration;
    }

    private static void AddDashboardRouteToEmbeddedResource(string route, string contentType, string resourceName)
        => DashboardRoutes.Routes.Add(route, new ContentDispatcher(contentType, resourceName, TimeSpan.FromDays(1)));
}
