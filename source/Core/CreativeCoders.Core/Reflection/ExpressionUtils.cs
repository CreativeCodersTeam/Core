using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace CreativeCoders.Core.Reflection;

/// <summary>
/// Provides utility methods for creating compiled delegates from expression trees for method invocations.
/// </summary>
public static class ExpressionUtils
{
    /// <summary>
    /// Creates a compiled <see cref="Action"/> delegate that calls the specified method on the given instance.
    /// </summary>
    /// <param name="instance">The object instance on which the method is called.</param>
    /// <param name="methodInfo">The method to invoke.</param>
    /// <returns>A compiled <see cref="Action"/> delegate for the method call.</returns>
    public static Action CreateCallAction(object instance, MethodInfo methodInfo)
    {
        return CreateCallExpression<Action>(instance, methodInfo);
    }

    /// <summary>
    /// Creates a compiled <see cref="Action{T}"/> delegate that calls the specified method on the given instance.
    /// </summary>
    /// <typeparam name="T">The type of the method parameter.</typeparam>
    /// <param name="instance">The object instance on which the method is called.</param>
    /// <param name="methodInfo">The method to invoke.</param>
    /// <returns>A compiled <see cref="Action{T}"/> delegate for the method call.</returns>
    public static Action<T> CreateCallAction<T>(object instance, MethodInfo methodInfo)
    {
        var parameterExpression = Expression.Parameter(typeof(T));

        return CreateCallExpression<Action<T>>(instance, methodInfo, parameterExpression);
    }

    /// <summary>
    /// Creates a compiled <see cref="Action{T1, T2}"/> delegate that calls the specified method on the given instance.
    /// </summary>
    /// <typeparam name="T1">The type of the first method parameter.</typeparam>
    /// <typeparam name="T2">The type of the second method parameter.</typeparam>
    /// <param name="instance">The object instance on which the method is called.</param>
    /// <param name="methodInfo">The method to invoke.</param>
    /// <returns>A compiled <see cref="Action{T1, T2}"/> delegate for the method call.</returns>
    public static Action<T1, T2> CreateCallAction<T1, T2>(object instance, MethodInfo methodInfo)
    {
        var parameterExpression1 = Expression.Parameter(typeof(T1));
        var parameterExpression2 = Expression.Parameter(typeof(T2));

        return CreateCallExpression<Action<T1, T2>>(instance, methodInfo,
            parameterExpression1, parameterExpression2);
    }

    /// <summary>
    /// Creates a compiled <see cref="Action{T1, T2, T3}"/> delegate that calls the specified method on the given instance.
    /// </summary>
    /// <typeparam name="T1">The type of the first method parameter.</typeparam>
    /// <typeparam name="T2">The type of the second method parameter.</typeparam>
    /// <typeparam name="T3">The type of the third method parameter.</typeparam>
    /// <param name="instance">The object instance on which the method is called.</param>
    /// <param name="methodInfo">The method to invoke.</param>
    /// <returns>A compiled <see cref="Action{T1, T2, T3}"/> delegate for the method call.</returns>
    public static Action<T1, T2, T3> CreateCallAction<T1, T2, T3>(object instance, MethodInfo methodInfo)
    {
        var parameterExpression1 = Expression.Parameter(typeof(T1));
        var parameterExpression2 = Expression.Parameter(typeof(T2));
        var parameterExpression3 = Expression.Parameter(typeof(T3));

        return CreateCallExpression<Action<T1, T2, T3>>(instance, methodInfo,
            parameterExpression1, parameterExpression2, parameterExpression3);
    }

    /// <summary>
    /// Creates a compiled <see cref="Action{T1, T2, T3, T4}"/> delegate that calls the specified method on the given instance.
    /// </summary>
    /// <typeparam name="T1">The type of the first method parameter.</typeparam>
    /// <typeparam name="T2">The type of the second method parameter.</typeparam>
    /// <typeparam name="T3">The type of the third method parameter.</typeparam>
    /// <typeparam name="T4">The type of the fourth method parameter.</typeparam>
    /// <param name="instance">The object instance on which the method is called.</param>
    /// <param name="methodInfo">The method to invoke.</param>
    /// <returns>A compiled <see cref="Action{T1, T2, T3, T4}"/> delegate for the method call.</returns>
    public static Action<T1, T2, T3, T4> CreateCallAction<T1, T2, T3, T4>(object instance,
        MethodInfo methodInfo)
    {
        var parameterExpression1 = Expression.Parameter(typeof(T1));
        var parameterExpression2 = Expression.Parameter(typeof(T2));
        var parameterExpression3 = Expression.Parameter(typeof(T3));
        var parameterExpression4 = Expression.Parameter(typeof(T4));

        return CreateCallExpression<Action<T1, T2, T3, T4>>(instance, methodInfo,
            parameterExpression1, parameterExpression2, parameterExpression3, parameterExpression4);
    }

    /// <summary>
    /// Creates a compiled <see cref="Func{TResult}"/> delegate that calls the specified method on the given instance.
    /// </summary>
    /// <typeparam name="TResult">The type of the return value.</typeparam>
    /// <param name="instance">The object instance on which the method is called.</param>
    /// <param name="methodInfo">The method to invoke.</param>
    /// <returns>A compiled <see cref="Func{TResult}"/> delegate for the method call.</returns>
    public static Func<TResult> CreateCallFunc<TResult>(object instance, MethodInfo methodInfo)
    {
        return CreateCallExpression<Func<TResult>>(instance, methodInfo);
    }

    /// <summary>
    /// Creates a compiled <see cref="Func{T, TResult}"/> delegate that calls the specified method on the given instance.
    /// </summary>
    /// <typeparam name="T">The type of the method parameter.</typeparam>
    /// <typeparam name="TResult">The type of the return value.</typeparam>
    /// <param name="instance">The object instance on which the method is called.</param>
    /// <param name="methodInfo">The method to invoke.</param>
    /// <returns>A compiled <see cref="Func{T, TResult}"/> delegate for the method call.</returns>
    public static Func<T, TResult> CreateCallFunc<T, TResult>(object instance, MethodInfo methodInfo)
    {
        var parameterExpression = Expression.Parameter(typeof(T));

        return CreateCallExpression<Func<T, TResult>>(instance, methodInfo, parameterExpression);
    }

    /// <summary>
    /// Creates a compiled <see cref="Func{T1, T2, TResult}"/> delegate that calls the specified method on the given instance.
    /// </summary>
    /// <typeparam name="T1">The type of the first method parameter.</typeparam>
    /// <typeparam name="T2">The type of the second method parameter.</typeparam>
    /// <typeparam name="TResult">The type of the return value.</typeparam>
    /// <param name="instance">The object instance on which the method is called.</param>
    /// <param name="methodInfo">The method to invoke.</param>
    /// <returns>A compiled <see cref="Func{T1, T2, TResult}"/> delegate for the method call.</returns>
    public static Func<T1, T2, TResult> CreateCallFunc<T1, T2, TResult>(object instance,
        MethodInfo methodInfo)
    {
        var parameterExpression1 = Expression.Parameter(typeof(T1));
        var parameterExpression2 = Expression.Parameter(typeof(T2));

        return CreateCallExpression<Func<T1, T2, TResult>>(instance, methodInfo,
            parameterExpression1, parameterExpression2);
    }

    /// <summary>
    /// Creates a compiled <see cref="Func{T1, T2, T3, TResult}"/> delegate that calls the specified method on the given instance.
    /// </summary>
    /// <typeparam name="T1">The type of the first method parameter.</typeparam>
    /// <typeparam name="T2">The type of the second method parameter.</typeparam>
    /// <typeparam name="T3">The type of the third method parameter.</typeparam>
    /// <typeparam name="TResult">The type of the return value.</typeparam>
    /// <param name="instance">The object instance on which the method is called.</param>
    /// <param name="methodInfo">The method to invoke.</param>
    /// <returns>A compiled <see cref="Func{T1, T2, T3, TResult}"/> delegate for the method call.</returns>
    public static Func<T1, T2, T3, TResult> CreateCallFunc<T1, T2, T3, TResult>(object instance,
        MethodInfo methodInfo)
    {
        var parameterExpression1 = Expression.Parameter(typeof(T1));
        var parameterExpression2 = Expression.Parameter(typeof(T2));
        var parameterExpression3 = Expression.Parameter(typeof(T3));

        return CreateCallExpression<Func<T1, T2, T3, TResult>>(instance, methodInfo,
            parameterExpression1, parameterExpression2, parameterExpression3);
    }

    /// <summary>
    /// Creates a compiled <see cref="Func{T1, T2, T3, T4, TResult}"/> delegate that calls the specified method on the given instance.
    /// </summary>
    /// <typeparam name="T1">The type of the first method parameter.</typeparam>
    /// <typeparam name="T2">The type of the second method parameter.</typeparam>
    /// <typeparam name="T3">The type of the third method parameter.</typeparam>
    /// <typeparam name="T4">The type of the fourth method parameter.</typeparam>
    /// <typeparam name="TResult">The type of the return value.</typeparam>
    /// <param name="instance">The object instance on which the method is called.</param>
    /// <param name="methodInfo">The method to invoke.</param>
    /// <returns>A compiled <see cref="Func{T1, T2, T3, T4, TResult}"/> delegate for the method call.</returns>
    public static Func<T1, T2, T3, T4, TResult> CreateCallFunc<T1, T2, T3, T4, TResult>(object instance,
        MethodInfo methodInfo)
    {
        var parameterExpression1 = Expression.Parameter(typeof(T1));
        var parameterExpression2 = Expression.Parameter(typeof(T2));
        var parameterExpression3 = Expression.Parameter(typeof(T3));
        var parameterExpression4 = Expression.Parameter(typeof(T4));

        return CreateCallExpression<Func<T1, T2, T3, T4, TResult>>(instance, methodInfo,
            parameterExpression1, parameterExpression2, parameterExpression3, parameterExpression4);
    }

    private static T CreateCallExpression<T>(object instance, MethodInfo methodInfo,
        params ParameterExpression[] arguments)
    {
        var instanceParameter = Expression.Constant(instance);

        var callExpression =
            Expression.Call(instanceParameter, methodInfo, arguments.OfType<Expression>().ToArray());

        var lambdaExpression = Expression.Lambda<T>(callExpression, arguments);

        return lambdaExpression.Compile();
    }
}
