using System;
using System.Diagnostics.CodeAnalysis;

namespace CreativeCoders.Core
{
    [ExcludeFromCodeCoverage]
    public static class FluentInterfaceExtensions
    {
        public static TFluent Fluent<TFluent>(this TFluent fluentInterface, Action fluentAction)
        {
            fluentAction();
            return fluentInterface;
        }
        
        public static TFluent Fluent<TFluent>(this TFluent fluentInterface, Action<TFluent> fluentAction)
        {
            fluentAction(fluentInterface);
            return fluentInterface;
        }
        
        // ReSharper disable once UnusedParameter.Global
        public static TFluent Fluent<TFluent>(this TFluent fluentInterface, Func<TFluent> fluentFunc)
        {
            return fluentFunc();
        }
        
        public static TFluent Fluent<TFluent>(this TFluent fluentInterface, Func<TFluent, TFluent> fluentFunc)
        {
            return fluentFunc(fluentInterface);
        }
        
        public static TFluent FluentIf<TFluent>(this TFluent fluentInterface, bool condition, Action fluentAction)
        {
            if (condition)
            {
                fluentAction();    
            }
            
            return fluentInterface;
        }
        
        public static TFluent FluentIf<TFluent>(this TFluent fluentInterface, bool condition, Action<TFluent> fluentAction)
        {
            if (condition)
            {
                fluentAction(fluentInterface);    
            }
            
            return fluentInterface;
        }
        
        public static TFluent FluentIf<TFluent>(this TFluent fluentInterface, bool condition, Func<TFluent> fluentFunc)
        {
            return condition
                ? fluentFunc()
                : fluentInterface;
        }
        
        public static TFluent FluentIf<TFluent>(this TFluent fluentInterface, bool condition, Func<TFluent, TFluent> fluentFunc)
        {
            return condition
                ? fluentFunc(fluentInterface)
                : fluentInterface;
        }
    }
}