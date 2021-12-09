using System.Reflection;
using Hangfire.Dashboard;

namespace Hangfire.MissionControl.Dashboard.Pages.Controls;

internal class UnsupportedControl : RazorPage
{
    public ParameterInfo Parameter { get; }

    public UnsupportedControl(ParameterInfo parameter)
    {
        Parameter = parameter;
    }

    public override void Execute()
    {
        var missionParam = Parameter.GetCustomAttribute<MissionParamAttribute>();
        var defaultValue = missionParam?.DefaultValue?.ToString() ?? string.Empty;

        WriteLiteral("<div class=\"input-group\">");
        WriteLiteral($"<div class=\"input-group-addon\">{Parameter.Name}</div>");
        WriteLiteral($"<input type=\"text\" name=\"{Parameter.Name}\" readonly placeholder=\"Unsupported type: {Parameter.ParameterType.FullName}\" class=\"form-control\" value=\"{defaultValue}\" />");
        WriteLiteral("</div>");
    }
}