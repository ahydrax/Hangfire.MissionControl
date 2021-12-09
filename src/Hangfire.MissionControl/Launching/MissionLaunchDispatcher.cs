using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Hangfire.Common;
using Hangfire.Dashboard;
using Hangfire.MissionControl.Model;
using Hangfire.States;

namespace Hangfire.MissionControl.Launching;

internal sealed class MissionLaunchDispatcher : IDashboardDispatcher
{
    public MissionMap MissionMap { get; }

    public MissionLaunchDispatcher(MissionMap missionMap)
    {
        MissionMap = missionMap;
    }

    public async Task Dispatch(DashboardContext context)
    {
        if (context.IsReadOnly)
        {
            context.Response.StatusCode = 401;
            return;
        }
            
        if (!"POST".Equals(context.Request.Method, StringComparison.InvariantCultureIgnoreCase))
        {
            context.Response.StatusCode = 405;
            return;
        }

        try
        {
            var missionId = context.Request.GetQuery(Mission.IdField);

            var mission = MissionMap.Missions[missionId];

            var (parameters, errors) = await CreateParameters(context, mission.MethodInfo);
            if (errors.Length > 0)
            {
                context.Response.StatusCode = 400;

                var errorBuilder = new StringBuilder();
                errorBuilder.Append("Missing parameters: ");
                errorBuilder.Append(string.Join(", ", errors.Where(x => x.error == ErrorType.Missing).Select(x => x.fieldName)));

                var invalid = string.Join(", ", errors.Where(x => x.error == ErrorType.Invalid).Select(x => x.fieldName));
                if (!string.IsNullOrWhiteSpace(invalid))
                {
                    errorBuilder.Append("<br/>");
                    errorBuilder.Append("Invalid parameters: ");
                    errorBuilder.Append(invalid);
                }

                await context.Response.WriteAsync(errorBuilder.ToString());
            }
            else
            {
                var jobId = context.GetBackgroundJobClient().Create(new Job(mission.MethodInfo, parameters), new EnqueuedState(mission.Queue));

                context.Response.StatusCode = 201;
                await context.Response.WriteAsync(jobId);
            }
        }
        catch (Exception e)
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync(e.Message);
        }
    }

    private static async Task<(object[] parameters, (string fieldName, ErrorType error)[] errors)> CreateParameters(DashboardContext context, MethodInfo methodInfo)
    {
        var parameters = methodInfo.GetParameters();
        var result = new List<object>(parameters.Length);
        var errors = new List<(string fieldName, ErrorType error)>();

        foreach (var parameter in parameters)
        {
            var parameterName = parameter.Name;
            var parameterType = parameter.ParameterType;
            var parameterValue = (await context.Request.GetFormValuesAsync(parameterName)).LastOrDefault();

            object value;
            ErrorType error;
            bool ok;

            (value, error, ok) = MissionParameterParser.ParseParameter(parameterType, parameterValue);

            if (ok)
            {
                result.Add(value);
            }
            else
            {
                errors.Add((parameterName, error));
            }
        }

        return (result.ToArray(), errors.ToArray());
    }
}