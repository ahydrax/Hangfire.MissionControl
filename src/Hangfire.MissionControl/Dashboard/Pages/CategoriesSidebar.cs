using Hangfire.Dashboard;

namespace Hangfire.MissionControl.Dashboard.Pages
{
    internal sealed class CategoriesSidebar : RazorPage
    {
        public string SelectedCategory { get; }
        public MissionMap MissionMap { get; }

        public CategoriesSidebar(string selectedCategory, MissionMap missionMap)
        {
            SelectedCategory = selectedCategory;
            MissionMap = missionMap;
        }

        public override void Execute()
        {
            WriteLiteral("<div id=\"categories\" class=\"list-group\">");

            foreach (var category in MissionMap.MissionCategories)
            {
                WriteLiteral("<a class=\"list-group-item");
                if (category.Key == SelectedCategory)
                {
                    WriteLiteral(" active");
                }
                WriteLiteral("\"href=\"");
                WriteLiteral(Url.To($"/missions/{category.Key}"));
                WriteLiteral("\">");
                WriteLiteral(category.Key);
                WriteLiteral("<span class=\"pull-right\">");
                WriteLiteral($"<span class=\"metric\">{category.Value}</span>");
                WriteLiteral("</span>");
                WriteLiteral("</a>");
            }

            WriteLiteral("</div>");
        }

        public static NonEscapedString Render(HtmlHelper helper, string selectedCategory, MissionMap missionMap)
            => helper.RenderPartial(new CategoriesSidebar(selectedCategory, missionMap));
    }
}
