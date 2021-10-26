﻿using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CreativeCoders.Core;
using CreativeCoders.Core.Collections;
using CreativeCoders.SysConsole.CliArguments.Commands;

namespace CreativeCoders.SysConsole.CliArguments.Building
{
    public static class CliBuilderExtensions
    {
        public static ICliBuilder AddCommand<TOptions>(this ICliBuilder cliBuilder,
            string name, Func<TOptions, Task<CliCommandResult>> executeAsync)
            where TOptions : class, new()
        {
            Ensure.IsNotNullOrWhitespace(name, nameof(name));
            Ensure.NotNull(executeAsync, nameof(executeAsync));

            cliBuilder.AddCommand<DelegateCliCommand<TOptions>, TOptions>(x =>
            {
                x.Name = name;
                x.OnExecuteAsync = executeAsync;
            });

            return cliBuilder;
        }

        public static ICliBuilder AddModule(this ICliBuilder cliBuilder, Type cliModuleType)
        {
            Ensure.NotNull(cliModuleType, nameof(cliModuleType));

            if (Activator.CreateInstance(cliModuleType) is ICliModule cliModule)
            {
                cliModule.Configure(cliBuilder);
            }

            return cliBuilder;
        }

        public static ICliBuilder AddModule(this ICliBuilder cliBuilder, Assembly assembly)
        {
            Ensure.NotNull(assembly, nameof(assembly));

            var cliModuleTypes = assembly
                .ExportedTypes
                .Where(x => !x.IsAbstract && !x.IsInterface && x.IsAssignableTo(typeof(ICliModule)));

            cliModuleTypes.ForEach(x => cliBuilder.AddModule(x));

            return cliBuilder;
        }
    }
}
