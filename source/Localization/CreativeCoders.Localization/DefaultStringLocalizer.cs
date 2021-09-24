using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace CreativeCoders.Localization
{
    internal class DefaultStringLocalizer : IStringLocalizer
    {
        private readonly IStringLocalizer _localizer;

        public DefaultStringLocalizer(IStringLocalizerFactory factory, IOptions<ExtendedLocalizationOptions> options)
        {
            var location = new AssemblyName(
                               options.Value.Assembly.FullName ??
                               throw new ArgumentException("This assembly name not found")).Name ??
                           throw new ArgumentException("This assembly name not found");

            _localizer = factory.Create(options.Value.ResourceName, location);
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
            => _localizer.GetAllStrings(includeParentCultures);

        public LocalizedString this[string name] => _localizer[name];

        public LocalizedString this[string name, params object[] arguments] => _localizer[name, arguments];
    }
}
