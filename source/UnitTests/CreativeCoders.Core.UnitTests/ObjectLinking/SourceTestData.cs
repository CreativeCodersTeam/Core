using CreativeCoders.Core.ObjectLinking;

namespace CreativeCoders.Core.UnitTests.ObjectLinking;

public class SourceTestData : ObservableObject
{
    private string _sourceText;

    private string _sourceName;

    private string _twoWayProperty;
    private int _intValue;

    private int _secondIntValue;

    [PropertyLink(typeof(TargetTestData), nameof(TargetTestData.TargetText))]
    public string SourceText
    {
        get => _sourceText;
        set => Set(ref _sourceText, value);
    }

    public string SourceName
    {
        get => _sourceName;
        set => Set(ref _sourceName, value);
    }

    [PropertyLink(typeof(TargetTestData), nameof(TargetTestData.TwoWayProperty),
        Direction = LinkDirection.TwoWay)]
    public string TwoWayProperty
    {
        get => _twoWayProperty;
        set => Set(ref _twoWayProperty, value);
    }


    public int IntValue
    {
        get => _intValue;
        set => Set(ref _intValue, value);
    }

    public int SecondIntValue
    {
        get => _secondIntValue;
        set => Set(ref _secondIntValue, value);
    }
}
