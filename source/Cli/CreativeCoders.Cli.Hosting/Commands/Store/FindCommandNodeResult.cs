using CreativeCoders.Core;

namespace CreativeCoders.Cli.Hosting.Commands.Store;

public class FindCommandNodeResult<TNode>(TNode? node, string[] remainingArgs)
    where TNode : CliTreeNode
{
    public TNode? Node { get; } = node;

    public string[] RemainingArgs { get; } = Ensure.NotNull(remainingArgs);
}
