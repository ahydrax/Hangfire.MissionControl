using System.Globalization;
using System.Threading;
using Hangfire.MissionControl.Extensions;
using Hangfire.Server;
using Newtonsoft.Json;

namespace Hangfire.MissionControl.Launching;

internal static class MissionParameterParser
{
    public static (object value, ErrorType error, bool ok) ParseParameter(Type parameterType, string parameterValue)
    {
        switch (parameterType)
        {
            case var t when t == typeof(string):
                return (parameterValue, ErrorType.No, true);

            case var t when t == typeof(bool):
                return TryParse(JsonConvert.DeserializeObject<bool>, parameterValue);

            case var t when t == typeof(byte):
                return TryParse(x => byte.Parse(x, NumberStyles.Any, CultureInfo.InvariantCulture), parameterValue);

            case var t when t == typeof(sbyte):
                return TryParse(x => sbyte.Parse(x, NumberStyles.Any, CultureInfo.InvariantCulture),
                    parameterValue);

            case var t when t == typeof(char):
                return TryParse(x => x[0], parameterValue);

            case var t when t == typeof(decimal):
                return TryParse(x => decimal.Parse(x, NumberStyles.Any, CultureInfo.InvariantCulture),
                    parameterValue);

            case var t when t == typeof(double):
                return TryParse(x => double.Parse(x, NumberStyles.Any, CultureInfo.InvariantCulture),
                    parameterValue);

            case var t when t == typeof(float):
                return TryParse(x => float.Parse(x, NumberStyles.Any, CultureInfo.InvariantCulture),
                    parameterValue);

            case var t when t == typeof(int):
                return TryParse(x => int.Parse(x, NumberStyles.Any, CultureInfo.InvariantCulture), parameterValue);

            case var t when t == typeof(uint):
                return TryParse(x => uint.Parse(x, NumberStyles.Any, CultureInfo.InvariantCulture), parameterValue);

            case var t when t == typeof(long):
                return TryParse(x => long.Parse(x, NumberStyles.Any, CultureInfo.InvariantCulture), parameterValue);

            case var t when t == typeof(ulong):
                return TryParse(x => ulong.Parse(x, NumberStyles.Any, CultureInfo.InvariantCulture),
                    parameterValue);

            case var t when t == typeof(short):
                return TryParse(x => short.Parse(x, NumberStyles.Any, CultureInfo.InvariantCulture),
                    parameterValue);

            case var t when t == typeof(ushort):
                return TryParse(x => ushort.Parse(x, NumberStyles.Any, CultureInfo.InvariantCulture),
                    parameterValue);

            case var t when t == typeof(DateTime):
                return TryParse(
                    x => DateTimeOffset.ParseExact(x, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None)
                        .UtcDateTime,
                    parameterValue);

            case var t when t == typeof(DateTimeOffset):
                return TryParse(
                    x => DateTimeOffset.ParseExact(x, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None),
                    parameterValue);

            case var t when t == typeof(Guid):
                return TryParse(Guid.Parse, parameterValue);
                
            case var t when t.IsEnum:
                return TryParse(x => Enum.Parse(parameterType, x), parameterValue);

            case var pctx when pctx == typeof(PerformContext):
            case var jct when jct == typeof(IJobCancellationToken):
            case var ct when ct == typeof(CancellationToken):
                return (null, ErrorType.No, true);

            case var t when t.CanBeInstantiated():
                return TryParse(x => JsonConvert.DeserializeObject(parameterValue, t), parameterValue);

            default:
                return (null, ErrorType.Unsupported, false);
        }
    }

    private static (object value, ErrorType error, bool ok) TryParse<TValue>(Func<string, TValue> parser, string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return (null, ErrorType.Missing, false);

        try
        {
            var deserializedValue = parser(value);
            return (deserializedValue, ErrorType.No, true);
        }
        catch
        {
            return (null, ErrorType.Invalid, false);
        }
    }
}