using System.Reflection;
using CreativeCoders.Cli.Core;
using CreativeCoders.Core.Reflection;

namespace CreativeCoders.Cli.Hosting.Commands;

public class CommandInfoCreator : ICommandInfoCreator
{
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
