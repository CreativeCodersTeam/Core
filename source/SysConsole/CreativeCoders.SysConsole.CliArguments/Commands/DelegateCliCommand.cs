using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace CreativeCoders.SysConsole.CliArguments.Commands
{
    public class DelegateCliCommand<TOptions> : CliCommandBase<TOptions>
        where TOptions : class, new()
    {
        [SuppressMessage("ReSharper", "ConstantConditionalAccessQualifier")]
        [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "<Pending>")]
        public override Task<CliCommandResult> ExecuteAsync(TOptions options) =>
            OnExecuteAsync?.Invoke(options) ?? throw new NotImplementedException();

        public override string Name { get; set; } = string.Empty;

        public Func<TOptions, Task<CliCommandResult>> OnExecuteAsync { get; set; } =
            _ => throw new NotImplementedException();
    }
}
