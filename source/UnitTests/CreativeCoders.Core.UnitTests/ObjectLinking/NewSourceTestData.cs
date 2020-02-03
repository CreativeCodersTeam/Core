using CreativeCoders.Core.ObjectLinking;
using CreativeCoders.Core.ObjectLinking.Converters;

namespace CreativeCoders.Core.UnitTests.ObjectLinking
{
    public class NewSourceTestData : SourceTestData
    {
        private bool? _isChecked;

        private bool? _isVisible;

        private bool? _boolWithDefault;

        private string _initialText;

        [PropertyLink(typeof(NewTargetTestData), nameof(NewTargetTestData.IsChecked), Direction = LinkDirection.TwoWay,
            Converter = typeof(NullableSourcePropertyConverter<bool>))]
        public bool? IsChecked
        {
            get => _isChecked;
            set => Set(ref _isChecked, value);
        }

        public bool? IsVisible
        {
            get => _isVisible;
            set => Set(ref _isVisible, value);
        }

        public bool? BoolWithDefault
        {
            get => _boolWithDefault;
            set => Set(ref _boolWithDefault, value);
        }

        [PropertyLink(typeof(NewTargetTestData), nameof(NewTargetTestData.InitialText),
            Direction = LinkDirection.TwoWay, InitWithTargetValue = true)]
        public string InitialText
        {
            get => _initialText;
            set => Set(ref _initialText, value);
        }
    }
}