using Hangfire.MissionControl.Model;

namespace Hangfire.MissionControl.Dashboard.Pages;

internal partial class MissionsOverviewPage
{
    public const string Title = "Missions";
    public const string PageRoute = "/missions";

    public string SelectedCategory { get; }
    public MissionMap MissionMap { get; }
    public MissionControlOptions Options { get; }

    public MissionsOverviewPage(string selectedCategory, MissionMap missionMap, MissionControlOptions options)
    {
        SelectedCategory = selectedCategory;
        MissionMap = missionMap;
        Options = options;
    }
}