using System;
using Hangfire.Annotations;

namespace Hangfire.MissionControl
{
    [PublicAPI]
    [AttributeUsage(AttributeTargets.Method)]
    public class MissionAttribute : Attribute
    {
        public string Name { get; set; }
        public string Queue { get; set; }
        public string Description { get; set; }
    }
}
