using System;
using System.ComponentModel;
using System.Threading;
using Hangfire.Server;

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

        [Mission]
        public string DoJob(Guid guid) => $"success-{guid:D}";
        
        [Mission]
        public string DoJob([MissionParam(DefaultValue = AType.Blue)] AType aType, BType bType) => $"success-{aType:G}-{bType:G}";

        [Mission]
        public string DoJob(PerformContext ctx, IJobCancellationToken token, CancellationToken ct) => "success";
        
        [Mission]
        public string DoJob(int id, PerformContext ctx, IJobCancellationToken token, CancellationToken ct) => $"success-{id}";
    }

    public enum AType
    {
        Red,
        Blue
    }

    public enum BType
    {
        X77,
        R12,
        
        [Description("Prototype")]
        Proto3
    }

    public class Foo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Bar[] Bars { get; set; }
    }

    public class Bar
    {
        public int Id { get; set; }
        public string Baz { get; set; }
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

        [Mission(Name = "Mission #4", Description = "This is JSON job")]
        public static string DoJsonJob(
            [MissionParam(Description = "This is 1st foo")] Foo foo1,
            [MissionParam(Description = "This is 2nd foo", DefaultValue = "{\"id\":1}")] Foo foo2)
        {
            return "success";
        }
    }

    [MissionLauncher(CategoryName = "Queueing")]
    public static class TestSuite3
    {
        [Mission(Name = "Mission #3", Description = "Mission with specified queue", Queue = "queue1")]
        public static string DoJob(string id, DateTime startOfPeriod, DateTimeOffset endOfPeriod) => "success";
    }

    [MissionLauncher(CategoryName = "Interface")]
    public interface ITestSuite4
    {
        [Mission(Name = "Mission #1", Description = "Mission with interface abstraction")]
        string DoJob(int id, DateTime time);
    }

    public class TestSuite4 : ITestSuite4
    {
        public string DoJob(int id, DateTime time) => "success";
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
