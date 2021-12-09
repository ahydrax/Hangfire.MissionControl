﻿using System.Collections.Concurrent;
using System.Reflection;
using System.Text;
using Hangfire.Dashboard;

namespace Hangfire.MissionControl.Dashboard.Pages;

internal static class MissionRenderer
{
    private static readonly ConcurrentDictionary<MethodInfo, NonEscapedString> Cache = new ConcurrentDictionary<MethodInfo, NonEscapedString>();

    public static NonEscapedString RenderMission(MethodInfo methodInfo)
    {
        if (Cache.ContainsKey(methodInfo)) return Cache[methodInfo];

        var rendered = RenderInternal(methodInfo);
        Cache.TryAdd(methodInfo, rendered);

        return rendered;
    }

    private static NonEscapedString RenderInternal(MethodInfo methodInfo)
    {
        var declaringType = methodInfo.DeclaringType;
        var parameters = methodInfo.GetParameters();
        var returnType = methodInfo.ReturnType;

        var builder = new StringBuilder();

        var namespaces = parameters.Select(x => x.ParameterType)
            .Concat(new[] { declaringType, returnType })
            .Select(x => x.Namespace)
            .Distinct()
            .OrderBy(x => x)
            .ToArray();

        foreach (var ns in namespaces)
        {
            AppendNamespace(builder, ns);
        }

        builder.AppendLine();

        var methodName = methodInfo.Name;
        var parameterList = string.Join(", ", parameters.Select(x => $"{Type(x.ParameterType.Name)} {x.Name}"));

        var call = $"{Type(returnType.Name)} {declaringType.Name}.{methodName}({parameterList})";

        builder.Append(call);

        return new NonEscapedString(builder.ToString());
    }

    private static void AppendNamespace(StringBuilder builder, string @namespace)
    {
        builder.Append(Keyword("using"));
        builder.Append(" ");
        builder.Append(@namespace);
        builder.Append(";");
        builder.AppendLine();
    }

    private static string Type(string value)
        => Span("type", value);

    private static string Keyword(string value)
        => Span("keyword", value);

    private static string Span(string @class, string value)
        => $"<span class=\"{@class}\">{value}</span>";
}