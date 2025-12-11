using CreativeCoders.Cli.Hosting.Commands.Store;

namespace CreativeCoders.Cli.Hosting.Commands.Validation;

public interface ICommandStructureValidator
{
    void Validate(ICliCommandStore commandStore);
}
