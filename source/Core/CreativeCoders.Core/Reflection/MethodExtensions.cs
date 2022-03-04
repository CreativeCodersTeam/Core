using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using CreativeCoders.Core.Collections;

namespace CreativeCoders.Core.Reflection
{
    public static class MethodExtensions
    {
        public static void ExecuteMethod(this object instance, string methodName, params object[] arguments)
        {
            Ensure.IsNotNull(instance, nameof(instance));
            
            var method = arguments == null || arguments.Length == 0
                ? instance.GetType().GetMethod(methodName, Type.EmptyTypes)
                : instance.GetType().GetMethod(methodName, arguments.Select(x => x.GetType()).ToArray());
            
            if (method == null)
            {
                throw new MissingMethodException();
            }

            method.Invoke(instance, arguments);
        }
        
        public static TResult ExecuteMethod<TResult>(this object instance, string methodName, params object[] arguments)
        {
            Ensure.IsNotNull(instance, nameof(instance));
            
            var method = arguments == null || arguments.Length == 0
                ? instance.GetType().GetMethod(methodName, Type.EmptyTypes)
                : instance.GetType().GetMethod(methodName, arguments.Select(x => x.GetType()).ToArray());
            
            if (method == null)
            {
                throw new MissingMethodException();
            }

            return (TResult) method.Invoke(instance, arguments);
        }
        
        public static TResult ExecuteGenericMethod<TResult>(this object instance, string methodName,
            IEnumerable<GenericArgument> genericArguments, params object[] methodParams)
        {
            Ensure.IsNotNull(instance, nameof(instance));
            
            var genericMethod = instance.GetType().GetMethods().FirstOrDefault(method =>
                method.Name == methodName && GenericParamsAreEqual(method.GetGenericArguments(),
                    genericArguments.Select(arg => arg.Name)));

            if (genericMethod == null)
            {
                throw new MissingMethodException(instance.GetType().Name, methodName);
            }

            var noneGenericMethod = genericMethod.MakeGenericMethod(genericArguments.Select(arg => arg.Type).ToArray());

            var resultObject = noneGenericMethod.Invoke(instance, methodParams);
            if (resultObject is TResult result)
            {
                return result;
            }

            return default;
        }

        public static TResult ExecuteGenericMethod<TResult>(this object instance, string methodName,
            Type[] genericTypeArguments, params object[] methodParams)
        {
            Ensure.IsNotNull(instance, nameof(instance));
            
            var noneGenericMethod = CreateGenericMethod(instance, methodName, genericTypeArguments,
                methodParams.Select(p => p.GetType()).ToArray());

            return ExecuteMethod<TResult>(instance, methodParams, noneGenericMethod);
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

        private static MethodInfo CreateGenericMethod(object instance, string methodName, Type[] genericTypeArguments,
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
                    var genericType = type;
                    var index = genericArgumentsList.FindIndex(t => t.Name == genericType.Name);

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

        public static void ExecuteGenericMethod(this object instance, string methodName,
            IEnumerable<GenericArgument> genericArguments, params object[] methodParams)
        {
            ExecuteGenericMethod<object>(instance, methodName, genericArguments, methodParams);
        }

        public static void ExecuteGenericMethod(this object instance, string methodName,
            Type[] genericTypeArguments, params object[] methodParams)
        {
            ExecuteGenericMethod<object>(instance, methodName, genericTypeArguments, methodParams);
        }

        public static Func<object[], TResult> CreateGenericMethodFunc<TResult>(this object instance, string methodName,
            Type[] parameterTypes, params Type[] genericTypeArguments)
        {
            var noneGenericMethod = CreateGenericMethod(instance, methodName, genericTypeArguments, parameterTypes);

            return methodParameters => ExecuteMethod<TResult>(instance, methodParameters, noneGenericMethod);
        }

        public static Func<T, TResult> CreateGenericMethodFunc<T, TResult>(this object instance, string methodName,
            Type[] parameterTypes, params Type[] genericTypeArguments)
        {
            var noneGenericMethod = CreateGenericMethod(instance, methodName, genericTypeArguments, parameterTypes);
            return ExpressionUtils.CreateCallFunc<T, TResult>(instance, noneGenericMethod);
        }

        public static Func<T1, T2, TResult> CreateGenericMethodFunc<T1, T2, TResult>(this object instance, string methodName,
            Type[] parameterTypes, params Type[] genericTypeArguments)
        {
            var noneGenericMethod = CreateGenericMethod(instance, methodName, genericTypeArguments, parameterTypes);

            return ExpressionUtils.CreateCallFunc<T1, T2, TResult>(instance, noneGenericMethod);
        }

        public static Func<T1, T2, T3, TResult> CreateGenericMethodFunc<T1, T2, T3, TResult>(this object instance,
            string methodName,
            Type[] parameterTypes, params Type[] genericTypeArguments)
        {
            var noneGenericMethod = CreateGenericMethod(instance, methodName, genericTypeArguments, parameterTypes);

            return ExpressionUtils.CreateCallFunc<T1, T2, T3, TResult>(instance, noneGenericMethod);
        }

        public static Func<T1, T2, T3, T4, TResult> CreateGenericMethodFunc<T1, T2, T3, T4, TResult>(this object instance,
            string methodName,
            Type[] parameterTypes, params Type[] genericTypeArguments)
        {
            var noneGenericMethod = CreateGenericMethod(instance, methodName, genericTypeArguments, parameterTypes);

            return ExpressionUtils.CreateCallFunc<T1, T2, T3, T4, TResult>(instance, noneGenericMethod);
        }
    }
}
