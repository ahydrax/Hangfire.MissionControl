using System.Reflection;
using Hangfire.Dashboard;

namespace Hangfire.MissionControl.Dashboard.Pages.Controls;

internal sealed class DateTimeControl : RazorPage
{
    public ParameterInfo Parameter { get; }

    public DateTimeControl(ParameterInfo parameter)
    {
        Parameter = parameter;
    }

    public override void Execute()
    {
        WriteLiteral("<div class=\"input-group date\">");
        WriteLiteral($"<span class=\"input-group-addon\">{Parameter.Name}</span>");
        WriteLiteral($"<input type=\"text\" class=\"form-control datetimepicker\" name=\"{Parameter.Name}\" placeholder=\"{Parameter.Name}\"/>");
        WriteLiteral("</div>");
    }
}