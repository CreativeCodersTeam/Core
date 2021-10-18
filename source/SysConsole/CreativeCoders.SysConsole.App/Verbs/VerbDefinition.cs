using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace CreativeCoders.SysConsole.App.Verbs
{
    public class VerbDefinition
    {
        private readonly Type _verbType;

        public VerbDefinition(Type verbType)
        {
            _verbType = verbType;
            OptionsType = GetOptionsType(verbType);
        }
        
        private static Type? GetOptionsType(IReflect verbType)
        {
            var optionsTypeProperty = verbType.GetProperty("OptionsType",
                BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);

            return optionsTypeProperty?.GetValue(null) as Type;
        }
        
        public IVerb? CreateVerb(object options, IServiceProvider serviceProvider)
        {
            var ctorInfo = _verbType.GetConstructors().Single();

            var argumentInfos = ctorInfo.GetParameters();

            var arguments = argumentInfos
                .Select(x =>
                    x.ParameterType == options.GetType()
                        ? options
                        : serviceProvider.GetRequiredService(x.ParameterType))
                .ToArray();
            
            var verb = Activator.CreateInstance(_verbType, arguments) as IVerb;

            return verb;
        }

        public Type? OptionsType { get; }
    }
}
