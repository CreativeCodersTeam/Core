using System.Diagnostics;
using JetBrains.Annotations;

namespace CreativeCoders.ProcessUtils;

[PublicAPI]
public interface IProcess : IDisposable
{
    bool Start();

    void Close();

    bool CloseMainWindow();

    void Refresh();

    void Kill();

    void WaitForExit();

    void WaitForExit(TimeSpan timeout);

    Task WaitForExitAsync(CancellationToken cancellationToken = default(CancellationToken));

    void WaitForInputIdle();

    int ExitCode { get; }

    int Id { get; }

    bool HasExited { get; }

    string MainWindowTitle { get; }

    string ProcessName { get; }

    bool Responding { get; }

    StreamReader StandardOutput { get; }

    StreamReader StandardError { get; }

    StreamWriter StandardInput { get; }

    ProcessStartInfo StartInfo { get; }
}
