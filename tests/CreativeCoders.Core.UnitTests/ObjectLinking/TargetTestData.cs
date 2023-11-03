using CreativeCoders.Core.ObjectLinking;
using CreativeCoders.Core.ObjectLinking.Converters;
using JetBrains.Annotations;

namespace CreativeCoders.Core.UnitTests.ObjectLinking;

[PublicAPI]
public class TargetTestData : ObservableObject
{
    private string _targetText;

    private string _targetName;

    private string _fromSourceName;

    private string _twoWayProperty;

    private string _stringValue;

    private double _doubleValue;

    private string _secondStringValue;
    private string _fromSourceNameTwoWay;

    public string TargetText
    {
        get => _targetText;
        set => Set(ref _targetText, value);
    }

    [PropertyLink(typeof(SourceTestData), nameof(SourceTestData.SourceName))]
    public string TargetName
    {
        get => _targetName;
        set => Set(ref _targetName, value);
    }

    [PropertyLink(typeof(SourceTestData), nameof(SourceTestData.SourceName),
        Direction = LinkDirection.OneWayFromTarget)]
    public string FromSourceName
    {
        get => _fromSourceName;
        set => Set(ref _fromSourceName, value);
    }

    [PropertyLink(typeof(SourceTestData), nameof(SourceTestData.SourceName),
        Direction = LinkDirection.TwoWay)]
    public string FromSourceNameTwoWay
    {
        get => _fromSourceNameTwoWay;
        set => Set(ref _fromSourceNameTwoWay, value);
    }

    public string TwoWayProperty
    {
        get => _twoWayProperty;
        set => Set(ref _twoWayProperty, value);
    }

    [PropertyLink(typeof(SourceTestData), nameof(SourceTestData.IntValue), Direction = LinkDirection.TwoWay,
        Converter = typeof(StringSourcePropertyConverter<int>))]
    public string StringValue
    {
        get => _stringValue;
        set => Set(ref _stringValue, value);
    }

    [PropertyLink(typeof(SourceTestData), nameof(SourceTestData.IntValue),
        Direction = LinkDirection.OneWayToTarget,
        Converter = typeof(StringSourcePropertyConverter<int>))]
    public double DoubleValue
    {
        get => _doubleValue;
        set => Set(ref _doubleValue, value);
    }

    [PropertyLink(typeof(SourceTestData), nameof(SourceTestData.IntValue), Direction = LinkDirection.TwoWay,
        Converter = typeof(StringSourcePropertyConverter<int>), ConverterParameter = 9876)]
    public string SecondStringValue
    {
        get => _secondStringValue;
        set => Set(ref _secondStringValue, value);
    }
}
