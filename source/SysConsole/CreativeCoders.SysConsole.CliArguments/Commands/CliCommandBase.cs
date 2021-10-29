using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace CreativeCoders.SysConsole.CliArguments.Commands
{
    public abstract class CliCommandBase<TOptions> : ICliCommand<TOptions>
        where TOptions : class, new()
    {
        public abstract Task<CliCommandResult> ExecuteAsync(TOptions options);

        [SuppressMessage("ReSharper", "ConstantConditionalAccessQualifier")]
        [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "<Pending>")]
        public Task<CliCommandResult> ExecuteAsync(object options)
        {
            if (options is not TOptions commandOptions)
            {
                throw new InvalidCastException(
                    $"Option must be of type '{typeof(TOptions).Name}' (type = '{options?.GetType().Name}')");
            }

            return ExecuteAsync(commandOptions);
        }

        public Type OptionsType => typeof(TOptions);

        public bool IsDefault { get; set; }

        public abstract string Name { get; set; }
    }
}
