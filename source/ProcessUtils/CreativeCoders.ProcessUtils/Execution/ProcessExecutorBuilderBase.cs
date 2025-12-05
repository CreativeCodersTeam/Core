using System.Diagnostics;

namespace CreativeCoders.ProcessUtils.Execution;

public abstract class ProcessExecutorBuilderBase
{
    protected string? FileName { get; set; }

    protected string[]? Arguments { get; set; }

    protected bool ThrowOnError { get; set; }

    protected Action<ProcessStartInfo>? ConfigureStartInfo { get; set; }
}
