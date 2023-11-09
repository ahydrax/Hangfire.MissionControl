namespace Hangfire.MissionControl;

[PublicAPI]
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
public class MissionLauncherAttribute : Attribute
{
    public string? CategoryName { get; set; }
}