using System;
using CreativeCoders.Core;
using CreativeCoders.Core.Reflection;
using JetBrains.Annotations;

namespace CreativeCoders.Scripting.Base
{
    [PublicAPI]
    public static class ScriptExtensions
    {
        public static Action CreateAction(this IScript script, IScriptContext scriptContext)
        {
            var methodName = script.MethodNames.OnlySingleOrDefault();

            if (methodName == null)
            {
                throw new MissingMethodException();
            }

            return script.CreateAction(methodName, scriptContext);
        }

        public static Action CreateAction(this IScript script, string methodName, IScriptContext scriptContext)
        {
            var scriptObject = script.CreateObject<object>(scriptContext);

            return () => scriptObject.ExecuteMethod(methodName);
        }

        public static Action CreateAction(this IScript script, string methodName)
        {
            return script.CreateAction(methodName, ScriptContext.Empty);
        }

        public static Action CreateAction(this IScript script)
        {
            return script.CreateAction(ScriptContext.Empty);
        }
        

        public static Action<T> CreateAction<T>(this IScript script, IScriptContext scriptContext)
        {
            var methodName = script.MethodNames.OnlySingleOrDefault();

            if (methodName == null)
            {
                throw new MissingMethodException();
            }

            return script.CreateAction<T>(methodName, scriptContext);
        }

        public static Action<T> CreateAction<T>(this IScript script, string methodName, IScriptContext scriptContext)
        {
            var scriptObject = script.CreateObject<object>(scriptContext);

            return arg => scriptObject.ExecuteMethod(methodName, arg);
        }

        public static Action<T> CreateAction<T>(this IScript script, string methodName)
        {
            return script.CreateAction<T>(methodName, ScriptContext.Empty);
        }

        public static Action<T> CreateAction<T>(this IScript script)
        {
            return script.CreateAction<T>(ScriptContext.Empty);
        }
        
        
        public static Action<T1, T2> CreateAction<T1, T2>(this IScript script, IScriptContext scriptContext)
        {
            var methodName = script.MethodNames.OnlySingleOrDefault();

            if (methodName == null)
            {
                throw new MissingMethodException();
            }

            return script.CreateAction<T1, T2>(methodName, scriptContext);
        }

        public static Action<T1, T2> CreateAction<T1, T2>(this IScript script, string methodName, IScriptContext scriptContext)
        {
            var scriptObject = script.CreateObject<object>(scriptContext);

            return (arg1, arg2) => scriptObject.ExecuteMethod(methodName, arg1, arg2);
        }

        public static Action<T1, T2> CreateAction<T1, T2>(this IScript script, string methodName)
        {
            return script.CreateAction<T1, T2>(methodName, ScriptContext.Empty);
        }

        public static Action<T1, T2> CreateAction<T1, T2>(this IScript script)
        {
            return script.CreateAction<T1, T2>(ScriptContext.Empty);
        }
        
        
        public static Action<T1, T2, T3> CreateAction<T1, T2, T3>(this IScript script, IScriptContext scriptContext)
        {
            var methodName = script.MethodNames.OnlySingleOrDefault();

            if (methodName == null)
            {
                throw new MissingMethodException();
            }

            return script.CreateAction<T1, T2, T3>(methodName, scriptContext);
        }

        public static Action<T1, T2, T3> CreateAction<T1, T2, T3>(this IScript script, string methodName, IScriptContext scriptContext)
        {
            var scriptObject = script.CreateObject<object>(scriptContext);

            return (arg1, arg2, arg3) => scriptObject.ExecuteMethod(methodName, arg1, arg2, arg3);
        }

        public static Action<T1, T2, T3> CreateAction<T1, T2, T3>(this IScript script, string methodName)
        {
            return script.CreateAction<T1, T2, T3>(methodName, ScriptContext.Empty);
        }

        public static Action<T1, T2, T3> CreateAction<T1, T2, T3>(this IScript script)
        {
            return script.CreateAction<T1, T2, T3>(ScriptContext.Empty);
        }
        
        
        public static Action<T1, T2, T3, T4> CreateAction<T1, T2, T3, T4>(this IScript script, IScriptContext scriptContext)
        {
            var methodName = script.MethodNames.OnlySingleOrDefault();

            if (methodName == null)
            {
                throw new MissingMethodException();
            }

            return script.CreateAction<T1, T2, T3, T4>(methodName, scriptContext);
        }

        public static Action<T1, T2, T3, T4> CreateAction<T1, T2, T3, T4>(this IScript script, string methodName, IScriptContext scriptContext)
        {
            var scriptObject = script.CreateObject<object>(scriptContext);

            return (arg1, arg2, arg3, arg4) => scriptObject.ExecuteMethod(methodName, arg1, arg2, arg3, arg4);
        }

        public static Action<T1, T2, T3, T4> CreateAction<T1, T2, T3, T4>(this IScript script, string methodName)
        {
            return script.CreateAction<T1, T2, T3, T4>(methodName, ScriptContext.Empty);
        }

        public static Action<T1, T2, T3, T4> CreateAction<T1, T2, T3, T4>(this IScript script)
        {
            return script.CreateAction<T1, T2, T3, T4>(ScriptContext.Empty);
        }
        
        
        public static Func<TResult> CreateFunc<TResult>(this IScript script, IScriptContext scriptContext)
        {
            var methodName = script.MethodNames.OnlySingleOrDefault();

            if (methodName == null)
            {
                throw new MissingMethodException();
            }

            return script.CreateFunc<TResult>(methodName, scriptContext);
        }
        
        public static Func<TResult> CreateFunc<TResult>(this IScript script, string methodName, IScriptContext scriptContext)
        {
            var scriptObject = script.CreateObject<object>(scriptContext);

            return () => scriptObject.ExecuteMethod<TResult>(methodName);
        }
        
        public static Func<TResult> CreateFunc<TResult>(this IScript script, string methodName)
        {
            return script.CreateFunc<TResult>(methodName, ScriptContext.Empty);
        }
        
        public static Func<TResult> CreateFunc<TResult>(this IScript script)
        {
            return script.CreateFunc<TResult>(ScriptContext.Empty);
        }
        
        
        public static Func<T, TResult> CreateFunc<T, TResult>(this IScript script, IScriptContext scriptContext)
        {
            var methodName = script.MethodNames.OnlySingleOrDefault();

            if (methodName == null)
            {
                throw new MissingMethodException();
            }

            return script.CreateFunc<T, TResult>(methodName, scriptContext);
        }
        
        public static Func<T, TResult> CreateFunc<T, TResult>(this IScript script, string methodName, IScriptContext scriptContext)
        {
            var scriptObject = script.CreateObject<object>(scriptContext);

            return arg => scriptObject.ExecuteMethod<TResult>(methodName, arg);
        }
        
        public static Func<T, TResult> CreateFunc<T, TResult>(this IScript script, string methodName)
        {
            return script.CreateFunc<T, TResult>(methodName, ScriptContext.Empty);
        }
        
        public static Func<T, TResult> CreateFunc<T, TResult>(this IScript script)
        {
            return script.CreateFunc<T, TResult>(ScriptContext.Empty);
        }
        
        
        public static Func<T1, T2, TResult> CreateFunc<T1, T2, TResult>(this IScript script, IScriptContext scriptContext)
        {
            var methodName = script.MethodNames.OnlySingleOrDefault();

            if (methodName == null)
            {
                throw new MissingMethodException();
            }

            return script.CreateFunc<T1, T2, TResult>(methodName, scriptContext);
        }
        
        public static Func<T1, T2, TResult> CreateFunc<T1, T2, TResult>(this IScript script, string methodName, IScriptContext scriptContext)
        {
            var scriptObject = script.CreateObject<object>(scriptContext);

            return (arg1, arg2) => scriptObject.ExecuteMethod<TResult>(methodName, arg1, arg2);
        }
        
        public static Func<T1, T2, TResult> CreateFunc<T1, T2, TResult>(this IScript script, string methodName)
        {
            return script.CreateFunc<T1, T2, TResult>(methodName, ScriptContext.Empty);
        }
        
        public static Func<T1, T2, TResult> CreateFunc<T1, T2, TResult>(this IScript script)
        {
            return script.CreateFunc<T1, T2, TResult>(ScriptContext.Empty);
        }
        
        
        public static Func<T1, T2, T3, TResult> CreateFunc<T1, T2, T3, TResult>(this IScript script, IScriptContext scriptContext)
        {
            var methodName = script.MethodNames.OnlySingleOrDefault();

            if (methodName == null)
            {
                throw new MissingMethodException();
            }

            return script.CreateFunc<T1, T2, T3, TResult>(methodName, scriptContext);
        }
        
        public static Func<T1, T2, T3, TResult> CreateFunc<T1, T2, T3, TResult>(this IScript script, string methodName, IScriptContext scriptContext)
        {
            var scriptObject = script.CreateObject<object>(scriptContext);

            return (arg1, arg2, arg3) => scriptObject.ExecuteMethod<TResult>(methodName, arg1, arg2, arg3);
        }
        
        public static Func<T1, T2, T3, TResult> CreateFunc<T1, T2, T3, TResult>(this IScript script, string methodName)
        {
            return script.CreateFunc<T1, T2, T3, TResult>(methodName, ScriptContext.Empty);
        }
        
        public static Func<T1, T2, T3, TResult> CreateFunc<T1, T2, T3, TResult>(this IScript script)
        {
            return script.CreateFunc<T1, T2, T3, TResult>(ScriptContext.Empty);
        }
        
        
        public static Func<T1, T2, T3, T4, TResult> CreateFunc<T1, T2, T3, T4, TResult>(this IScript script, IScriptContext scriptContext)
        {
            var methodName = script.MethodNames.OnlySingleOrDefault();

            if (methodName == null)
            {
                throw new MissingMethodException();
            }

            return script.CreateFunc<T1, T2, T3, T4, TResult>(methodName, scriptContext);
        }
        
        public static Func<T1, T2, T3, T4, TResult> CreateFunc<T1, T2, T3, T4, TResult>(this IScript script, string methodName, IScriptContext scriptContext)
        {
            var scriptObject = script.CreateObject<object>(scriptContext);

            return (arg1, arg2, arg3, arg4) => scriptObject.ExecuteMethod<TResult>(methodName, arg1, arg2, arg3, arg4);
        }
        
        public static Func<T1, T2, T3, T4, TResult> CreateFunc<T1, T2, T3, T4, TResult>(this IScript script, string methodName)
        {
            return script.CreateFunc<T1, T2, T3, T4, TResult>(methodName, ScriptContext.Empty);
        }
        
        public static Func<T1, T2, T3, T4, TResult> CreateFunc<T1, T2, T3, T4, TResult>(this IScript script)
        {
            return script.CreateFunc<T1, T2, T3, T4, TResult>(ScriptContext.Empty);
        }
    }
}