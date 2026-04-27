using System.Diagnostics.CodeAnalysis;
using AwesomeAssertions;
using CreativeCoders.Cli.Commands.Confirmation;
using CreativeCoders.Cli.Commands.Crud;
using CreativeCoders.Cli.Commands.Crud.Convenience;
using CreativeCoders.Cli.Commands.Interaction;
using CreativeCoders.Cli.Commands.Output;
using CreativeCoders.Cli.Commands.UnitTests.TestSupport;
using FakeItEasy;
using Spectre.Console;
using Xunit;

namespace CreativeCoders.Cli.Commands.UnitTests.Crud;

public sealed record User(int Id, string Name);

public class RepositoryCrudTests
{
    [SuppressMessage("csharpsquid", "S2094", Justification = "Test code")]
    private sealed class ListUsersOptions : ListOptions;

    private sealed class ListUsersCommand(
        ICrudRepository<User, int> repo,
        IConfirmationPrompt c,
        IInteractivePrompter p,
        IAnsiConsole console,
        IEnumerable<IOutputFormatter<IReadOnlyList<User>>> formatters)
        : RepositoryListCommandBase<User, int, ListUsersOptions>(repo, c, p, console, formatters);

    private sealed class GetUserCommand(
        ICrudRepository<User, int> repo,
        IConfirmationPrompt c,
        IInteractivePrompter p,
        IAnsiConsole console,
        IEnumerable<IOutputFormatter<User?>> formatters)
        : RepositoryGetCommandBase<User, int, KeyOnlyOptions>(repo, c, p, console, formatters);

    private sealed class KeyOnlyOptions : IKeyedOptions<int>
    {
        public int Key { get; set; }
    }

    private sealed class RemoveUserCommand(
        ICrudRepository<User, int> repo,
        IConfirmationPrompt c,
        IInteractivePrompter p,
        IAnsiConsole console)
        : RepositoryRemoveCommandBase<User, int, RemoveOptions<int>>(repo, c, p, console);

    [Fact]
    public async Task List_DelegatesToRepositoryListAsync()
    {
        var repo = A.Fake<ICrudRepository<User, int>>();
        A.CallTo(() => repo.ListAsync(A<CancellationToken>._))
            .Returns(Task.FromResult<IReadOnlyList<User>>(new[] { new User(1, "alice") }));

        var (confirm, prompter, console) = TestCommandFactory.CreateDeps();
        var sut = new ListUsersCommand(repo, confirm, prompter, console,
            TestCommandFactory.EmptyFormatters<IReadOnlyList<User>>());

        var result = await sut.ExecuteAsync(new ListUsersOptions());

        result.ExitCode.Should().Be(0);
        A.CallTo(() => repo.ListAsync(A<CancellationToken>._)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task Get_WhenFound_ReturnsSuccess()
    {
        var repo = A.Fake<ICrudRepository<User, int>>();
        A.CallTo(() => repo.GetAsync(1, A<CancellationToken>._))
            .Returns(Task.FromResult<User?>(new User(1, "alice")));
        var (confirm, prompter, console) = TestCommandFactory.CreateDeps();
        var sut = new GetUserCommand(repo, confirm, prompter, console,
            TestCommandFactory.EmptyFormatters<User?>());

        var result = await sut.ExecuteAsync(new KeyOnlyOptions { Key = 1 });

        result.ExitCode.Should().Be(0);
    }

    [Fact]
    public async Task Get_WhenNotFound_ReturnsNotFoundExitCode()
    {
        var repo = A.Fake<ICrudRepository<User, int>>();
        A.CallTo(() => repo.GetAsync(99, A<CancellationToken>._))
            .Returns(Task.FromResult<User?>(null));
        var (confirm, prompter, console) = TestCommandFactory.CreateDeps();
        var sut = new GetUserCommand(repo, confirm, prompter, console,
            TestCommandFactory.EmptyFormatters<User?>());

        var result = await sut.ExecuteAsync(new KeyOnlyOptions { Key = 99 });

        result.ExitCode.Should().Be(ExitCodes.NotFound);
    }

    [Fact]
    public async Task Remove_WithoutYes_PromptsForConfirmation_AndDeclineCancelsRemoval()
    {
        var repo = A.Fake<ICrudRepository<User, int>>();
        var (confirm, prompter, console) = TestCommandFactory.CreateDeps();
        A.CallTo(() => confirm.ConfirmAsync(A<string>._, A<bool>._, A<CancellationToken>._))
            .Returns(Task.FromResult(false));

        var sut = new RemoveUserCommand(repo, confirm, prompter, console);

        var result = await sut.ExecuteAsync(new RemoveOptions<int> { Key = 7 });

        result.ExitCode.Should().Be(ExitCodes.Cancelled);
        A.CallTo(() => repo.RemoveAsync(A<int>._, A<CancellationToken>._)).MustNotHaveHappened();
    }

    [Fact]
    public async Task Remove_WithYes_CallsRepositoryRemoveAsync()
    {
        var repo = A.Fake<ICrudRepository<User, int>>();
        var (confirm, prompter, console) = TestCommandFactory.CreateDeps();
        var sut = new RemoveUserCommand(repo, confirm, prompter, console);

        var result = await sut.ExecuteAsync(new RemoveOptions<int> { Key = 7, Yes = true });

        result.ExitCode.Should().Be(0);
        A.CallTo(() => repo.RemoveAsync(7, A<CancellationToken>._)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task Remove_WithDryRun_DoesNotCallRepository()
    {
        var repo = A.Fake<ICrudRepository<User, int>>();
        var (confirm, prompter, console) = TestCommandFactory.CreateDeps();
        var sut = new RemoveUserCommand(repo, confirm, prompter, console);

        var result = await sut.ExecuteAsync(new RemoveOptions<int>
        {
            Key = 7,
            Yes = true,
            DryRun = true
        });

        result.ExitCode.Should().Be(0);
        A.CallTo(() => repo.RemoveAsync(A<int>._, A<CancellationToken>._)).MustNotHaveHappened();
    }
}
