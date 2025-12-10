using System.Reflection;
using CreativeCoders.Cli.Core;

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
            CommandType = commandType
        };
    }
}