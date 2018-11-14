using System;
using Hangfire.Annotations;

namespace Hangfire.MissionControl
{
    [PublicAPI]
    [AttributeUsage(AttributeTargets.Class)]
    public class MissionLauncherAttribute : Attribute
    {
        public string CategoryName { get; set; }
    }
}
