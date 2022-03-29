using System;
using System.Windows.Input;
using CreativeCoders.Mvvm.Commands;
using JetBrains.Annotations;

namespace CreativeCoders.Mvvm;

[PublicAPI]
public class ActionViewModel : ViewModelBase
{
    private ICommand _command;

    private string _caption;

    private bool _isVisible = true;

    private KeyGesture _shortCut;

    private string _toolTip;

    private string _smallIcon;

    private string _largeIcon;

    private bool? _isChecked;

    public ActionViewModel() { }

    public ActionViewModel(Action executeAction) : this(executeAction, () => true) { }

    public ActionViewModel(Action executeAction, Func<bool> canExecuteAction) : this(
        new SimpleRelayCommand(executeAction, canExecuteAction)) { }

    public ActionViewModel(ICommand command)
    {
        Command = command;
    }

    public void Execute()
    {
        Command?.Execute(null);
    }

    public ICommand Command
    {
        get => _command;
        set => Set(ref _command, value);
    }

    public string Caption
    {
        get => _caption;
        set => Set(ref _caption, value);
    }

    public bool IsVisible
    {
        get => _isVisible;
        set => Set(ref _isVisible, value);
    }

    public KeyGesture ShortCut
    {
        get => _shortCut;
        set => Set(ref _shortCut, value);
    }

    public string ToolTip
    {
        get => _toolTip;
        set => Set(ref _toolTip, value);
    }

    public string SmallIcon
    {
        get => _smallIcon;
        set => Set(ref _smallIcon, value);
    }

    public string LargeIcon
    {
        get => _largeIcon;
        set => Set(ref _largeIcon, value);
    }

    public bool? IsChecked
    {
        get => _isChecked;
        set => Set(ref _isChecked, value);
    }
}
