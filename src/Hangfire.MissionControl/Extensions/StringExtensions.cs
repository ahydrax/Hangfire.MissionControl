using Newtonsoft.Json;
using System.IO;

namespace Hangfire.MissionControl.Extensions
{
    internal static class StringExtensions
    {
        public static string JsonPretiffy(this string json)
        {
            using (var stringReader = new StringReader(json))
            using (var stringWriter = new StringWriter())
            {
                var jsonReader = new JsonTextReader(stringReader);
                var jsonWriter = new JsonTextWriter(stringWriter)
                {
                    Formatting = Formatting.Indented
                };
                jsonWriter.WriteToken(jsonReader);
                return stringWriter.ToString();
            }
        }
    }
}