using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CreativeCoders.Config;
using CreativeCoders.Config.Base;
using FakeItEasy;

namespace CreativeCoders.Core.UnitTests.Config
{
    [SuppressMessage("ReSharper", "MemberCanBeMadeStatic.Global")]
    public class SettingTestHelper
    {
        public void TestSettingCtor<T>(Func<ISettingFactory<T>, ISetting<T>> createSetting)
            where T : class
        {
            Xunit.Assert.Throws<ArgumentNullException>(() => createSetting(null));

            var factory = A.Fake<ISettingFactory<T>>();

            var setting = createSetting(factory);

            Xunit.Assert.NotNull(setting);
        }

        public void TestSettingValue<T>(Func<ISettingFactory<T>, ISetting<T>> createSetting, Func<T> createValue)
            where T : class
        {
            var settingObject0 = createValue();
            var settingObject1 = createValue();
            var factory = A.Fake<ISettingFactory<T>>();
            A.CallTo(() => factory.Create()).Returns(settingObject0).NumberOfTimes(1).Then.Returns(settingObject1);

            var setting = createSetting(factory);

            var value0 = setting.Value;
            var value1 = setting.Value;

            Xunit.Assert.Same(settingObject0, value0);
            Xunit.Assert.Same(settingObject0, value1);
            Xunit.Assert.Same(value0, value1);
        }

        public void TestSettingsCtor<T>(Func<ISettingsFactory<T>, ISettings<T>> createSettings)
            where T : class
        {
            Xunit.Assert.Throws<ArgumentNullException>(() => createSettings(null));

            var factory = A.Fake<ISettingsFactory<T>>();

            var settings = new Settings<T>(factory);

            Xunit.Assert.NotNull(settings);
        }

        public void TestSettingsValues<T>(Func<ISettingsFactory<T>, ISettings<T>> createSettings, Func<IEnumerable<T>> createValues)
            where T : class
        {
            var settingObjects0 = createValues();
            var settingObjects1 = createValues();

            var factory = A.Fake<ISettingsFactory<T>>();
            A.CallTo(() => factory.Create()).Returns(settingObjects0).NumberOfTimes(1).Then.Returns(settingObjects1);

            var settings = createSettings(factory);

            var values0 = settings.Values;
            var values1 = settings.Values;

            Xunit.Assert.Same(settingObjects0, values0);
            Xunit.Assert.Same(settingObjects0, values1);
            Xunit.Assert.Same(values0, values1);            
        }
    }
}