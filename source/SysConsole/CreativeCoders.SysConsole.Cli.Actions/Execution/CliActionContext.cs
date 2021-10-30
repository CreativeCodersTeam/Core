namespace CreativeCoders.SysConsole.Cli.Actions.Execution
{
    public class CliActionContext
    {
        public CliActionContext(CliActionRequest request)
        {
            Request = request;
        }

        public CliActionRequest Request { get; }

        public int ReturnCode { get; set; }
    }
}
