using CreativeCoders.Cli.Commands.Confirmation;
using CreativeCoders.Cli.Commands.Interaction;
using CreativeCoders.Cli.Commands.Output;
using FakeItEasy;
using Spectre.Console;
using Spectre.Console.Testing;

namespace CreativeCoders.Cli.Commands.UnitTests.TestSupport;

internal static class TestCommandFactory
{
    public static (IConfirmationPrompt Confirm, IInteractivePrompter Prompter, TestConsole Console)
        CreateDeps(bool isInteractive = true)
    {
        var confirm = A.Fake<IConfirmationPrompt>();
        var prompter = A.Fake<IInteractivePrompter>();
        A.CallTo(() => prompter.IsInteractive).Returns(isInteractive);
        var console = new TestConsole();

        return (confirm, prompter, console);
    }

    public static IEnumerable<IOutputFormatter<T>> EmptyFormatters<T>()
        => Array.Empty<IOutputFormatter<T>>();

    public static IEnumerable<IOutputFormatter<T>> Formatters<T>(params IOutputFormatter<T>[] formatters)
        => formatters;
}
