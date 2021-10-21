namespace CreativeCoders.SysConsole.App.VerbObjects
{
    internal class ExecutionResult
    {
        public ExecutionResult(bool hasBeenExecuted, int returnCode)
        {
            HasBeenExecuted = hasBeenExecuted;
            ReturnCode = returnCode;
        }

        public bool HasBeenExecuted { get; }

        public int ReturnCode { get; }
    }
}
