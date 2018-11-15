using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Hangfire.Common;
using Hangfire.Dashboard;
using Hangfire.States;
using Newtonsoft.Json;

namespace Hangfire.MissionControl
{
    internal sealed class MissionLauncher : IDashboardDispatcher
    {
        public MissionMap MissionMap { get; }

        public MissionLauncher(MissionMap missionMap)
        {
            MissionMap = missionMap;
        }

        public async Task Dispatch(DashboardContext context)
        {
            if (!"POST".Equals(context.Request.Method, StringComparison.InvariantCultureIgnoreCase))
            {
                context.Response.StatusCode = 405;
                return;
            }

            try
            {
                var missionId = context.Request.GetQuery(Mission.IdField);

                var mission = MissionMap.Missions[missionId];

                var parameters = await CreateParameters(context, mission.MethodInfo);
                var jobId = context.GetBackgroundJobClient().Create(new Job(mission.MethodInfo, parameters), new EnqueuedState());

                context.Response.StatusCode = 200;
                await context.Response.WriteAsync(jobId);
            }
            catch (Exception e)
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync(e.Message);
            }
        }

        private static async Task<object[]> CreateParameters(DashboardContext context, MethodInfo methodInfo)
        {
            var result = new List<object>();
            var missingFields = new List<string>();

            foreach (var parameter in methodInfo.GetParameters())
            {
                var parameterType = parameter.ParameterType;
                var parameterValue = (await context.Request.GetFormValuesAsync(parameter.Name)).LastOrDefault();


                switch (parameterValue)
                {
                    case string stringValue when parameterType == typeof(string):
                        result.Add(stringValue);
                        break;

                    case string primitiveValue when !string.IsNullOrWhiteSpace(parameterValue):
                        result.Add(JsonConvert.DeserializeObject(primitiveValue, parameterType));
                        break;

                    default:
                        missingFields.Add(parameter.Name);
                        continue;
                }
            }

            if (missingFields.Count > 0)
                throw new InvalidOperationException($"Fields are missing: {string.Join(",", missingFields)}");

            return result.ToArray();
        }
    }
}
