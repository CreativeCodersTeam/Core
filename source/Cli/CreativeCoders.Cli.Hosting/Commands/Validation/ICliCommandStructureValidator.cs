using CreativeCoders.Cli.Hosting.Commands.Store;

namespace CreativeCoders.Cli.Hosting.Commands.Validation;

public interface ICliCommandStructureValidator
{
    void Validate(ICliCommandStore commandStore);
}
