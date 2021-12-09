using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Hangfire.MissionControl.Model;

internal sealed class Mission
{
    public const string IdField = "id";
    public string Id { get; }
    public string CategoryName { get; }
    public string Name { get; }
    public string Queue { get; }
    public string Description { get; }
    public MethodInfo MethodInfo { get; }

    public Mission(MissionLauncherAttribute launcherAttribute, MissionAttribute missionAttribute, MethodInfo methodInfo)
    {
        Id = GenerateId(methodInfo);
        CategoryName = launcherAttribute.CategoryName ?? methodInfo.DeclaringType?.Name ?? "Default";
        Name = missionAttribute.Name ?? methodInfo.Name;
        Queue = missionAttribute.Queue ?? "default";
        Description = missionAttribute.Description;
        MethodInfo = methodInfo;
    }

    private static string GenerateId(MethodInfo methodInfo)
    {
        var id = GenerateSignature(methodInfo);

        using (var crypt = new SHA1Managed())
        {
            var hashStringBuilder = new StringBuilder();
            var hash = crypt.ComputeHash(Encoding.ASCII.GetBytes(id));
            foreach (var @byte in hash)
            {
                hashStringBuilder.Append(@byte.ToString("x2"));
            }
            return hashStringBuilder.ToString();
        }
    }

    private static string GenerateSignature(MethodInfo methodInfo)
    {
        var declaringType = methodInfo.DeclaringType?.FullName ?? "Unknown";
        var methodName = methodInfo.Name;
        var parameterList = string.Join(", ", methodInfo.GetParameters().Select(x => $"{x.ParameterType.FullName}"));

        return $"{declaringType}.{methodName}({parameterList})";
    }
}