using CreativeCoders.Core;
using CreativeCoders.Core.Collections;

namespace CreativeCoders.Cli.Hosting.Commands.Store;

public class CliCommandStore
{
    private readonly IEnumerable<CliCommandInfo> _commands;

    private readonly List<CliTreeNode> _treeRootNodes = [];

    public CliCommandStore(IEnumerable<CliCommandInfo> commands)
    {
        _commands = Ensure.NotNull(commands);

        BuildTree();
    }

    private void BuildTree()
    {
        _commands.ForEach(AddCommand);
    }

    private void AddCommand(CliCommandInfo command)
    {
        AddCommand(command, command.CommandAttribute.Commands);

        command.CommandAttribute.AlternativeCommands
            .ForEach(altCommand => AddCommand(command, altCommand));
    }

    private void AddCommand(CliCommandInfo command, string[] commands)
    {
        switch (commands.Length)
        {
            case 0:
                throw new InvalidOperationException("Commands must not be empty");
            case 1:
                _treeRootNodes.Add(new CliCommandNode(command, commands[0]));
                break;
            default:
            {
                var groupNode = GetGroupNode(commands.Take(commands.Length - 1).ToArray());

                if (groupNode == null)
                {
                    throw new InvalidOperationException("Command group node could not be created");
                }

                groupNode?.ChildNodes.Add(new CliCommandNode(command, commands[^1]));

                break;
            }
        }
    }

    private CliCommandGroupNode? GetGroupNode(string[] commands)
    {
        return GetGroupNode(_treeRootNodes, commands);
    }

    private CliCommandGroupNode? GetGroupNode(IEnumerable<CliTreeNode> nodes, string[] cmds)
    {
        var childNode = nodes
            .OfType<CliCommandGroupNode>()
            .FirstOrDefault(x => x.Name == cmds[0]);

        return childNode != null
            ? GetGroupNode(childNode.GetSubCommandGroups(), cmds.Skip(1).ToArray())
            : null;
    }

    public IEnumerable<CliTreeNode> TreeRootNodes => _treeRootNodes;

    public IEnumerable<CliCommandInfo> Commands => _commands;
}
