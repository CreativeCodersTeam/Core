using System;
using System.Windows.Input;
using Xunit;

namespace CreativeCoders.Mvvm.UnitTests
{
    public class ActionViewModelTests
    {
        [Fact]
        public void CtorTest()
        {
            var action = new ActionViewModel();
            action = new ActionViewModel(() => action = null);
            action = new ActionViewModel(() => action = null, () => action != null);
        }

        [Fact]
        public void ExecuteTest()
        {
            var actionExecuted = false;
            var action = new ActionViewModel(() => actionExecuted = true);
            action.Execute();
            Assert.True(actionExecuted);
        }

        [Fact]
        public void CaptionTest()
        {
            TestProperty(action => action.Caption, (action, value) => action.Caption = value, "TestCaption");
        }

        [Fact]
        public void IsVisibleTest()
        {
            TestProperty(action => action.IsVisible, (action, value) => action.IsVisible = value, true);
        }

        [Fact]
        public void ShortCutTest()
        {
            TestProperty(action => action.ShortCut, (action, value) => action.ShortCut = value, new KeyGesture(new Key()));
        }

        [Fact]
        public void ToolTipTest()
        {
            TestProperty(action => action.ToolTip, (action, value) => action.ToolTip = value, "TestToolTip");
        }

        [Fact]
        public void SmallIconTest()
        {
            TestProperty(action => action.SmallIcon, (action, value) => action.SmallIcon = value, "TestSmallIcon");
        }

        [Fact]
        public void LargeIconTest()
        {
            TestProperty(action => action.LargeIcon, (action, value) => action.LargeIcon = value, "TestLargeIcon");
        }

        [Fact]
        public void IsCheckedTest()
        {
            TestProperty(action => action.IsChecked, (action, value) => action.IsChecked = value, true);
        }

        private static void TestProperty<TProp>(Func<ActionViewModel, TProp> getProperty, Action<ActionViewModel, TProp> setProperty, TProp testValue)
        {
            var action = new ActionViewModel();
            setProperty(action, testValue);

            Assert.Equal(testValue, getProperty(action));
        }
    }
}
