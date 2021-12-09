namespace Hangfire.MissionControl;

[PublicAPI]
public sealed class MissionControlOptions
{
    public bool RequireConfirmation { get; set; }
    public bool HideCodeSnippet { get; set; }
}