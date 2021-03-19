using System;
using System.Collections;

namespace Hangfire.MissionControl.Extensions
{
    internal static class TypeExtensions
    {
        public static object CreateSampleInstance(this Type type)
        {
            var instance = Activator.CreateInstance(type);

            Type propertyType;
            foreach (var property in type.GetProperties())
            {
                propertyType = property.PropertyType;

                if (propertyType.CanBeInstantiated())
                {
                    type.GetProperty(property.Name).SetValue(instance, propertyType.CreateSampleInstance());
                }

                if (typeof(IEnumerable).IsAssignableFrom(propertyType)
                    && propertyType != typeof(string))
                {
                    var elementType = propertyType.IsArray
                        ? propertyType.GetElementType()
                        : propertyType.GenericTypeArguments[0];

                    var array = Array.CreateInstance(elementType, 1);
                    array.SetValue(elementType.CanBeInstantiated() ? elementType.CreateSampleInstance() : default, 0);
                    type.GetProperty(property.Name).SetValue(instance, array);
                }
            }

            return instance;
        }

        public static bool CanBeInstantiated(this Type type) =>
            type.IsClass && type.GetConstructor(Type.EmptyTypes) != null;
    }
}