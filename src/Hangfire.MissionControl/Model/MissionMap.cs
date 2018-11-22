using System.Collections.Generic;
using System.Linq;

namespace Hangfire.MissionControl.Model
{
    internal class MissionMap
    {
        public Dictionary<string, int> MissionCategories { get; }
        public Dictionary<string, Mission> Missions { get; }

        public MissionMap(Dictionary<string, Mission> missions)
        {
            Missions = missions.OrderBy(x => x.Value.Name)
                .ToDictionary(x => x.Key, x => x.Value);

            MissionCategories = missions.Values
                .GroupBy(x => x.CategoryName)
                .OrderBy(x => x.Key)
                .ToDictionary(x => x.Key, x => x.Count());
        }
    }
}
