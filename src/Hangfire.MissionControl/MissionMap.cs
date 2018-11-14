using System.Collections.Generic;
using System.Linq;

namespace Hangfire.MissionControl
{
    internal class MissionMap
    {
        public Dictionary<string, int> MissionCategories { get; }
        public Dictionary<string, Mission> Missions { get; }

        public MissionMap(Dictionary<string, Mission> missions)
        {
            Missions = missions;

            MissionCategories = missions.Values
                .GroupBy(x => x.CategoryName)
                .ToDictionary(x => x.Key, x => x.Count());
        }
    }
}
