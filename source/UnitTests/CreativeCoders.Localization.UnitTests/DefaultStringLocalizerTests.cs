using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Xunit;

namespace CreativeCoders.Localization.UnitTests
{
    public class DefaultStringLocalizerTests
    {
        [Fact]
        public void GetAllStrings_WithTestResource_ReturnsStringsFromResource()
        {
            var sp = CreateProviderWithLocalization();

            var localizer = sp.GetRequiredService<IStringLocalizer>();

            // Act
            var strings = localizer.GetAllStrings(false).ToArray();

            // Assert
            strings
                .Should()
                .HaveCount(2);

            strings.First().Name
                .Should()
                .Be("TextWithArgs");

            strings.Last().Name
                .Should()
                .Be("Text");
        }

        [Fact]
        public void Indexer_WithExistingName_ReturnsTestText()
        {
            var sp = CreateProviderWithLocalization();

            var localizer = sp.GetRequiredService<IStringLocalizer>();

            // Act
            var text = localizer["Text"];

            // Assert
            text.ResourceNotFound
                .Should()
                .BeFalse();

            text.Name
                .Should()
                .Be("Text");

            text.Value
                .Should()
                .Be("Test Text");
        }

        [Fact]
        public void Indexer_WithArgument_ReturnsTestText()
        {
            var sp = CreateProviderWithLocalization();

            var localizer = sp.GetRequiredService<IStringLocalizer>();

            // Act
            var text = localizer["TextWithArgs", "1234"];

            // Assert
            text.ResourceNotFound
                .Should()
                .BeFalse();

            text.Name
                .Should()
                .Be("TextWithArgs");

            text.Value
                .Should()
                .Be("Test 1234 Text");
        }

        private static IServiceProvider CreateProviderWithLocalization()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

            var services = new ServiceCollection();

            services.AddLogging();

            services.SetupLocalization("Resources", x =>
            {
                x.Assembly = typeof(DefaultStringLocalizerTests).Assembly;
                x.ResourceName = "TestResources";
            });

            return services.BuildServiceProvider();
        }
    }
}
