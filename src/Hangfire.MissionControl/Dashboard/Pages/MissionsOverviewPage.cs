namespace Hangfire.MissionControl.Dashboard.Pages
{
    internal partial class MissionsOverviewPage
    {
        public const string Title = "Missions";
        public const string PageRoute = "/missions";

        public string SelectedCategory { get; }
        public MissionMap MissionMap { get; }

        public MissionsOverviewPage(string selectedCategory, MissionMap missionMap)
        {
            SelectedCategory = selectedCategory;
            MissionMap = missionMap;
        }
    }
}
