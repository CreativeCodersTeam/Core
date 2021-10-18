using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace CreativeCoders.SysConsole.App.Verbs
{
    public static class TypeExtensions
    {
        public static T? CreateInstance<T>(this Type type, IServiceProvider serviceProvider, params object[] args)
            where T : class
        {
            return type.GetConstructors()
                .Select(constructorInfo => CreateInstance<T>(type, constructorInfo, serviceProvider, args))
                .FirstOrDefault(instance => instance != null);
        }

        public static T? CreateInstance<T>(this Type type, ConstructorInfo ctorInfo,
            IServiceProvider serviceProvider,
            params object[] args)
            where T : class
        {
            var argList = args.ToList();

            var argumentInfos = ctorInfo.GetParameters();

            var arguments = argumentInfos
                .Select(x =>
                {
                    var index = argList.FindIndex(argType =>
                        x.ParameterType == argType.GetType()
                        || (argType.GetType().GetInterfaces()
                            .Any(interfaceType => interfaceType == x.ParameterType)));

                    if (index == -1)
                    {
                        return serviceProvider.GetRequiredService(x.ParameterType);
                    }

                    var arg = argList[index];

                    argList.RemoveAt(index);

                    return arg;
                })
                .ToArray();

            if (argList.Count > 0)
            {
                return default;
            }

            return Activator.CreateInstance(type, arguments) as T;
        }
    }
}
