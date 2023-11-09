using System.Globalization;
using Hangfire.MissionControl.Extensions;
using Hangfire.Server;
using Newtonsoft.Json;

namespace Hangfire.MissionControl.Launching;

internal static class MissionParameterParser
{
    private const string DateTimeParseFormat = "yyyy-MM-dd HH:mm";

    // ReSharper disable once RedundantVerbatimPrefix
    public static (bool ok, object? value, ErrorType error) ParseParameter(Type type, string @value) =>
        type switch
        {
            _ when type == typeof(string) => (true, value, ErrorType.No),
            _ when type == typeof(bool) => TryParse(JsonConvert.DeserializeObject<bool>, value),
            _ when type == typeof(byte) => TryParse(x => byte.Parse(x, NumberStyles.Any, CultureInfo.InvariantCulture), value),
            _ when type == typeof(sbyte) => TryParse(x => sbyte.Parse(x, NumberStyles.Any, CultureInfo.InvariantCulture), value),
            _ when type == typeof(char) => TryParse(x => x[0], value),
            _ when type == typeof(decimal) => TryParse(x => decimal.Parse(x, NumberStyles.Any, CultureInfo.InvariantCulture), value),
            _ when type == typeof(double) => TryParse(x => double.Parse(x, NumberStyles.Any, CultureInfo.InvariantCulture), value),
            _ when type == typeof(float) => TryParse(x => float.Parse(x, NumberStyles.Any, CultureInfo.InvariantCulture), value),
            _ when type == typeof(int) => TryParse(x => int.Parse(x, NumberStyles.Any, CultureInfo.InvariantCulture), value),
            _ when type == typeof(uint) => TryParse(x => uint.Parse(x, NumberStyles.Any, CultureInfo.InvariantCulture), value),
            _ when type == typeof(long) => TryParse(x => long.Parse(x, NumberStyles.Any, CultureInfo.InvariantCulture), value),
            _ when type == typeof(ulong) => TryParse(x => ulong.Parse(x, NumberStyles.Any, CultureInfo.InvariantCulture), value),
            _ when type == typeof(short) => TryParse(x => short.Parse(x, NumberStyles.Any, CultureInfo.InvariantCulture), value),
            _ when type == typeof(ushort) => TryParse(x => ushort.Parse(x, NumberStyles.Any, CultureInfo.InvariantCulture), value),
            _ when type == typeof(DateTime) => TryParse(x => ParseDateTime(x), value),
            _ when type == typeof(DateTimeOffset) => TryParse(x => ParseDateTimeOffset(x), value),
            _ when type == typeof(Guid) => TryParse(Guid.Parse, value),
            _ when type.IsEnum => TryParse(x => Enum.Parse(type, x), value),
            _ when type == typeof(PerformContext) => (true, null, ErrorType.No),
            _ when type == typeof(IJobCancellationToken) => (true, null, ErrorType.No),
            _ when type == typeof(CancellationToken) => (true, null, ErrorType.No),
            var t when t.CanBeInstantiated() => TryParse(x => JsonConvert.DeserializeObject(value, t), value),
            _ => (false, null, ErrorType.Unsupported)
        };

    private static DateTime ParseDateTime(string x) =>
        DateTimeOffset.ParseExact(x, DateTimeParseFormat, CultureInfo.InvariantCulture, DateTimeStyles.None).UtcDateTime;

    private static DateTimeOffset ParseDateTimeOffset(string x) =>
        DateTimeOffset.ParseExact(x, DateTimeParseFormat, CultureInfo.InvariantCulture, DateTimeStyles.None);

    private static (bool ok, object? value, ErrorType error) TryParse<TValue>(Func<string, TValue> parser, string value) =>
        value switch
        {
            null => (false, null, ErrorType.Missing),
            not null when string.IsNullOrWhiteSpace(value) => (false, null, ErrorType.Missing),
            not null => TryParseNonNullValue(parser, value)
        };

    private static (bool ok, object? value, ErrorType error) TryParseNonNullValue<TValue>(Func<string, TValue> parser, string value)
    {
        try
        {
            var deserializedValue = parser(value);
            return (true, deserializedValue, ErrorType.No);
        }
        catch
        {
            return (false, null, ErrorType.Invalid);
        }
    }
}
