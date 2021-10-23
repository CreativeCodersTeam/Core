namespace CreativeCoders.SysConsole.App.Execution
{
    public class ExecutorResult
    {
        public ExecutorResult(bool executionIsHandled, int returnCode)
        {
            ExecutionIsHandled = executionIsHandled;
            ReturnCode = returnCode;
        }

        public bool ExecutionIsHandled { get; }

        public int ReturnCode { get; }
    }
}