namespace CreativeCoders.SysConsole.Cli.Actions
{
    public class CliActionResult
    {
        public CliActionResult()
        {
        }

        public CliActionResult(int returnCode)
        {
            ReturnCode = returnCode;
        }

        public int ReturnCode { get; init; }
    }
}
