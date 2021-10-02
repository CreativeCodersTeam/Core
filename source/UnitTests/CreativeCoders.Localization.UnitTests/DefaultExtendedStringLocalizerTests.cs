using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Localization;
using Xunit;

namespace CreativeCoders.Localization.UnitTests
{
    public class DefaultExtendedStringLocalizerTests
    {
        private static readonly LocalizedString LocalizedString1 = new("Name1", "Value1", false);

        private static readonly LocalizedString LocalizedString11 = new("Name1", "Value2", false);

        private static readonly LocalizedString LocalizedString2 = new("Name2", "Value2", false);

        [Fact]
        public void GetStrings_DifferentStrings_ReturnsConcatList()
        {
            var localizer = CreateLocalizer(
                new[] { LocalizedString1 },
                new[] { LocalizedString2 },
                LocalizedString1, LocalizedString2);

            // Act
            var strings = localizer.GetAllStrings(false).ToArray();

            // Assert
            strings
                .Should()
                .HaveCount(2);

            strings.First().Name
                .Should()
                .Be(LocalizedString1.Name);

            strings.First().Value
                .Should()
                .Be(LocalizedString1.Value);

            strings.Last().Name
                .Should()
                .Be(LocalizedString2.Name);

            strings.Last().Value
                .Should()
                .Be(LocalizedString2.Value);
        }

        [Fact]
        public void GetStrings_DuplicateNames_ReturnsMergedList()
        {
            var localizer = CreateLocalizer(
                new[] { LocalizedString1, LocalizedString2 },
                new[] { LocalizedString11 },
                LocalizedString1, LocalizedString2);

            // Act
            var strings = localizer.GetAllStrings(false).ToArray();

            // Assert
            strings
                .Should()
                .HaveCount(2);

            strings.First().Name
                .Should()
                .Be(LocalizedString2.Name);

            strings.First().Value
                .Should()
                .Be(LocalizedString2.Value);

            strings.Last().Name
                .Should()
                .Be(LocalizedString1.Name);

            strings.Last().Value
                .Should()
                .Be(LocalizedString11.Value);
        }

        [Fact]
        public void GetStrings_DuplicateNamesWithResourceNotFound_ReturnsMergedList()
        {
            var localizer = CreateLocalizer(
                new[] { new LocalizedString(LocalizedString1.Name, LocalizedString1.Value, true) },
                new[] { LocalizedString11 },
                LocalizedString1, LocalizedString2);

            // Act
            var strings = localizer.GetAllStrings(false).ToArray();

            // Assert
            strings
                .Should()
                .HaveCount(1);

            strings.First().Name
                .Should()
                .Be(LocalizedString11.Name);

            strings.First().Value
                .Should()
                .Be(LocalizedString11.Value);
        }

        [Fact]
        public void Indexer_InBothLocalizer_ReturnsTypedLocalizer()
        {
            var localizer = CreateLocalizer(null, null, LocalizedString1, LocalizedString11);

            // Act
            var text = localizer[LocalizedString1.Name];

            // Assert
            text.Value
                .Should()
                .Be(LocalizedString11.Value);
        }

        [Fact]
        public void Indexer_InGlobalLocalizer_ReturnsTypedLocalizer()
        {
            var localizer = CreateLocalizer(null, null, LocalizedString1, LocalizedString11);

            // Act
            var text = localizer[LocalizedString1.Name];

            // Assert
            text.Value
                .Should()
                .Be(LocalizedString11.Value);
        }

        [Fact]
        public void Indexer_InGlobalLocalizerOnly_ReturnsResultFromGlobalLocalizer()
        {
            var localizer = CreateLocalizer(null, null, LocalizedString1,
                new LocalizedString(LocalizedString2.Name, LocalizedString2.Value, true));

            // Act
            var text = localizer[LocalizedString1.Name];

            // Assert
            text.Value
                .Should()
                .Be(LocalizedString1.Value);
        }

        [Fact]
        public void IndexerWithArgs_InGlobalLocalizer_ReturnsTypedLocalizer()
        {
            var localizer = CreateLocalizer(null, null, LocalizedString1, LocalizedString11);

            // Act
            var text = localizer[LocalizedString1.Name, "1234"];

            // Assert
            text.Value
                .Should()
                .Be(LocalizedString11.Value);
        }

        [Fact]
        public void IndexerWithArgs_InGlobalLocalizerOnly_ReturnsResultFromGlobalLocalizer()
        {
            var localizer = CreateLocalizer(null, null, LocalizedString1,
                new LocalizedString(LocalizedString2.Name, LocalizedString2.Value, true));

            // Act
            var text = localizer[LocalizedString1.Name, "1234"];

            // Assert
            text.Value
                .Should()
                .Be(LocalizedString1.Value);
        }

        private static DefaultExtendedStringLocalizer<DefaultExtendedStringLocalizerTests> CreateLocalizer(
            IEnumerable<LocalizedString> globalLocalizedStrings, IEnumerable<LocalizedString> localizedStrings,
            LocalizedString globalLocalizedString, LocalizedString localizedString)
        {
            var localizer = A.Fake<IStringLocalizer<DefaultExtendedStringLocalizerTests>>();

            var globalLocalizer = A.Fake<IStringLocalizer>();

            A.CallTo(() => globalLocalizer.GetAllStrings(A<bool>.Ignored)).Returns(globalLocalizedStrings);
            A.CallTo(() => localizer.GetAllStrings(A<bool>.Ignored)).Returns(localizedStrings);

            A.CallTo(() => globalLocalizer[A<string>.Ignored]).Returns(globalLocalizedString);
            A.CallTo(() => localizer[A<string>.Ignored]).Returns(localizedString);

            A.CallTo(() => globalLocalizer[A<string>.Ignored, A<string>.Ignored]).Returns(globalLocalizedString);
            A.CallTo(() => localizer[A<string>.Ignored, A<string>.Ignored]).Returns(localizedString);

            return new DefaultExtendedStringLocalizer<DefaultExtendedStringLocalizerTests>(globalLocalizer, localizer);
        }
    }
}
