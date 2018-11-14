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

            foreach (var parameter in methodInfo.GetParameters())
            {
                var parameterType = parameter.ParameterType;
                var parameterValue = (await context.Request.GetFormValuesAsync(parameter.Name)).FirstOrDefault();

                if (parameterType == typeof(string))
                {
                    result.Add(parameterValue ?? string.Empty);
                }
                else if (string.IsNullOrEmpty(parameterValue))
                {
                    result.Add(GetDefaultValue(parameterType));
                }
                else
                {
                    result.Add(JsonConvert.DeserializeObject(parameterValue, parameterType));
                }
            }

            return result.ToArray();
        }

        private static object GetDefaultValue(Type targetType)
        {
            if (targetType.IsValueType)
            {
                return Activator.CreateInstance(targetType);
            }
            return null;
        }
    }
}
