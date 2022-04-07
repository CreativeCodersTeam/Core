using System.Windows.Input;
using CreativeCoders.Core.ObjectLinking;
using JetBrains.Annotations;

namespace CreativeCoders.Mvvm.Ribbon.Controls;

[PublicAPI]
public abstract class RibbonCommandControlViewModel : RibbonControlViewModel
{
    private ICommand _command;

    protected RibbonCommandControlViewModel() { }

    protected RibbonCommandControlViewModel(ActionViewModel action)
    {
        Action = action;
        ActionLink = new ObjectLinkBuilder(action, this).Build();
    }

    [PropertyLink(typeof(ActionViewModel), nameof(ActionViewModel.Command), Direction = LinkDirection.TwoWay,
        InitWithTargetValue = true)]
    public ICommand Command
    {
        get => _command;
        set => Set(ref _command, value);
    }

    protected ActionViewModel Action { get; }

    protected ObjectLink ActionLink { get; }
}
