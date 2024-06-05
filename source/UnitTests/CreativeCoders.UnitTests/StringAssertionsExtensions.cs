using CreativeCoders.Core.IO;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace CreativeCoders.UnitTests;

public static class StringAssertionsExtensions
{
    public static AndConstraint<StringAssertions> FileExists(this StringAssertions assertions)
    {
        Execute.Assertion
            .BecauseOf(assertions.Subject)
            .ForCondition(FileSys.File.Exists(assertions.Subject))
            .FailWith("Expected file {context:file} to exist, but it does not.");

        return new AndConstraint<StringAssertions>(assertions);
    }

    public static AndConstraint<StringAssertions> DirectoryExists(this StringAssertions assertions)
    {
        Execute.Assertion
            .BecauseOf(assertions.Subject)
            .ForCondition(FileSys.Directory.Exists(assertions.Subject))
            .FailWith("Expected directory {context:directory} to exist, but it does not.");

        return new AndConstraint<StringAssertions>(assertions);
    }

    public static AndConstraint<StringAssertions> FileHasContent(this StringAssertions assertions,
        string content)
    {
        Execute.Assertion
            .BecauseOf(assertions.Subject)
            .ForCondition(FileSys.File.Exists(assertions.Subject))
            .FailWith("Expected file {context:file} to exist, but it does not.");

        var fileContent = FileSys.File.ReadAllText(assertions.Subject);

        Execute.Assertion
            .BecauseOf(fileContent)
            .ForCondition(fileContent == content)
            .FailWith("Expected file {context:file} to have content {0}{reason}, but found {1}.", content,
                fileContent);

        return new AndConstraint<StringAssertions>(assertions);
    }
}
