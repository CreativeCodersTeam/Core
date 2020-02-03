using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace CreativeCoders.Di
{
    [PublicAPI]
    public class MethodExecutor
    {
        public static void Execute(IDiContainer container, object instance, MethodInfo methodInfo)
        {
            var parameters = CreateParameters(container, methodInfo);

            methodInfo.Invoke(instance, parameters.ToArray());
        }

        public static T Execute<T>(IDiContainer container, object instance, MethodInfo methodInfo)
        {
            var parameters = CreateParameters(container, methodInfo);

            return (T) methodInfo.Invoke(instance, parameters.ToArray());
        }

        private static IEnumerable<object> CreateParameters(IDiContainer container, MethodBase methodInfo)
        {
            return methodInfo.GetParameters().Select(p => container.GetInstance(p.ParameterType));
        }
    }
}