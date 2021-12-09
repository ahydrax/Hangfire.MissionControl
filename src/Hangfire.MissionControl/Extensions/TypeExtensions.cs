using System.Collections;

namespace Hangfire.MissionControl.Extensions;

internal static class TypeExtensions
{
    private const int SampleMaxDepth = 3;

    public static object CreateSampleInstance(this Type type) => type.CreateSampleInstanceInternal(0, SampleMaxDepth);

    private static object CreateSampleInstanceInternal(this Type type, int currentDepth, int maxDepth)
    {
        if (currentDepth > maxDepth) return GetDefaultValue(type);

        var instance = Activator.CreateInstance(type);

        foreach (var property in type.GetProperties())
        {
            var propertyType = property.PropertyType;
            var propertyInfo = type.GetProperty(property.Name)!;
            
            if (propertyType.CanBeInstantiated())
            {
                propertyInfo.SetValue(instance,
                    propertyType.CreateSampleInstanceInternal(currentDepth + 1, SampleMaxDepth));
            }

            if (typeof(IEnumerable).IsAssignableFrom(propertyType) && propertyType != typeof(string))
            {
                var elementType = propertyType.IsArray
                    ? propertyType.GetElementType()!
                    : propertyType.GenericTypeArguments[0];

                var array = Array.CreateInstance(elementType, 1);
                array.SetValue(
                    elementType.CanBeInstantiated()
                        ? elementType.CreateSampleInstanceInternal(currentDepth + 1, SampleMaxDepth)
                        : GetDefaultValue(elementType),
                    0);
                propertyInfo.SetValue(instance, array);
            }
        }

        return instance;
    }

    private static object GetDefaultValue(Type type) => type.IsValueType ? Activator.CreateInstance(type) : null;

    public static bool CanBeInstantiated(this Type type) => type.IsClass && type.GetConstructor(Type.EmptyTypes) != null;
}
