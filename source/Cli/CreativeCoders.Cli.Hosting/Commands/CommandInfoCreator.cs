using System.Reflection;
using CreativeCoders.Cli.Core;
using CreativeCoders.Core.Reflection;

namespace CreativeCoders.Cli.Hosting.Commands;

/// <summary>
/// Provides the default implementation of <see cref="ICommandInfoCreator"/> that creates
/// <see cref="CliCommandInfo"/> instances by inspecting the <see cref="CliCommandAttribute"/> on command types.
/// </summary>
public class CommandInfoCreator : ICommandInfoCreator
{
    /// <inheritdoc />
    public CliCommandInfo? Create(Type commandType)
    {
        var commandAttribute = commandType.GetCustomAttribute<CliCommandAttribute>(false);

        if (commandAttribute == null)
        {
            return null;
        }

        return new CliCommandInfo
        {
            CommandAttribute = commandAttribute,
            CommandType = commandType,
            OptionsType = GetCommandOptionsType(commandType)
        };
    }

    private static Type? GetCommandOptionsType(Type commandType)
    {
        if (commandType.IsAssignableTo(typeof(ICliCommand)))
        {
            return null;
        }

        if (!commandType.ImplementsGenericInterface(typeof(ICliCommand<>)))
        {
            throw new InvalidOperationException("Invalid command type");
        }

        var optionsTypes = commandType.GetGenericInterfaceArguments(typeof(ICliCommand<>));

        return optionsTypes.Length != 1
            ? throw new InvalidOperationException("Invalid command type")
            : optionsTypes[0];
    }
}
