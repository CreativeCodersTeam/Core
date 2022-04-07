using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CreativeCoders.Core.Comparing;

namespace CreativeCoders.Core.Reflection;

public static class MethodInfoExtensions
{
    public static bool ParametersAreEqual(this MethodInfo methodInfo, MethodInfo methodInfoForCompare)
    {
        var parameters1 = methodInfo.GetParameters();
        var parameters2 = methodInfoForCompare.GetParameters();

        if (parameters1.Length != parameters2.Length)
        {
            return false;
        }

        var parametersEqual = parameters1.SequenceEqual(parameters2,
            new MultiFuncEqualityComparer<ParameterInfo, string, Type>(x => x.Name, x => x.ParameterType));

        return parametersEqual;
    }

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

        if (!returnTypeEqual)
        {
            if (methodInfo.ReturnType.IsGenericType || methodInfoForCompare.ReturnType.IsGenericType)
            {
                var genericReturnTypeArguments = methodInfo.ReturnType.GenericTypeArguments;
                var genericReturnTypeArgumentsForCompare =
                    methodInfoForCompare.ReturnType.GenericTypeArguments;

                returnTypeEqual = genericReturnTypeArguments.SequenceEqual(
                    genericReturnTypeArgumentsForCompare,
                    new DelegateEqualityComparer<Type>(
                        (x, y) => x == y || x.IsGenericParameter || y.IsGenericParameter));
            }
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
