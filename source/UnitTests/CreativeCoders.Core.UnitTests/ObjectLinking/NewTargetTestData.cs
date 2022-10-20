using CreativeCoders.Core.ObjectLinking;
using CreativeCoders.Core.ObjectLinking.Converters;
using JetBrains.Annotations;

namespace CreativeCoders.Core.UnitTests.ObjectLinking;

public class NewTargetTestData : TargetTestData
{
    private bool _isChecked;

    private bool _isVisible;

    private bool _boolWithDefault;

    private string _initialText;

    public bool IsChecked
    {
        get => _isChecked;
        set => Set(ref _isChecked, value);
    }

    [PropertyLink(typeof(NewSourceTestData), nameof(NewSourceTestData.IsVisible),
        Direction = LinkDirection.TwoWay,
        Converter = typeof(NullableTargetPropertyConverter<bool>))]
    public bool IsVisible
    {
        get => _isVisible;
        set => Set(ref _isVisible, value);
    }

    [UsedImplicitly]
    [PropertyLink(typeof(NewSourceTestData), nameof(NewSourceTestData.BoolWithDefault),
        Direction = LinkDirection.TwoWay,
        Converter = typeof(NullableTargetPropertyConverter<bool>), ConverterParameter = true)]
    public bool BoolWithDefault
    {
        get => _boolWithDefault;
        set => Set(ref _boolWithDefault, value);
    }

    [UsedImplicitly]
    public string InitialText
    {
        get => _initialText;
        set => Set(ref _initialText, value);
    }
}
