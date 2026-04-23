using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace CreativeCoders.Core.Reflection;

#nullable enable

/// <summary>
/// Provides extension methods for <see cref="ParameterInfo"/> to support parameter inspection and argument resolution.
/// </summary>
public static class ParameterInfoExtensions
{
    /// <summary>
    /// Determines whether the parameter is decorated with the <see langword="params"/> keyword.
    /// </summary>
    /// <param name="parameterInfo">The parameter to inspect.</param>
    /// <returns>
    /// <see langword="true"/> if the parameter is a params array; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool IsParams(this ParameterInfo parameterInfo)
    {
        return parameterInfo.IsDefined(typeof(ParamArrayAttribute), false);
    }

    /// <summary>
    /// Creates an argument array for the specified parameters by matching provided values by type
    /// and resolving remaining parameters from a service provider.
    /// </summary>
    /// <param name="parameters">The method parameters to create arguments for.</param>
    /// <param name="serviceProvider">The service provider used to resolve unmatched parameters.</param>
    /// <param name="allArgumentsMatched">
    /// When this method returns, <see langword="true"/> if all provided argument values were consumed;
    /// otherwise, <see langword="false"/>.
    /// </param>
    /// <param name="argumentValues">The explicit argument values to match against the parameters.</param>
    /// <returns>An array of resolved argument values in parameter order.</returns>
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
