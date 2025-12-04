using CreativeCoders.Core.Collections;
using CreativeCoders.DependencyInjection;
using CreativeCoders.ProcessUtils;
using CreativeCoders.ProcessUtils.Execution;
using CreativeCoders.ProcessUtils.Execution.Parsers;
using Microsoft.Extensions.DependencyInjection;

namespace ProcessUtilsSampleApp;

internal static class Program
{
    private static void Main(string[] args)
    {
        var services = new ServiceCollection() as IServiceCollection;

        services.AddObjectFactory();

        services.AddProcessUtils();

        var sp = services.BuildServiceProvider();

        var factory = sp.GetRequiredService<IObjectFactory>();

        var builder = factory.GetInstance<IProcessExecutorBuilder<string[]>>();

        var executor = builder
            .SetFileName("defaults")
            .SetArguments(["domains"])
            .SetOutputParser<SplitLinesOutputParser>(x =>
            {
                x.SplitOptions = StringSplitOptions.RemoveEmptyEntries;
                x.Separators = [","];
                x.TrimLines = true;
            })
            .Build();

        var lines = executor.Execute();

        lines?.Order().ForEach(x => Console.WriteLine(x));

        Console.ReadLine();
    }
}
