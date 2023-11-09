using System.Globalization;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Hangfire.MissionControl.Model;

public sealed class Mission
{
    public const string IdField = "id";
    public string Id { get; }
    public string CategoryName { get; }
    public string Name { get; }
    public string? Queue { get; }
    public string? Description { get; }
    public MethodInfo MethodInfo { get; }

    public Mission(MissionLauncherAttribute? launcherAttribute, MissionAttribute? missionAttribute, MethodInfo methodInfo)
        : this(
            methodInfo,
            missionAttribute?.Name ?? methodInfo.Name,
            launcherAttribute?.CategoryName ?? methodInfo.DeclaringType?.Name ?? "Default",
            missionAttribute?.Queue,
            missionAttribute?.Description)
    {
    }

    public Mission(Delegate function,
        string name,
        string? categoryName = "Default",
        string? queue = "default",
        string? description = "")
        : this(
            function.GetMethodInfo(),
            name,
            categoryName,
            queue,
            description)
    {
    }

    public Mission(MethodInfo methodInfo,
        string? name = null,
        string? categoryName = "Default",
        string? queue = "default",
        string? description = "")
    {
        Id = GenerateId(methodInfo);
        CategoryName = categoryName ?? throw new ArgumentNullException(nameof(categoryName));
        Name = name ?? methodInfo.Name;
        Queue = queue;
        Description = description;
        MethodInfo = methodInfo;
    }

    private static string GenerateId(MethodInfo methodInfo)
    {
        var id = GenerateSignature(methodInfo);
        using var crypt = SHA1.Create();
        var hashStringBuilder = new StringBuilder();
        var hash = crypt.ComputeHash(Encoding.ASCII.GetBytes(id));
        foreach (var @byte in hash)
        {
            hashStringBuilder.Append(@byte.ToString("x2", CultureInfo.InvariantCulture));
        }

        return hashStringBuilder.ToString();
    }

    private static string GenerateSignature(MethodInfo methodInfo)
    {
        var declaringType = methodInfo.DeclaringType?.FullName ?? "Unknown";
        var methodName = methodInfo.Name;
        var parameterList = string.Join(", ", methodInfo.GetParameters().Select(x => $"{x.ParameterType.FullName}"));

        return $"{declaringType}.{methodName}({parameterList})";
    }
}
