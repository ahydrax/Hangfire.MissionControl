using System;
using System.Reflection;
using Hangfire.Dashboard;
using Hangfire.Server;

namespace Hangfire.MissionControl.Dashboard.Pages.Controls
{
    internal static class ControlFactory
    {
        public static RazorPage CreateControl(ParameterInfo parameter, Mission mission)
        {
            var parameterType = parameter.ParameterType;
            switch (parameterType)
            {
                case var _ when parameterType == typeof(PerformContext):
                case var _ when parameterType == typeof(IJobCancellationToken):
                    return new NullControl();

                case var _ when parameterType == typeof(DateTime):
                    return new DateTimeControl(parameter);

                case var _ when parameterType == typeof(byte):
                case var _ when parameterType == typeof(int):
                case var _ when parameterType == typeof(long):
                    return new IntegerControl(parameter);

                case var _ when parameterType == typeof(float):
                case var _ when parameterType == typeof(double):
                    return new FloatingPointControl(parameter);

                case var _ when parameterType == typeof(bool):
                    return new BooleanControl(parameter);

                case var _ when parameterType == typeof(string):
                    return new StringControl(parameter);

                default:
                    return new StringControl(parameter);
            }
        }
    }
}
