namespace CreativeCoders.SysConsole.CliArguments.Commands;

public class CliCommandResult
{
    public CliCommandResult(int returnCode)
    {
        ReturnCode = returnCode;
    }

    public int ReturnCode { get; }
}