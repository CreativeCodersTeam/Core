using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace CreativeCoders.Core.Reflection
{
    public static class MethodExtensions
    {
        public static TResult ExecuteGenericMethod<TResult>(this object obj, string methodName,
            IEnumerable<GenericArgument> genericArguments, params object[] methodParams)
        {
            var genericMethod = obj.GetType().GetMethods().FirstOrDefault(method =>
                method.Name == methodName && GenericParamsAreEqual(method.GetGenericArguments(),
                    genericArguments.Select(arg => arg.Name)));

            if (genericMethod == null)
            {
                throw new MissingMethodException();
            }

            var noneGenericMethod = genericMethod.MakeGenericMethod(genericArguments.Select(arg => arg.Type).ToArray());

            var resultObject = noneGenericMethod.Invoke(obj, methodParams);
            if (resultObject is TResult result)
            {
                return result;
            }

            return default;
        }

        public static TResult ExecuteGenericMethod<TResult>(this object obj, string methodName,
            Type[] genericTypeArguments, params object[] methodParams)
        {
            var noneGenericMethod = CreateGenericMethod(obj, methodName, genericTypeArguments,
                methodParams.Select(p => p.GetType()).ToArray());

            return ExecuteMethod<TResult>(obj, methodParams, noneGenericMethod);
        }

        private static TResult ExecuteMethod<TResult>(object obj, object[] methodParams, MethodBase noneGenericMethod)
        {
            var resultObject = noneGenericMethod.Invoke(obj, methodParams);
            if (resultObject is TResult result)
            {
                return result;
            }

            return default;
        }

        private static MethodInfo CreateGenericMethod(object obj, string methodName, Type[] genericTypeArguments,
            Type[] methodParamTypes)
        {
            var genericMethods = obj
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

        private static bool ParametersMatchExactly(MethodInfo method, Type[] methodParamTypes, Type[] genericTypeArguments)
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

            var parameterTypes = ReplaceGenericTypes(parameterInfos, method.GetGenericArguments(), genericTypeArguments);

            return methodParamTypes.SequenceEqual(parameterTypes);
        }

        [SuppressMessage("ReSharper", "UnusedParameter.Local")]
        private static bool ParameterMatchWithParams(MethodInfo method, Type[] methodParamTypes, Type[] genericTypeArguments)
        {
            throw new NotSupportedException("Dynamic invocation of methods with params argument ist currently not supported");
        }

        private static bool ParametersMatch(MethodBase method, IReadOnlyList<Type> methodParamTypes,
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
        private static IEnumerable<Type> ReplaceGenericTypes(ParameterInfo[] parameterInfos, Type[] genericArguments,
            IReadOnlyList<Type> genericTypeArguments)
        {
            var genericArgumentsList = genericArguments.ToList();

            // ReSharper disable once LoopCanBePartlyConvertedToQuery
            foreach (var parameterInfo in parameterInfos)
            {
                var type = parameterInfo.ParameterType;

                if (type.IsGenericParameter)
                {
                    var index = genericArgumentsList.FindIndex(t => t.Name == type.Name);
                    type = genericTypeArguments[index];
                }

                yield return type;
            }
        }

        private static bool GenericParamsAreEqual(Type[] genericArguments, IEnumerable<string> genericArgumentNames)
        {
            genericArguments = genericArguments.Where(genericArg => genericArg.IsGenericParameter).ToArray();
            return genericArguments.Select(genericArg => genericArg.Name).SequenceEqual(genericArgumentNames);
        }

        public static void ExecuteGenericMethod(this object obj, string methodName,
            IEnumerable<GenericArgument> genericArguments, params object[] methodParams)
        {
            ExecuteGenericMethod<object>(obj, methodName, genericArguments, methodParams);
        }

        public static void ExecuteGenericMethod(this object obj, string methodName,
            Type[] genericTypeArguments, params object[] methodParams)
        {
            ExecuteGenericMethod<object>(obj, methodName, genericTypeArguments, methodParams);
        }

        public static Func<object[], TResult> CreateGenericMethodFunc<TResult>(this object obj, string methodName,
            Type[] parameterTypes, params Type[] genericTypeArguments)
        {
            var noneGenericMethod = CreateGenericMethod(obj, methodName, genericTypeArguments, parameterTypes);

            return methodParameters => ExecuteMethod<TResult>(obj, methodParameters, noneGenericMethod);
        }

        public static Func<T, TResult> CreateGenericMethodFunc<T, TResult>(this object obj, string methodName,
            Type[] parameterTypes, params Type[] genericTypeArguments)
        {
            var noneGenericMethod = CreateGenericMethod(obj, methodName, genericTypeArguments, parameterTypes);
            return ExpressionUtils.CreateCallFunc<T, TResult>(obj, noneGenericMethod);
        }

        public static Func<T1, T2, TResult> CreateGenericMethodFunc<T1, T2, TResult>(this object obj, string methodName,
            Type[] parameterTypes, params Type[] genericTypeArguments)
        {
            var noneGenericMethod = CreateGenericMethod(obj, methodName, genericTypeArguments, parameterTypes);

            return ExpressionUtils.CreateCallFunc<T1, T2, TResult>(obj, noneGenericMethod);
        }

        public static Func<T1, T2, T3, TResult> CreateGenericMethodFunc<T1, T2, T3, TResult>(this object obj,
            string methodName,
            Type[] parameterTypes, params Type[] genericTypeArguments)
        {
            var noneGenericMethod = CreateGenericMethod(obj, methodName, genericTypeArguments, parameterTypes);

            return ExpressionUtils.CreateCallFunc<T1, T2, T3, TResult>(obj, noneGenericMethod);
        }

        public static Func<T1, T2, T3, T4, TResult> CreateGenericMethodFunc<T1, T2, T3, T4, TResult>(this object obj,
            string methodName,
            Type[] parameterTypes, params Type[] genericTypeArguments)
        {
            var noneGenericMethod = CreateGenericMethod(obj, methodName, genericTypeArguments, parameterTypes);

            return ExpressionUtils.CreateCallFunc<T1, T2, T3, T4, TResult>(obj, noneGenericMethod);
        }
    }
}