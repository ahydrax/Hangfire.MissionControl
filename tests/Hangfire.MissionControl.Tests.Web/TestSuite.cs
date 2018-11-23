using System;

namespace Hangfire.MissionControl.Tests.Web
{
    [MissionLauncher(CategoryName = "Primitives")]
    public class TestSuite
    {
        [Mission]
        public string DoJob(string name) => "success";

        [Mission]
        public string DoJob(bool isChecked) => "success";

        [Mission]
        public string DoJob(byte byteValue) => "success";

        [Mission]
        public string DoJob(int intValue) => "success";

        [Mission]
        public string DoJob(long longValue) => "success";

        [Mission]
        public string DoJob(float floatValue) => "success";

        [Mission]
        public string DoJob(double doubleValue) => "success";

        [Mission]
        public string DoJob(DateTime dataTime) => "success";
    }

    [MissionLauncher(CategoryName = "Descriptions")]
    public class TestSuite2
    {
        [Mission(Name = "Mission #1")]
        public string DoJob(int id, DateTime time) => "success";

        [Mission(Name = "Mission #2", Description = "This is very important mission")]
        public static string DoAnotherJob(double value) => "success2";

        [Mission(Name = "Mission #3", Description = "This is very important mission")]
        public static string DoAnotherJob(
            [MissionParam(Description = "Customer name", DefaultValue = "Jack")] string customerName) => "success2";
    }

    [MissionLauncher(CategoryName = "Queueing")]
    public static class TestSuite3
    {
        [Mission(Name = "Mission #3", Description = "Mission with specified queue", Queue = "queue1")]
        public static string DoJob(string id, DateTime startOfPeriod, DateTimeOffset endOfPeriod) => "success";
    }

    [MissionLauncher(CategoryName = "Migrations")]
    public static class UserMigrator
    {
        [Mission(Name = "[2018-11-10] Update user DB", Description = "Remove unused fields")]
        public static string Migrate() => "success";
    }

    [MissionLauncher(CategoryName = "Migrations")]
    public static class EventMigrator
    {
        [Mission(Name = "[2018-11-12] Migrate events db", Description = "Add event specific fields")]
        public static string Migrate() => "success";
    }
}
