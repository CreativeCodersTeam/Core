namespace CreativeCoders.ProcessUtils.Execution;

public class ProcessExecutorInfo<T>(string fileName, string[] arguments, IProcessOutputParser<T> outputParser)
    : ProcessExecutorInfo(fileName, arguments)
{
    public IProcessOutputParser<T> OutputParser { get; set; } = outputParser;
}

public class ProcessExecutorInfo(string fileName, string[] arguments)
{
    public string FileName { get; } = fileName;

    public string[] Arguments { get; } = arguments;

    public bool RedirectStandardOutput { get; set; } = true;

    public bool RedirectStandardError { get; set; } = true;

    public bool RedirectStandardInput { get; set; } = true;
}
