using CreativeCoders.Core;
using CreativeCoders.Core.Collections;

namespace CreativeCoders.Cli.Hosting.Commands.Store;

public class CliCommandStore : ICliCommandStore
{
    private readonly List<CliCommandInfo> _commands = [];

    private readonly List<CliTreeNode> _treeRootNodes = [];

    public void AddCommands(IEnumerable<CliCommandInfo> commands)
    {
        _commands.AddRange(Ensure.NotNull(commands));

        commands.ForEach(AddCommand);
    }

    public CliCommandInfo? FindCommandForArgs(string[] args)
    {
        return FindCommandForArgs(_treeRootNodes, args);
    }

    public CliCommandGroupNode? FindGroupNodeForArgs(string[] args)
    {
        return FindGroupNodeForArgs(null, _treeRootNodes, args);
    }

    public CliCommandGroupNode? FindGroupNodeForArgs(CliCommandGroupNode? lastGroupNode,
        IEnumerable<CliTreeNode> nodes, string[] args)
    {
        if (args.Length == 0)
        {
            return null;
        }

        var node = nodes.FirstOrDefault(x => x.Name == args[0]);

        return node switch
        {
            null => lastGroupNode,
            CliCommandNode commandNode => commandNode.Parent,
            CliCommandGroupNode groupNode => FindGroupNodeForArgs(groupNode, groupNode.ChildNodes,
                args.Skip(1).ToArray()),
            _ => null
        };
    }

    private CliCommandInfo? FindCommandForArgs(IEnumerable<CliTreeNode> nodes, string[] args)
    {
        if (args.Length == 0)
        {
            return null;
        }

        var node = nodes.FirstOrDefault(x => x.Name == args[0]);

        return node switch
        {
            null => null,
            CliCommandNode commandNode => commandNode.CommandInfo,
            CliCommandGroupNode groupNode => FindCommandForArgs(groupNode.ChildNodes, args.Skip(1).ToArray()),
            _ => null
        };
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

                _treeRootNodes.Add(groupNode);

                groupNode.ChildNodes.Add(new CliCommandNode(command, commands[^1]));

                break;
            }
        }
    }

    private CliCommandGroupNode? GetGroupNode(string[] commands)
    {
        return GetGroupNode(_treeRootNodes, commands);
    }

    private static CliCommandGroupNode? GetGroupNode(List<CliTreeNode> nodes, string[] cmds)
    {
        var childNode = nodes
            .OfType<CliCommandGroupNode>()
            .FirstOrDefault(x => x.Name == cmds[0]);

        // Last group node is found
        if (cmds.Length == 1 && childNode != null)
        {
            return childNode;
        }

        if (childNode != null)
        {
            return GetGroupNode(childNode.ChildNodes, cmds.Skip(1).ToArray());
        }

        var groupNode = new CliCommandGroupNode(cmds[0]);

        nodes.Add(groupNode);

        return cmds.Length == 1
            ? groupNode
            : GetGroupNode(groupNode.ChildNodes, cmds.Skip(1).ToArray());
    }

    public IEnumerable<CliTreeNode> TreeRootNodes => _treeRootNodes;

    public IEnumerable<CliCommandInfo> Commands => _commands;
}
