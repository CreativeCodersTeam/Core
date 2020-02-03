using System.Windows;
using CreativeCoders.Mvvm.UnitTests.TestObjects;
using CreativeCoders.Mvvm.Wpf;
using FakeItEasy;
using Xunit;

namespace CreativeCoders.Mvvm.UnitTests.Wpf
{
    public class ItemTypeTemplateSelectorTests
    {
        [Fact]
        public void CtorTest()
        {
            var _ = new ItemTypeTemplateSelector();
        }

        [Fact]
        public void AddTypeTemplateMappingTest()
        {
            var selector = new TestMappingTemplateSelector();

            var dataTemplate = A.Fake<DataTemplate>();
            
            selector.AddMapping<string>(() => dataTemplate);
        }

        [Fact]
        public void SelectTemplateTest()
        {
            var selector = new TestMappingTemplateSelector();

            var dataTemplate = A.Fake<DataTemplate>();

            selector.AddMapping<string>(() => dataTemplate);

            var template = selector.SelectTemplate(string.Empty, null);

            Assert.Equal(dataTemplate, template);
        }
    }
}