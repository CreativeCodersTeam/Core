using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CreativeCoders.Core.Comparing;

namespace CreativeCoders.Core.Reflection;

#nullable enable

/// <summary>
/// Provides extension methods for <see cref="MethodInfo"/> to support invocation and comparison.
/// </summary>
public static class MethodInfoExtensions
{
    /// <summary>
    /// Invokes the method on the specified instance, resolving parameters from a service provider, and returns the result.
    /// </summary>
    /// <typeparam name="T">The type of the return value.</typeparam>
    /// <param name="methodInfo">The method to invoke.</param>
    /// <param name="instance">The object instance on which to invoke the method.</param>
    /// <param name="serviceProvider">The service provider used to resolve method parameters.</param>
    /// <param name="args">Additional arguments to pass to the method.</param>
    /// <returns>The result of the method invocation cast to <typeparamref name="T"/>.</returns>
    public static T? Execute<T>(this MethodInfo methodInfo, object instance,
        IServiceProvider serviceProvider, params object[] args)
    {
        Ensure.NotNull(methodInfo);
        Ensure.NotNull(serviceProvider);

        var arguments = methodInfo.GetParameters().CreateArguments(serviceProvider, out _, args);

        return (T?) methodInfo.Invoke(instance, arguments);
    }

    /// <summary>
    /// Invokes the method on the specified instance, resolving parameters from a service provider, discarding the return value.
    /// </summary>
    /// <param name="methodInfo">The method to invoke.</param>
    /// <param name="instance">The object instance on which to invoke the method.</param>
    /// <param name="serviceProvider">The service provider used to resolve method parameters.</param>
    /// <param name="args">Additional arguments to pass to the method.</param>
    public static void Execute(this MethodInfo methodInfo, object instance,
        IServiceProvider serviceProvider, params object[] args)
    {
        Ensure.NotNull(methodInfo);
        Ensure.NotNull(serviceProvider);

        var arguments = methodInfo.GetParameters().CreateArguments(serviceProvider, out _, args);

        methodInfo.Invoke(instance, arguments);
    }

    /// <summary>
    /// Determines whether two methods have the same parameter names and types.
    /// </summary>
    /// <param name="methodInfo">The first method to compare.</param>
    /// <param name="methodInfoForCompare">The second method to compare against.</param>
    /// <returns>
    /// <see langword="true"/> if both methods have identical parameter names and types; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool ParametersAreEqual(this MethodInfo methodInfo, MethodInfo methodInfoForCompare)
    {
        var parameters1 = methodInfo.GetParameters();
        var parameters2 = methodInfoForCompare.GetParameters();

        if (parameters1.Length != parameters2.Length)
        {
            return false;
        }

        var parametersEqual = parameters1.SequenceEqual(parameters2,
            new MultiFuncEqualityComparer<ParameterInfo, string?, Type>(x => x.Name, x => x.ParameterType));

        return parametersEqual;
    }

    /// <summary>
    /// Determines whether the specified method matches another method by name, parameters, return type, and generic arguments.
    /// </summary>
    /// <param name="methodInfo">The method to check.</param>
    /// <param name="methodInfoForCompare">The method to compare against.</param>
    /// <returns>
    /// <see langword="true"/> if the methods match; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool MatchesMethod(this MethodInfo methodInfo, MethodInfo methodInfoForCompare)
    {
        var match = methodInfo == methodInfoForCompare;

        if (match)
        {
            return true;
        }

        if (methodInfo.Name != methodInfoForCompare.Name)
        {
            return false;
        }

        var parametersEqual = methodInfo.ParametersAreEqual(methodInfoForCompare);

        if (!parametersEqual)
        {
            return false;
        }

        var returnTypeEqual = methodInfoForCompare.ReturnType.IsAssignableFrom(methodInfo.ReturnType);

        if (!returnTypeEqual && (methodInfo.ReturnType.IsGenericType || methodInfoForCompare.ReturnType.IsGenericType))
        {
            var genericReturnTypeArguments = methodInfo.ReturnType.GenericTypeArguments;
            var genericReturnTypeArgumentsForCompare =
                methodInfoForCompare.ReturnType.GenericTypeArguments;

            returnTypeEqual = genericReturnTypeArguments.SequenceEqual(
                genericReturnTypeArgumentsForCompare,
                new DelegateEqualityComparer<Type>(
                    (x, y) => x == y || x.IsGenericParameter || y.IsGenericParameter));
        }

        if (!returnTypeEqual)
        {
            return false;
        }

        if (!methodInfo.IsGenericMethod)
        {
            return false;
        }

        var genericArguments = methodInfo.GetGenericArguments().ToArray();
        var genericArgumentsForCompare = methodInfoForCompare.GetGenericArguments().ToArray();

        var genericArgsAreEqual = GenericArgsAreEqual(genericArguments, genericArgumentsForCompare);

        return genericArgsAreEqual;
    }

    private static bool GenericArgsAreEqual(IReadOnlyCollection<Type> genericArguments,
        IReadOnlyCollection<Type> genericArgumentsForCompare)
    {
        if (genericArguments.Count != genericArgumentsForCompare.Count)
        {
            return false;
        }

        return genericArguments.SequenceEqual(genericArgumentsForCompare,
            new DelegateEqualityComparer<Type>((x, y) =>
                x == y || x.IsGenericParameter || y.IsGenericParameter));
    }
}
