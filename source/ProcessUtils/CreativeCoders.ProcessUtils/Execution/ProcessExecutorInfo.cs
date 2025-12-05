namespace CreativeCoders.ProcessUtils.Execution;

public class ProcessExecutorInfo<T>(
    string fileName,
    string[] arguments,
    bool usePlaceholderVars,
    IProcessOutputParser<T> outputParser)
    : ProcessExecutorInfo(fileName, arguments, usePlaceholderVars)
{
    public IProcessOutputParser<T> OutputParser { get; set; } = outputParser;
}

public class ProcessExecutorInfo(string fileName, string[] arguments, bool usePlaceholderVars)
{
    public string FileName { get; } = fileName;

    public string[] Arguments { get; } = arguments;

    public bool RedirectStandardOutput { get; set; } = true;

    public bool RedirectStandardError { get; set; } = true;

    public bool RedirectStandardInput { get; set; } = true;
}
