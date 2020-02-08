using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace CreativeCoders.Core.Reflection
{
    public static class ExpressionUtils
    {
        public static Action CreateCallAction(object instance, MethodInfo methodInfo)
        {
            return CreateCallExpression<Action>(instance, methodInfo);
        }

        public static Action<T> CreateCallAction<T>(object instance, MethodInfo methodInfo)
        {
            var parameterExpression = Expression.Parameter(typeof(T));

            return CreateCallExpression<Action<T>>(instance, methodInfo, parameterExpression);
        }

        public static Action<T1, T2> CreateCallAction<T1, T2>(object instance, MethodInfo methodInfo)
        {
            var parameterExpression1 = Expression.Parameter(typeof(T1));
            var parameterExpression2 = Expression.Parameter(typeof(T2));

            return CreateCallExpression<Action<T1, T2>>(instance, methodInfo,
                parameterExpression1, parameterExpression2);
        }

        public static Action<T1, T2, T3> CreateCallAction<T1, T2, T3>(object instance, MethodInfo methodInfo)
        {
            var parameterExpression1 = Expression.Parameter(typeof(T1));
            var parameterExpression2 = Expression.Parameter(typeof(T2));
            var parameterExpression3 = Expression.Parameter(typeof(T3));

            return CreateCallExpression<Action<T1, T2, T3>>(instance, methodInfo,
                parameterExpression1, parameterExpression2, parameterExpression3);
        }

        public static Action<T1, T2, T3, T4> CreateCallAction<T1, T2, T3, T4>(object instance, MethodInfo methodInfo)
        {
            var parameterExpression1 = Expression.Parameter(typeof(T1));
            var parameterExpression2 = Expression.Parameter(typeof(T2));
            var parameterExpression3 = Expression.Parameter(typeof(T3));
            var parameterExpression4 = Expression.Parameter(typeof(T4));

            return CreateCallExpression<Action<T1, T2, T3, T4>>(instance, methodInfo,
                parameterExpression1, parameterExpression2, parameterExpression3, parameterExpression4);
        }

        public static Func<TResult> CreateCallFunc<TResult>(object instance, MethodInfo methodInfo)
        {
            return CreateCallExpression<Func<TResult>>(instance, methodInfo);
        }

        public static Func<T, TResult> CreateCallFunc<T, TResult>(object instance, MethodInfo methodInfo)
        {
            var parameterExpression = Expression.Parameter(typeof(T));

            return CreateCallExpression<Func<T, TResult>>(instance, methodInfo, parameterExpression);
        }

        public static Func<T1, T2, TResult> CreateCallFunc<T1, T2, TResult>(object instance, MethodInfo methodInfo)
        {
            var parameterExpression1 = Expression.Parameter(typeof(T1));
            var parameterExpression2 = Expression.Parameter(typeof(T2));

            return CreateCallExpression<Func<T1, T2, TResult>>(instance, methodInfo, 
                parameterExpression1, parameterExpression2);
        }

        public static Func<T1, T2, T3, TResult> CreateCallFunc<T1, T2, T3, TResult>(object instance, MethodInfo methodInfo)
        {
            var parameterExpression1 = Expression.Parameter(typeof(T1));
            var parameterExpression2 = Expression.Parameter(typeof(T2));
            var parameterExpression3 = Expression.Parameter(typeof(T3));

            return CreateCallExpression<Func<T1, T2, T3, TResult>>(instance, methodInfo,
                parameterExpression1, parameterExpression2, parameterExpression3);
        }

        public static Func<T1, T2, T3, T4, TResult> CreateCallFunc<T1, T2, T3, T4, TResult>(object instance, MethodInfo methodInfo)
        {
            var parameterExpression1 = Expression.Parameter(typeof(T1));
            var parameterExpression2 = Expression.Parameter(typeof(T2));
            var parameterExpression3 = Expression.Parameter(typeof(T3));
            var parameterExpression4 = Expression.Parameter(typeof(T4));

            return CreateCallExpression<Func<T1, T2, T3, T4, TResult>>(instance, methodInfo,
                parameterExpression1, parameterExpression2, parameterExpression3, parameterExpression4);
        }

        private static T CreateCallExpression<T>(object instance, MethodInfo methodInfo, params ParameterExpression[] arguments)
        {
            var instanceParameter = Expression.Constant(instance);

            var callExpression = Expression.Call(instanceParameter, methodInfo, arguments.OfType<Expression>().ToArray());

            var lambdaExpression = Expression.Lambda<T>(callExpression, arguments);

            return lambdaExpression.Compile();
        }
    }
}