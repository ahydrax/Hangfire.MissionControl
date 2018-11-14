using System.Reflection;
using Hangfire.Dashboard;

namespace Hangfire.MissionControl.Dashboard.Pages.Controls
{
    internal sealed class DateTimeControl : RazorPage
    {
        public ParameterInfo Parameter { get; }
        
        public DateTimeControl(ParameterInfo parameter)
        {
            Parameter = parameter;
        }

        public override void Execute()
        {
            WriteLiteral("<div class=\"input-group\">");
            WriteLiteral($"<div class=\"input-group-addon\">{Parameter.Name}</div>");
            WriteLiteral($"<input type=\"datetime-local\" name=\"{Parameter.Name}\" placeholder=\"{Parameter.Name}\" class=\"form-control\" />");
            WriteLiteral("</div>");
        }
    }
}
