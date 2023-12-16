using CreativeCoders.Mvvm;
using CreativeCoders.Mvvm.Commands;
using CreativeCoders.Mvvm.FileDialogService;
using JetBrains.Annotations;

namespace SkeletorSampleApp.ViewModels;

[UsedImplicitly]
public class MainViewModel : ViewModelBase
{
    private string _title;
        
    private object _details;
        
    private readonly FirstDetailsViewModel _firstDetails;
        
    private readonly SecondDetailsViewModel _secondDetails;

    public MainViewModel(IFileDialogService fileDialogService)
    {
        ShowFirstDetailsCommand = new SimpleRelayCommand(ExecuteShowFirstDetails, CanShowFirstDetails);
        ShowSecondDetailsCommand = new SimpleRelayCommand(ExecuteShowSecondDetails, CanShowSecondDetails);
        _firstDetails = new FirstDetailsViewModel(fileDialogService);
        _secondDetails = new SecondDetailsViewModel();
    }

    private bool CanShowSecondDetails()
    {
        return Details != _secondDetails;
    }

    private void ExecuteShowSecondDetails()
    {
        Details = _secondDetails;
    }

    private bool CanShowFirstDetails()
    {
        return Details != _firstDetails;
    }

    private void ExecuteShowFirstDetails()
    {
        Details = _firstDetails;
    }

    public string Title
    {
        get => _title;
        set => Set(ref _title, value);
    }

    public ICommandEx ShowFirstDetailsCommand { get; }

    public ICommandEx ShowSecondDetailsCommand { get; }

    [UsedImplicitly]
    public object Details
    {
        get => _details;
        set => Set(ref _details, value);
    }
}
