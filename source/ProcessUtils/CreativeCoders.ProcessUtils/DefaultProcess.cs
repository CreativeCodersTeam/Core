using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using CreativeCoders.Core;

namespace CreativeCoders.ProcessUtils;

[ExcludeFromCodeCoverage]
public sealed class DefaultProcess(Process process) : IProcess
{
    private readonly Process _process = Ensure.NotNull(process);

    public bool Start() => _process.Start();

    public void Close() => _process.Close();

    public bool CloseMainWindow() => _process.CloseMainWindow();

    public void Refresh() => _process.Refresh();

    public void Kill() => _process.Kill();

    public void WaitForExit() => _process.WaitForExit();

    public void WaitForExit(TimeSpan timeout) => _process.WaitForExit(timeout);

    public Task WaitForExitAsync(CancellationToken cancellationToken = default)
        => _process.WaitForExitAsync(cancellationToken);

    public void WaitForInputIdle() => _process.WaitForInputIdle();

    public int ExitCode => _process.ExitCode;

    public int Id => _process.Id;

    public bool HasExited => _process.HasExited;

    public string MainWindowTitle => _process.MainWindowTitle;

    public string ProcessName => _process.ProcessName;

    public bool Responding => _process.Responding;

    public StreamReader StandardOutput => _process.StandardOutput;

    public StreamReader StandardError => _process.StandardError;

    public StreamWriter StandardInput => _process.StandardInput;

    public ProcessStartInfo StartInfo => _process.StartInfo;

    public void Dispose() => _process.Dispose();
}
