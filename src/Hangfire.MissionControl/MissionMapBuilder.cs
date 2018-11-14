using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Hangfire.MissionControl
{
    internal static class MissionMapBuilder
    {
        public static MissionMap BuildMap(Assembly[] missionAssemblies)
        {
            var targetTypes = missionAssemblies
                .SelectMany(x => x.GetTypes())
                .Where(x => x.GetCustomAttribute<MissionLauncherAttribute>() != null)
                .ToArray();
            
            var missions = LookupForMission(targetTypes).ToDictionary(x => x.Id, x => x);
            if (missions.Count == 0) throw new InvalidOperationException("No missions were found.");
            
            return new MissionMap(missions);
        }

        private static IEnumerable<Mission> LookupForMission(Type[] targetTypes)
        {
            foreach (var targetType in targetTypes)
            {
                var missionLauncherAttribute = targetType.GetCustomAttribute<MissionLauncherAttribute>();
                var suitableMethods = targetType
                    .GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance)
                    .Where(x => x.GetCustomAttribute<MissionAttribute>() != null)
                    .ToArray();

                foreach (var suitableMethod in suitableMethods)
                {
                    var missionAttribute = suitableMethod.GetCustomAttribute<MissionAttribute>();
                    yield return new Mission(missionLauncherAttribute, missionAttribute, suitableMethod);
                }
            }
        }
    }
}
