using AwesomeAssertions;
using CreativeCoders.Cli.Commands.Confirmation;
using CreativeCoders.Cli.Commands.Interaction;
using CreativeCoders.Cli.Commands.Options;
using CreativeCoders.Cli.Commands.UnitTests.TestSupport;
using CreativeCoders.Cli.Core;
using FakeItEasy;
using Spectre.Console;
using Spectre.Console.Testing;
using Xunit;

namespace CreativeCoders.Cli.Commands.UnitTests;

public class CliCommandBaseTests
{
    private sealed class FullOptions : IDryRunOptions, IConfirmableOptions, IVerbosityOptions
    {
        public bool DryRun { get; set; }
        public bool Yes { get; set; }
        public Verbosity Verbosity { get; set; }
    }

    private sealed class TestCommand : CliCommandBase<FullOptions>
    {
        public bool ExecuteCalled { get; private set; }
        public bool DryRunCalled { get; private set; }
        public Func<Exception>? ExceptionFactory { get; set; }

        public TestCommand(IConfirmationPrompt c, IInteractivePrompter p, IAnsiConsole console)
            : base(c, p, console) { }

        protected override Task<CommandResult> OnExecuteAsync(FullOptions options, CancellationToken ct)
        {
            ExecuteCalled = true;

            if (ExceptionFactory is not null)
            {
                throw ExceptionFactory();
            }

            return Task.FromResult(CommandResult.Success);
        }

        protected override Task OnDryRunAsync(FullOptions options, CancellationToken ct)
        {
            DryRunCalled = true;

            return Task.CompletedTask;
        }

        protected override string GetConfirmationMessage(FullOptions options)
            => "Confirm?";
    }

    [Fact]
    public async Task ExecuteAsync_WhenYes_SkipsConfirmationAndRunsExecute()
    {
        var (confirm, prompter, console) = TestCommandFactory.CreateDeps();
        var sut = new TestCommand(confirm, prompter, console);

        var result = await sut.ExecuteAsync(new FullOptions { Yes = true });

        result.ExitCode.Should().Be(0);
        sut.ExecuteCalled.Should().BeTrue();
        A.CallTo(() => confirm.ConfirmAsync(A<string>._, A<bool>._, A<CancellationToken>._))
            .MustNotHaveHappened();
    }

    [Fact]
    public async Task ExecuteAsync_WhenConfirmableAndUserConfirms_RunsExecute()
    {
        var (confirm, prompter, console) = TestCommandFactory.CreateDeps();
        A.CallTo(() => confirm.ConfirmAsync(A<string>._, A<bool>._, A<CancellationToken>._))
            .Returns(Task.FromResult(true));
        var sut = new TestCommand(confirm, prompter, console);

        var result = await sut.ExecuteAsync(new FullOptions());

        result.ExitCode.Should().Be(0);
        sut.ExecuteCalled.Should().BeTrue();
    }

    [Fact]
    public async Task ExecuteAsync_WhenUserDeclinesConfirmation_ReturnsCancelled()
    {
        var (confirm, prompter, console) = TestCommandFactory.CreateDeps();
        A.CallTo(() => confirm.ConfirmAsync(A<string>._, A<bool>._, A<CancellationToken>._))
            .Returns(Task.FromResult(false));
        var sut = new TestCommand(confirm, prompter, console);

        var result = await sut.ExecuteAsync(new FullOptions());

        result.ExitCode.Should().Be(ExitCodes.Cancelled);
        sut.ExecuteCalled.Should().BeFalse();
    }

    [Fact]
    public async Task ExecuteAsync_WhenDryRun_RunsDryRunHookAndSkipsExecute()
    {
        var (confirm, prompter, console) = TestCommandFactory.CreateDeps();
        var sut = new TestCommand(confirm, prompter, console);

        var result = await sut.ExecuteAsync(new FullOptions { Yes = true, DryRun = true });

        result.ExitCode.Should().Be(0);
        sut.DryRunCalled.Should().BeTrue();
        sut.ExecuteCalled.Should().BeFalse();
    }

    [Fact]
    public async Task ExecuteAsync_WhenExecuteThrows_ReturnsErrorExitCode()
    {
        var (confirm, prompter, console) = TestCommandFactory.CreateDeps();
        var sut = new TestCommand(confirm, prompter, console)
        {
            ExceptionFactory = () => new InvalidOperationException("boom")
        };

        var result = await sut.ExecuteAsync(new FullOptions { Yes = true });

        result.ExitCode.Should().Be(ExitCodes.Error);
    }

    [Fact]
    public async Task ExecuteAsync_WhenExecuteCancelled_ReturnsCancelledExitCode()
    {
        var (confirm, prompter, console) = TestCommandFactory.CreateDeps();
        var sut = new TestCommand(confirm, prompter, console)
        {
            ExceptionFactory = () => new OperationCanceledException()
        };

        var result = await sut.ExecuteAsync(new FullOptions { Yes = true });

        result.ExitCode.Should().Be(ExitCodes.Cancelled);
    }

    [Fact]
    public async Task ExecuteAsync_WhenOptionsImplementInteractiveAndTty_CallsPromptHook()
    {
        var (confirm, prompter, console) = TestCommandFactory.CreateDeps(isInteractive: true);
        A.CallTo(() => confirm.ConfirmAsync(A<string>._, A<bool>._, A<CancellationToken>._))
            .Returns(Task.FromResult(true));

        var sut = new InteractiveCommand(confirm, prompter, console);

        await sut.ExecuteAsync(new InteractiveOptions { Yes = true });

        sut.PromptHookCalled.Should().BeTrue();
    }

    [Fact]
    public async Task ExecuteAsync_WhenNoInteractive_SkipsPromptHookEvenOnTty()
    {
        var (confirm, prompter, console) = TestCommandFactory.CreateDeps(isInteractive: true);
        var sut = new InteractiveCommand(confirm, prompter, console);

        await sut.ExecuteAsync(new InteractiveOptions { Yes = true, NoInteractive = true });

        sut.PromptHookCalled.Should().BeFalse();
    }

    private sealed class InteractiveOptions : IInteractiveOptions, IConfirmableOptions
    {
        public bool NoInteractive { get; set; }
        public bool Yes { get; set; }
    }

    private sealed class InteractiveCommand : CliCommandBase<InteractiveOptions>
    {
        public bool PromptHookCalled { get; private set; }

        public InteractiveCommand(IConfirmationPrompt c, IInteractivePrompter p, IAnsiConsole console)
            : base(c, p, console) { }

        protected override Task<CommandResult> OnExecuteAsync(InteractiveOptions options,
            CancellationToken ct)
            => Task.FromResult(CommandResult.Success);

        protected override Task PromptMissingOptionsAsync(InteractiveOptions options,
            IInteractivePrompter prompter, CancellationToken ct)
        {
            PromptHookCalled = true;

            return Task.CompletedTask;
        }
    }
}
