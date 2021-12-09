using System.ComponentModel;
using System.Reflection;
using Hangfire.Dashboard;

namespace Hangfire.MissionControl.Dashboard.Pages.Controls;

public class EnumControl : RazorPage
{
    public ParameterInfo Parameter { get; }

    public EnumControl(ParameterInfo parameter)
    {
        Parameter = parameter;
    }

    public override void Execute()
    {
        var enumType = Parameter.ParameterType;
        var enumValues = Enum.GetValues(enumType);

        var missionParam = Parameter.GetCustomAttribute<MissionParamAttribute>();
        var defaultValue = missionParam?.DefaultValue ?? enumValues.GetValue(0);

        WriteLiteral("<div class=\"input-group\">");
        WriteLiteral($"<div class=\"input-group-addon\">{Parameter.Name}</div>");
        WriteLiteral($"<select name=\"{Parameter.Name}\" class=\"form-control\">");
        foreach (var enumValue in enumValues)
        {
            var enumValueName = Enum.GetName(enumType, enumValue);
            var description = GetDescription(enumType, enumValueName);
            var value = enumValue.ToString();
            var selected = defaultValue.Equals(enumValue) ? "selected" : string.Empty;
            var label = enumValueName;
            if (description != null)
            {
                label += $" ({description})";
            }

            WriteLiteral($"<option {selected} value=\"{value}\">{label}</option>");
        }
        WriteLiteral("</select>");
        WriteLiteral("</div>");
    }

    private static string GetDescription(Type enumType, string valueName)
        => enumType.GetField(valueName)
            ?.GetCustomAttribute<DescriptionAttribute>()
            ?.Description;
}