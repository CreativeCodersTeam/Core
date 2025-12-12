using CreativeCoders.Cli.Core;
using CreativeCoders.Core;
using CreativeCoders.Core.Collections;

namespace CreativeCoders.Cli.Hosting.Commands.Store;

public class CliCommandStore : ICliCommandStore
{
    private readonly List<CliCommandInfo> _commands = [];

    private readonly List<CliTreeNode> _treeRootNodes = [];

    private IEnumerable<CliCommandGroupAttribute>? _groupAttributes;

    public void AddCommands(IEnumerable<CliCommandInfo> commands,
        IEnumerable<CliCommandGroupAttribute>? groupAttributes = null)
    {
        _groupAttributes = groupAttributes;

        _commands.AddRange(Ensure.NotNull(commands));

        commands.ForEach(AddCommand);
    }

    public FindCommandNodeResult<CliCommandGroupNode>? FindCommandGroupNode(string[] args)
    {
        return FindCommandGroupNode(null, _treeRootNodes, args);
    }

    private static FindCommandNodeResult<CliCommandGroupNode>? FindCommandGroupNode(
        CliCommandGroupNode? lastGroupNode,
        IEnumerable<CliTreeNode> nodes, string[] args)
    {
        if (args.Length == 0)
        {
            return lastGroupNode == null
                ? null
                : new FindCommandNodeResult<CliCommandGroupNode>(lastGroupNode, []);
        }

        var node = nodes.FirstOrDefault(x => x.Name == args[0]);

        var remainingArgs = args.Skip(1).ToArray();

        return node switch
        {
            null => lastGroupNode == null
                ? null
                : new FindCommandNodeResult<CliCommandGroupNode>(lastGroupNode, args),
            CliCommandNode commandNode => new FindCommandNodeResult<CliCommandGroupNode>(commandNode.Parent,
                args),
            CliCommandGroupNode groupNode => FindCommandGroupNode(groupNode, groupNode.ChildNodes,
                remainingArgs),
            _ => null
        };
    }

    public FindCommandNodeResult<CliCommandNode>? FindCommandNode(string[] args)
    {
        return FindCommandNode(_treeRootNodes, args);
    }

    private static FindCommandNodeResult<CliCommandNode>? FindCommandNode(IEnumerable<CliTreeNode> nodes,
        string[] args)
    {
        if (args.Length == 0)
        {
            return null;
        }

        var node = nodes.FirstOrDefault(x => x.Name == args[0]);

        return node switch
        {
            null => null,
            CliCommandNode commandNode => new FindCommandNodeResult<CliCommandNode>(commandNode,
                args.Skip(1).ToArray()),
            CliCommandGroupNode groupNode => FindCommandNode(groupNode.ChildNodes,
                args.Skip(1).ToArray()),
            _ => null
        };
    }

    private void AddCommand(CliCommandInfo command)
    {
        AddCommand(command, command.CommandAttribute.Commands);

        if (command.CommandAttribute.AlternativeCommands.Length != 0)
        {
            AddCommand(command, command.CommandAttribute.AlternativeCommands);
        }
    }

    private void AddCommand(CliCommandInfo command, string[] commands)
    {
        switch (commands.Length)
        {
            case 0:
                throw new InvalidOperationException("Commands must not be empty");
            case 1:
                _treeRootNodes.Add(new CliCommandNode(command, commands[0], null));
                break;
            default:
            {
                var groupNode = GetGroupNode(commands.Take(commands.Length - 1).ToArray());

                if (groupNode.Parent == null)
                {
                    _treeRootNodes.Add(groupNode);
                }

                groupNode.ChildNodes.Add(new CliCommandNode(command, commands[^1], groupNode));

                break;
            }
        }
    }

    private CliCommandGroupNode GetGroupNode(string[] commands)
    {
        return GetGroupNode(null, _treeRootNodes, commands);
    }

    private CliCommandGroupNode GetGroupNode(CliCommandGroupNode? parent, List<CliTreeNode> nodes,
        string[] cmds)
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
            return GetGroupNode(childNode, childNode.ChildNodes, cmds.Skip(1).ToArray());
        }

        var groupNode = new CliCommandGroupNode(cmds[0], parent);

        groupNode.GroupAttribute = _groupAttributes?
            .FirstOrDefault(x => x.Commands.SequenceEqual(groupNode.GetNamePath()));

        nodes.Add(groupNode);

        return cmds.Length == 1
            ? groupNode
            : GetGroupNode(groupNode, groupNode.ChildNodes, cmds.Skip(1).ToArray());
    }

    public IEnumerable<CliTreeNode> TreeRootNodes => _treeRootNodes;

    public IEnumerable<CliCommandInfo> Commands => _commands;

    public IEnumerable<CliCommandGroupAttribute> GroupAttributes => _groupAttributes ?? [];
}
