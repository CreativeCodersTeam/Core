using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace CreativeCoders.Core.Reflection;

#nullable enable

public static class ParameterInfoExtensions
{
    public static bool IsParams(this ParameterInfo parameterInfo)
    {
        return parameterInfo.IsDefined(typeof(ParamArrayAttribute), false);
    }

    public static object[] CreateArguments(this IEnumerable<ParameterInfo> parameters,
        IServiceProvider serviceProvider, out bool allArgumentsMatched,
        params object[] argumentValues)
    {
        var argList = argumentValues.ToList();

        var arguments = parameters
            .Select(x =>
            {
                var index = argList.FindIndex(argType =>
                    x.ParameterType == argType.GetType()
                    || argType.GetType().GetInterfaces()
                        .Any(interfaceType => interfaceType == x.ParameterType));

                if (index == -1)
                {
                    return serviceProvider.GetRequiredService(x.ParameterType);
                }

                var arg = argList[index];

                argList.RemoveAt(index);

                return arg;
            })
            .ToArray();

        allArgumentsMatched = argList.Count == 0;

        return arguments;
    }
}
