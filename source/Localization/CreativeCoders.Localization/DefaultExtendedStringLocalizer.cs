using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Localization;

namespace CreativeCoders.Localization
{
    internal class DefaultExtendedStringLocalizer<T> : IExtendedStringLocalizer<T>
    {
        private readonly IStringLocalizer _globalLocalizer;

        private readonly IStringLocalizer<T> _localizer;

        public DefaultExtendedStringLocalizer(IStringLocalizer globalLocalizer, IStringLocalizer<T> localizer)
        {
            _globalLocalizer = globalLocalizer;
            _localizer = localizer;
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            var globalStrings = _globalLocalizer.GetAllStrings(includeParentCultures);

            var strings = _localizer.GetAllStrings(includeParentCultures);

            return MergeStrings(globalStrings, strings);
        }

        private static IEnumerable<LocalizedString> MergeStrings(IEnumerable<LocalizedString> globalStrings, IEnumerable<LocalizedString> strings)
        {
            var mergedStrings = new List<LocalizedString>(globalStrings);

            foreach (var localizedString in strings)
            {
                var globalLocalizedString = mergedStrings.FirstOrDefault(x => x.Name == localizedString.Name);

                if (globalLocalizedString == null)
                {
                    mergedStrings.Add(localizedString);
                }
                else if (globalLocalizedString.ResourceNotFound || !localizedString.ResourceNotFound)
                {
                    mergedStrings.Remove(globalLocalizedString);

                    mergedStrings.Add(localizedString);
                }
            }

            return mergedStrings;
        }

        public LocalizedString this[string name]
        {
            get
            {
                var text = _localizer[name];

                if (text.ResourceNotFound)
                {
                    text = _globalLocalizer[name];
                }

                return text;
            }
        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                var text = _localizer[name, arguments];

                if (text.ResourceNotFound)
                {
                    text = _globalLocalizer[name, arguments];
                }

                return text;
            }
        }
    }
}
