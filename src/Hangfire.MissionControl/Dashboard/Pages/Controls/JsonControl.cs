using Hangfire.Dashboard;
using Hangfire.MissionControl.Extensions;
using Newtonsoft.Json;
using System.Reflection;

namespace Hangfire.MissionControl.Dashboard.Pages.Controls
{
    public class JsonControl : RazorPage
    {
        public ParameterInfo Parameter { get; }

        public JsonControl(ParameterInfo parameter)
        {
            Parameter = parameter;
        }

        public override void Execute()
        {
            var missionParam = Parameter.GetCustomAttribute<MissionParamAttribute>();
            var description = missionParam?.Description ?? Parameter.Name;
            var defaultValue = missionParam?.DefaultValue?.ToString()
                ?? JsonConvert.SerializeObject(Parameter.ParameterType.CreateSampleInstance());
            var sampleJson = defaultValue.JsonPretiffy();

            WriteLiteral("<div class=\"input-group\">");
            WriteLiteral($"<div class=\"input-group-addon\">{Parameter.Name}</div>");
            WriteLiteral($"<textarea name=\"{Parameter.Name}\" placeholder=\"{description}\" " +
                         $"rows=\"{sampleJson.Split('\n').Length}\" cols=\"50\" class=\"form-control\">");
            WriteLiteral(sampleJson);
            WriteLiteral("</textarea>");
            WriteLiteral("</div>");
        }
    }
}