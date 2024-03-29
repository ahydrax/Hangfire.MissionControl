﻿using Hangfire.Dashboard;
using Hangfire.MissionControl.Extensions;
using Hangfire.Server;
using System.Reflection;
using System.Threading;

namespace Hangfire.MissionControl.Dashboard.Pages.Controls;

internal static class ControlFactory
{
    public static RazorPage CreateControl(ParameterInfo parameter)
    {
        var parameterType = parameter.ParameterType;
        switch (parameterType)
        {
            case var _ when parameterType == typeof(char):
            case var _ when parameterType == typeof(string):
                return new StringControl(parameter);

            case var _ when parameterType == typeof(bool):
                return new BooleanControl(parameter);

            case var _ when parameterType == typeof(decimal):
            case var _ when parameterType == typeof(double):
            case var _ when parameterType == typeof(float):
                return new FloatingPointControl(parameter);

            case var _ when parameterType == typeof(byte):
            case var _ when parameterType == typeof(sbyte):
            case var _ when parameterType == typeof(int):
            case var _ when parameterType == typeof(uint):
            case var _ when parameterType == typeof(long):
            case var _ when parameterType == typeof(ulong):
            case var _ when parameterType == typeof(short):
            case var _ when parameterType == typeof(ushort):
                return new IntegerControl(parameter);

            case var _ when parameterType == typeof(DateTime):
                return new DateTimeControl(parameter);

            case var _ when parameterType == typeof(DateTimeOffset):
                return new DateTimeControl(parameter);

            case var _ when parameterType == typeof(Guid):
                return new GuidControl(parameter);

            case var t when t.IsEnum:
                return new EnumControl(parameter);

            case var _ when parameterType == typeof(PerformContext):
            case var _ when parameterType == typeof(IJobCancellationToken):
            case var _ when parameterType == typeof(CancellationToken):
                return new NullControl();

            case var _ when parameterType.CanBeInstantiated():
                return new JsonControl(parameter);

            default:
                return new UnsupportedControl(parameter);
        }
    }
}