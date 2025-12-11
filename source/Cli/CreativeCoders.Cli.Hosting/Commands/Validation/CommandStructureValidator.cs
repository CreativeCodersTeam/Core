using CreativeCoders.Cli.Hosting.Commands.Store;
using CreativeCoders.Cli.Hosting.Exceptions;

namespace CreativeCoders.Cli.Hosting.Commands.Validation;

public class CommandStructureValidator : ICommandStructureValidator
{
    public void Validate(ICliCommandStore commandStore)
    {
        // Ensure, that all Group attributes commands are unique
        var uniqueGroupAttributesCount = commandStore
            .GroupAttributes
            .Select(x => x.Commands)
            .Distinct()
            .ToArray()
            .Length;

        if (uniqueGroupAttributesCount != commandStore.GroupAttributes.Count())
        {
            throw new CliCommandGroupDuplicateException();
        }

        // Ensure that all Commands are unique
        var allCommands = commandStore
            .Commands
            .SelectMany(x =>
            {
                if (x.CommandAttribute.AlternativeCommands.Length == 0)
                {
                    return [x.CommandAttribute.Commands];
                }

                List<string[]> commandsLists =
                    [x.CommandAttribute.Commands, x.CommandAttribute.AlternativeCommands];

                return commandsLists;
            })
            .ToArray();

        var uniqueCommandsCount = allCommands
            .Distinct().ToArray().Length;

        if (uniqueCommandsCount != allCommands.Length)
        {
            throw new AmbiguousCliCommandsException();
        }
    }
}
