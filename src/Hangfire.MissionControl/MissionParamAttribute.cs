namespace Hangfire.MissionControl;

[PublicAPI]
[AttributeUsage(AttributeTargets.Parameter)]
public class MissionParamAttribute : Attribute
{
    public object DefaultValue { get; set; }
    public string Description { get; set; }
}