using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using CreativeCoders.Core.Collections;

namespace CreativeCoders.Core.Reflection;

/// <summary>
/// Provides extension methods for dynamically invoking methods on objects using reflection.
/// </summary>
public static class MethodExtensions
{
    /// <summary>
    /// Invokes a method by name on the specified object instance.
    /// </summary>
    /// <param name="instance">The object on which to invoke the method.</param>
    /// <param name="methodName">The name of the method to invoke.</param>
    /// <param name="arguments">The arguments to pass to the method.</param>
    /// <exception cref="MissingMethodException">The method is not found on the instance type.</exception>
    public static void ExecuteMethod(this object instance, string methodName, params object[] arguments)
    {
        Ensure.IsNotNull(instance);

        var method = arguments == null || arguments.Length == 0
            ? instance.GetType().GetMethod(methodName, Type.EmptyTypes)
            : instance.GetType().GetMethod(methodName, arguments.Select(x => x.GetType()).ToArray());

        if (method == null)
        {
            throw new MissingMethodException();
        }

        method.Invoke(instance, arguments);
    }

    /// <summary>
    /// Invokes a method by name on the specified object instance and returns the result.
    /// </summary>
    /// <typeparam name="TResult">The type of the return value.</typeparam>
    /// <param name="instance">The object on which to invoke the method.</param>
    /// <param name="methodName">The name of the method to invoke.</param>
    /// <param name="arguments">The arguments to pass to the method.</param>
    /// <returns>The result of the method invocation cast to <typeparamref name="TResult"/>.</returns>
    /// <exception cref="MissingMethodException">The method is not found on the instance type.</exception>
    public static TResult ExecuteMethod<TResult>(this object instance, string methodName,
        params object[] arguments)
    {
        Ensure.IsNotNull(instance);

        var method = arguments == null || arguments.Length == 0
            ? instance.GetType().GetMethod(methodName, Type.EmptyTypes)
            : instance.GetType().GetMethod(methodName, arguments.Select(x => x.GetType()).ToArray());

        if (method == null)
        {
            throw new MissingMethodException();
        }

        return (TResult)method.Invoke(instance, arguments);
    }

    /// <summary>
    /// Invokes a generic method by name using named generic arguments and returns the result.
    /// </summary>
    /// <typeparam name="TResult">The type of the return value.</typeparam>
    /// <param name="instance">The object on which to invoke the method.</param>
    /// <param name="methodName">The name of the generic method to invoke.</param>
    /// <param name="genericArguments">The named generic type arguments used to construct the generic method.</param>
    /// <param name="methodParams">The parameters to pass to the method.</param>
    /// <returns>The result of the method invocation cast to <typeparamref name="TResult"/>, or the default value if the result cannot be cast.</returns>
    /// <exception cref="MissingMethodException">The generic method is not found on the instance type.</exception>
    public static TResult ExecuteGenericMethod<TResult>(this object instance, string methodName,
        IEnumerable<GenericArgument> genericArguments, params object[] methodParams)
    {
        Ensure.IsNotNull(instance);

        var genericMethod = instance.GetType().GetMethods().FirstOrDefault(method =>
            method.Name == methodName && GenericParamsAreEqual(method.GetGenericArguments(),
                genericArguments.Select(arg => arg.Name)));

        if (genericMethod == null)
        {
            throw new MissingMethodException(instance.GetType().Name, methodName);
        }

        var noneGenericMethod =
            genericMethod.MakeGenericMethod(genericArguments.Select(arg => arg.Type).ToArray());

        var resultObject = noneGenericMethod.Invoke(instance, methodParams);
        if (resultObject is TResult result)
        {
            return result;
        }

        return default;
    }

    /// <summary>
    /// Invokes a generic method by name using type arguments and returns the result.
    /// </summary>
    /// <typeparam name="TResult">The type of the return value.</typeparam>
    /// <param name="instance">The object on which to invoke the method.</param>
    /// <param name="methodName">The name of the generic method to invoke.</param>
    /// <param name="genericTypeArguments">The type arguments used to construct the generic method.</param>
    /// <param name="methodParams">The parameters to pass to the method.</param>
    /// <returns>The result of the method invocation cast to <typeparamref name="TResult"/>, or the default value if the result cannot be cast.</returns>
    /// <exception cref="MissingMethodException">The generic method is not found on the instance type.</exception>
    public static TResult ExecuteGenericMethod<TResult>(this object instance, string methodName,
        Type[] genericTypeArguments, params object[] methodParams)
    {
        Ensure.IsNotNull(instance);

        var noneGenericMethod = CreateGenericMethod(instance, methodName, genericTypeArguments,
            methodParams.Select(p => p.GetType()).ToArray());

        return ExecuteMethod<TResult>(instance, methodParams, noneGenericMethod);
    }

    private static TResult ExecuteMethod<TResult>(object obj, object[] methodParams,
        MethodBase noneGenericMethod)
    {
        var resultObject = noneGenericMethod.Invoke(obj, methodParams);
        if (resultObject is TResult result)
        {
            return result;
        }

        return default;
    }

    private static MethodInfo CreateGenericMethod(object instance, string methodName,
        Type[] genericTypeArguments,
        Type[] methodParamTypes)
    {
        var genericMethods = instance
            .GetType()
            .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
            .Where(method => method.Name == methodName &&
                             genericTypeArguments.Length == method.GetGenericArguments().Length)
            .ToArray();

        var genericMethod = genericMethods.SingleOrDefault(method =>
            ParametersMatchExactly(method, methodParamTypes, genericTypeArguments));

        if (genericMethod == null)
        {
            genericMethod = genericMethods.FirstOrDefault(method =>
                ParametersMatch(method, methodParamTypes, genericTypeArguments));
        }

        if (genericMethod == null)
        {
            throw new MissingMethodException();
        }

        var noneGenericMethod = genericMethod.MakeGenericMethod(genericTypeArguments);
        return noneGenericMethod;
    }

    private static bool ParametersMatchExactly(MethodInfo method, Type[] methodParamTypes,
        Type[] genericTypeArguments)
    {
        var parameterInfos = method.GetParameters();

        if (parameterInfos.LastOrDefault()?.IsParams() == true)
        {
            return ParameterMatchWithParams(method, methodParamTypes, genericTypeArguments);
        }

        if (parameterInfos.Length != methodParamTypes.Length)
        {
            return false;
        }

        var parameterTypes =
            ReplaceGenericTypes(parameterInfos, method.GetGenericArguments(), genericTypeArguments);

        return methodParamTypes.SequenceEqual(parameterTypes);
    }

    [SuppressMessage("ReSharper", "UnusedParameter.Local")]
    private static bool ParameterMatchWithParams(MethodInfo method, Type[] methodParamTypes,
        Type[] genericTypeArguments)
    {
        throw new NotSupportedException(
            "Dynamic invocation of methods with params argument ist currently not supported");
    }

    private static bool ParametersMatch(MethodInfo method, IReadOnlyList<Type> methodParamTypes,
        IReadOnlyList<Type> genericTypeArguments)
    {
        var parameterInfos = method.GetParameters();
        if (parameterInfos.Length != methodParamTypes.Count)
        {
            return false;
        }

        var parameterTypes =
            ReplaceGenericTypes(parameterInfos, method.GetGenericArguments(), genericTypeArguments);

        return parameterTypes
            .SelectWithIndex()
            .All(parameterType => parameterType.Data.IsAssignableFrom(methodParamTypes[parameterType.Index]));
    }

    [SuppressMessage("ReSharper", "ParameterTypeCanBeEnumerable.Local")]
    private static IEnumerable<Type> ReplaceGenericTypes(ParameterInfo[] parameterInfos,
        Type[] genericArguments,
        IReadOnlyList<Type> genericTypeArguments)
    {
        var genericArgumentsList = genericArguments.ToList();

        // ReSharper disable once LoopCanBePartlyConvertedToQuery
        foreach (var parameterInfo in parameterInfos)
        {
            var type = parameterInfo.ParameterType;

            if (type.IsGenericParameter)
            {
                var genericType = type;
                var index = genericArgumentsList.FindIndex(t => t.Name == genericType.Name);

                type = genericTypeArguments[index];
            }

            yield return type;
        }
    }

    private static bool GenericParamsAreEqual(Type[] genericArguments,
        IEnumerable<string> genericArgumentNames)
    {
        genericArguments = genericArguments.Where(genericArg => genericArg.IsGenericParameter).ToArray();
        return genericArguments.Select(genericArg => genericArg.Name).SequenceEqual(genericArgumentNames);
    }

    /// <summary>
    /// Invokes a generic method by name using named generic arguments, discarding the return value.
    /// </summary>
    /// <param name="instance">The object on which to invoke the method.</param>
    /// <param name="methodName">The name of the generic method to invoke.</param>
    /// <param name="genericArguments">The named generic type arguments used to construct the generic method.</param>
    /// <param name="methodParams">The parameters to pass to the method.</param>
    /// <exception cref="MissingMethodException">The generic method is not found on the instance type.</exception>
    public static void ExecuteGenericMethod(this object instance, string methodName,
        IEnumerable<GenericArgument> genericArguments, params object[] methodParams)
    {
        ExecuteGenericMethod<object>(instance, methodName, genericArguments, methodParams);
    }

    /// <summary>
    /// Invokes a generic method by name using type arguments, discarding the return value.
    /// </summary>
    /// <param name="instance">The object on which to invoke the method.</param>
    /// <param name="methodName">The name of the generic method to invoke.</param>
    /// <param name="genericTypeArguments">The type arguments used to construct the generic method.</param>
    /// <param name="methodParams">The parameters to pass to the method.</param>
    /// <exception cref="MissingMethodException">The generic method is not found on the instance type.</exception>
    public static void ExecuteGenericMethod(this object instance, string methodName,
        Type[] genericTypeArguments, params object[] methodParams)
    {
        ExecuteGenericMethod<object>(instance, methodName, genericTypeArguments, methodParams);
    }

    /// <summary>
    /// Creates a compiled delegate that invokes a generic method accepting an object array of parameters and returning a result.
    /// </summary>
    /// <typeparam name="TResult">The type of the return value.</typeparam>
    /// <param name="instance">The object on which the method is called.</param>
    /// <param name="methodName">The name of the generic method.</param>
    /// <param name="parameterTypes">The types of the method parameters used for overload resolution.</param>
    /// <param name="genericTypeArguments">The type arguments used to construct the generic method.</param>
    /// <returns>A compiled delegate that invokes the method and returns the result.</returns>
    /// <exception cref="MissingMethodException">The generic method is not found on the instance type.</exception>
    public static Func<object[], TResult> CreateGenericMethodFunc<TResult>(this object instance,
        string methodName,
        Type[] parameterTypes, params Type[] genericTypeArguments)
    {
        var noneGenericMethod =
            CreateGenericMethod(instance, methodName, genericTypeArguments, parameterTypes);

        return methodParameters => ExecuteMethod<TResult>(instance, methodParameters, noneGenericMethod);
    }

    /// <summary>
    /// Creates a compiled strongly-typed delegate with one parameter that invokes a generic method.
    /// </summary>
    /// <typeparam name="T">The type of the method parameter.</typeparam>
    /// <typeparam name="TResult">The type of the return value.</typeparam>
    /// <param name="instance">The object on which the method is called.</param>
    /// <param name="methodName">The name of the generic method.</param>
    /// <param name="parameterTypes">The types of the method parameters used for overload resolution.</param>
    /// <param name="genericTypeArguments">The type arguments used to construct the generic method.</param>
    /// <returns>A compiled delegate that invokes the method and returns the result.</returns>
    /// <exception cref="MissingMethodException">The generic method is not found on the instance type.</exception>
    public static Func<T, TResult> CreateGenericMethodFunc<T, TResult>(this object instance,
        string methodName,
        Type[] parameterTypes, params Type[] genericTypeArguments)
    {
        var noneGenericMethod =
            CreateGenericMethod(instance, methodName, genericTypeArguments, parameterTypes);
        return ExpressionUtils.CreateCallFunc<T, TResult>(instance, noneGenericMethod);
    }

    /// <summary>
    /// Creates a compiled strongly-typed delegate with two parameters that invokes a generic method.
    /// </summary>
    /// <typeparam name="T1">The type of the first method parameter.</typeparam>
    /// <typeparam name="T2">The type of the second method parameter.</typeparam>
    /// <typeparam name="TResult">The type of the return value.</typeparam>
    /// <param name="instance">The object on which the method is called.</param>
    /// <param name="methodName">The name of the generic method.</param>
    /// <param name="parameterTypes">The types of the method parameters used for overload resolution.</param>
    /// <param name="genericTypeArguments">The type arguments used to construct the generic method.</param>
    /// <returns>A compiled delegate that invokes the method and returns the result.</returns>
    /// <exception cref="MissingMethodException">The generic method is not found on the instance type.</exception>
    public static Func<T1, T2, TResult> CreateGenericMethodFunc<T1, T2, TResult>(this object instance,
        string methodName,
        Type[] parameterTypes, params Type[] genericTypeArguments)
    {
        var noneGenericMethod =
            CreateGenericMethod(instance, methodName, genericTypeArguments, parameterTypes);

        return ExpressionUtils.CreateCallFunc<T1, T2, TResult>(instance, noneGenericMethod);
    }

    /// <summary>
    /// Creates a compiled strongly-typed delegate with three parameters that invokes a generic method.
    /// </summary>
    /// <typeparam name="T1">The type of the first method parameter.</typeparam>
    /// <typeparam name="T2">The type of the second method parameter.</typeparam>
    /// <typeparam name="T3">The type of the third method parameter.</typeparam>
    /// <typeparam name="TResult">The type of the return value.</typeparam>
    /// <param name="instance">The object on which the method is called.</param>
    /// <param name="methodName">The name of the generic method.</param>
    /// <param name="parameterTypes">The types of the method parameters used for overload resolution.</param>
    /// <param name="genericTypeArguments">The type arguments used to construct the generic method.</param>
    /// <returns>A compiled delegate that invokes the method and returns the result.</returns>
    /// <exception cref="MissingMethodException">The generic method is not found on the instance type.</exception>
    public static Func<T1, T2, T3, TResult> CreateGenericMethodFunc<T1, T2, T3, TResult>(this object instance,
        string methodName,
        Type[] parameterTypes, params Type[] genericTypeArguments)
    {
        var noneGenericMethod =
            CreateGenericMethod(instance, methodName, genericTypeArguments, parameterTypes);

        return ExpressionUtils.CreateCallFunc<T1, T2, T3, TResult>(instance, noneGenericMethod);
    }

    /// <summary>
    /// Creates a compiled strongly-typed delegate with four parameters that invokes a generic method.
    /// </summary>
    /// <typeparam name="T1">The type of the first method parameter.</typeparam>
    /// <typeparam name="T2">The type of the second method parameter.</typeparam>
    /// <typeparam name="T3">The type of the third method parameter.</typeparam>
    /// <typeparam name="T4">The type of the fourth method parameter.</typeparam>
    /// <typeparam name="TResult">The type of the return value.</typeparam>
    /// <param name="instance">The object on which the method is called.</param>
    /// <param name="methodName">The name of the generic method.</param>
    /// <param name="parameterTypes">The types of the method parameters used for overload resolution.</param>
    /// <param name="genericTypeArguments">The type arguments used to construct the generic method.</param>
    /// <returns>A compiled delegate that invokes the method and returns the result.</returns>
    /// <exception cref="MissingMethodException">The generic method is not found on the instance type.</exception>
    public static Func<T1, T2, T3, T4, TResult> CreateGenericMethodFunc<T1, T2, T3, T4, TResult>(
        this object instance,
        string methodName,
        Type[] parameterTypes, params Type[] genericTypeArguments)
    {
        var noneGenericMethod =
            CreateGenericMethod(instance, methodName, genericTypeArguments, parameterTypes);

        return ExpressionUtils.CreateCallFunc<T1, T2, T3, T4, TResult>(instance, noneGenericMethod);
    }
}
