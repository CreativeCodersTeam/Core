using CreativeCoders.Core;
using CreativeCoders.ProcessUtils.Execution.Parsers;

namespace CreativeCoders.ProcessUtils.Execution;

public class ProcessExecutorBuilder(IProcessFactory processFactory)
    : ProcessExecutorBuilderBase, IProcessExecutorBuilder
{
    private readonly IProcessFactory _processFactory = Ensure.NotNull(processFactory);

    public IProcessExecutorBuilder SetFileName(string fileName)
    {
        FileName = fileName;

        return this;
    }

    public IProcessExecutorBuilder SetArguments(string[] arguments, bool usePlaceholderVars = false)
    {
        Arguments = arguments;
        UsePlaceholderVars = usePlaceholderVars;

        return this;
    }

    public IProcessExecutor Build()
    {
        if (string.IsNullOrWhiteSpace(FileName))
        {
            throw new InvalidOperationException("FileName must be set before building the executor.");
        }

        var executorInfo = new ProcessExecutorInfo(
            FileName,
            Arguments ?? [],
            UsePlaceholderVars);

        return new ProcessExecutor(executorInfo, _processFactory);
    }
}

public class ProcessExecutorBuilder<T>(IProcessFactory processFactory)
    : ProcessExecutorBuilderBase, IProcessExecutorBuilder<T>
{
    private IProcessOutputParser<T>? _outputParser;

    private readonly IProcessFactory _processFactory = Ensure.NotNull(processFactory);

    public IProcessExecutorBuilder<T> SetFileName(string fileName)
    {
        FileName = fileName;

        return this;
    }

    public IProcessExecutorBuilder<T> SetArguments(string[] arguments, bool usePlaceholderVars = false)
    {
        Arguments = arguments;
        UsePlaceholderVars = usePlaceholderVars;

        return this;
    }

    public IProcessExecutorBuilder<T> SetOutputParser(IProcessOutputParser<T> parser)
    {
        _outputParser = parser;

        return this;
    }

    public IProcessExecutorBuilder<T> SetOutputParser<TParser>(Action<TParser>? configure = null)
        where TParser : IProcessOutputParser<T>, new()
    {
        var parser = new TParser();

        configure?.Invoke(parser);

        _outputParser = parser;

        return this;
    }

    private void ReturnOutputAsText()
    {
        if (typeof(T) != typeof(string))
        {
            throw new InvalidOperationException("OutputParser must be set to return output as text.");
        }

        _outputParser = (IProcessOutputParser<T>)new PassThroughProcessOutputParser();
    }

    public IProcessExecutor<T> Build()
    {
        if (string.IsNullOrWhiteSpace(FileName))
        {
            throw new InvalidOperationException("FileName must be set before building the executor.");
        }

        if (_outputParser == null && typeof(T) == typeof(string))
        {
            ReturnOutputAsText();
        }

        if (_outputParser == null)
        {
            throw new InvalidOperationException("OutputParser must be set before building the executor.");
        }

        var executorInfo = new ProcessExecutorInfo<T>(
            FileName,
            Arguments ?? [],
            UsePlaceholderVars,
            _outputParser ??
            throw new InvalidOperationException("OutputParser must be set before building the executor."));

        return new ProcessExecutor<T>(executorInfo, _processFactory);
    }
}
