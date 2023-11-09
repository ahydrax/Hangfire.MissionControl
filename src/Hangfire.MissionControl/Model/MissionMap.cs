using System.Collections.Generic;

namespace Hangfire.MissionControl.Model;

public class MissionMap
{
    public IDictionary<string, int> MissionCategories { get; }
    public IDictionary<string, Mission> Missions { get; }

    public MissionMap(IDictionary<string, Mission> missions)
    {
        Missions = missions
            .OrderBy(x => x.Value.Name)
            .ToDictionary(x => x.Key, x => x.Value);

        MissionCategories = missions.Values
            .GroupBy(x => x.CategoryName)
            .OrderBy(x => x.Key)
            .ToDictionary(x => x.Key, x => x.Count());
    }
}