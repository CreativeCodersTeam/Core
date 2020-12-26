using System.Collections.Generic;
using System.Linq;
using CreativeCoders.Core.ObjectLinking;
using CreativeCoders.Core.ObjectLinking.Converters;
using Xunit;

namespace CreativeCoders.Core.UnitTests.ObjectLinking
{
    public class PropertyLinkItemsBuilderTests
    {
        [Fact]
        public void Build_OneDefinitionInput_OneInfoOutput()
        {
            var sourceTestData = new SourceTestData();
            var targetTestData = new TargetTestData();
            const string sourcePropertyName = nameof(SourceTestData.SourceName);
            const string targetPropertyName = nameof(TargetTestData.TargetName);
            var sourceProperty = sourceTestData.GetType().GetProperty(sourcePropertyName);
            var targetProperty = targetTestData.GetType().GetProperty(targetPropertyName);

            var linkDefinitions = new List<PropertyLinkDefinition>
            {
                new()
                {
                    Source = sourceTestData,
                    Target = targetTestData,
                    SourceProperty = sourceProperty,
                    TargetType = targetTestData.GetType(),
                    TargetPropertyName = targetPropertyName,
                    LinkDirection = LinkDirection.OneWayToTarget
                }
            };

            var items = new PropertyLinkItemsBuilder(linkDefinitions).Build().ToArray();

            Assert.Single(items);
            var item = items.First();

            Assert.Equal(sourceTestData, item.Info.Source);
            Assert.Equal(targetTestData, item.Info.Target);
            Assert.Equal(sourceProperty, item.Info.SourceProperty);
            Assert.Equal(targetProperty, item.Info.TargetProperty);
            Assert.Equal(LinkDirection.OneWayToTarget, item.Info.Direction);
        }

        [Fact]
        public void Build_TwoDifferentDefinitionInput_TwoInfoOutput()
        {
            var sourceTestData = new SourceTestData();
            var targetTestData = new TargetTestData();
            const string sourcePropertyName0 = nameof(SourceTestData.SourceName);
            const string targetPropertyName0 = nameof(TargetTestData.TargetName);
            const string sourcePropertyName1 = nameof(SourceTestData.SourceText);
            const string targetPropertyName1 = nameof(TargetTestData.TargetText);
            var sourceProperty0 = sourceTestData.GetType().GetProperty(sourcePropertyName0);
            var targetProperty0 = targetTestData.GetType().GetProperty(targetPropertyName0);
            var sourceProperty1 = sourceTestData.GetType().GetProperty(sourcePropertyName1);
            var targetProperty1 = targetTestData.GetType().GetProperty(targetPropertyName1);

            var linkDefinitions = new List<PropertyLinkDefinition>
            {
                new()
                {
                    Source = sourceTestData,
                    Target = targetTestData,
                    SourceProperty = sourceProperty0,
                    TargetType = targetTestData.GetType(),
                    TargetPropertyName = targetPropertyName0,
                    LinkDirection = LinkDirection.OneWayToTarget
                },
                new()
                {
                    Source = sourceTestData,
                    Target = targetTestData,
                    SourceProperty = sourceProperty1,
                    TargetType = targetTestData.GetType(),
                    TargetPropertyName = targetPropertyName1,
                    LinkDirection = LinkDirection.OneWayFromTarget
                }
            };

            var items = new PropertyLinkItemsBuilder(linkDefinitions).Build().ToArray();

            Assert.Equal(2, items.Length);
            var item0 = items.First();

            Assert.Equal(sourceTestData, item0.Info.Source);
            Assert.Equal(targetTestData, item0.Info.Target);
            Assert.Equal(sourceProperty0, item0.Info.SourceProperty);
            Assert.Equal(targetProperty0, item0.Info.TargetProperty);
            Assert.Equal(LinkDirection.OneWayToTarget, item0.Info.Direction);

            var item1 = items.Last();

            Assert.Equal(sourceTestData, item1.Info.Source);
            Assert.Equal(targetTestData, item1.Info.Target);
            Assert.Equal(sourceProperty1, item1.Info.SourceProperty);
            Assert.Equal(targetProperty1, item1.Info.TargetProperty);
            Assert.Equal(LinkDirection.OneWayFromTarget, item1.Info.Direction);
        }

        [Fact]
        public void Build_TwoDifferentDefinitionWithOneEqualPropertyInput_TwoInfoOutput()
        {
            var sourceTestData = new SourceTestData();
            var targetTestData = new TargetTestData();
            const string sourcePropertyName0 = nameof(SourceTestData.SourceName);
            const string targetPropertyName0 = nameof(TargetTestData.TargetName);
            const string sourcePropertyName1 = nameof(SourceTestData.SourceText);
            const string targetPropertyName1 = nameof(TargetTestData.TargetName);
            var sourceProperty0 = sourceTestData.GetType().GetProperty(sourcePropertyName0);
            var targetProperty0 = targetTestData.GetType().GetProperty(targetPropertyName0);
            var sourceProperty1 = sourceTestData.GetType().GetProperty(sourcePropertyName1);
            var targetProperty1 = targetTestData.GetType().GetProperty(targetPropertyName1);

            var linkDefinitions = new List<PropertyLinkDefinition>
            {
                new()
                {
                    Source = sourceTestData,
                    Target = targetTestData,
                    SourceProperty = sourceProperty0,
                    TargetType = targetTestData.GetType(),
                    TargetPropertyName = targetPropertyName0,
                    LinkDirection = LinkDirection.OneWayToTarget
                },
                new()
                {
                    Source = sourceTestData,
                    Target = targetTestData,
                    SourceProperty = sourceProperty1,
                    TargetType = targetTestData.GetType(),
                    TargetPropertyName = targetPropertyName1,
                    LinkDirection = LinkDirection.OneWayFromTarget
                }
            };

            var items = new PropertyLinkItemsBuilder(linkDefinitions).Build().ToArray();

            Assert.Equal(2, items.Length);
            var item0 = items.First();

            Assert.Equal(sourceTestData, item0.Info.Source);
            Assert.Equal(targetTestData, item0.Info.Target);
            Assert.Equal(sourceProperty0, item0.Info.SourceProperty);
            Assert.Equal(targetProperty0, item0.Info.TargetProperty);
            Assert.Equal(LinkDirection.OneWayToTarget, item0.Info.Direction);

            var item1 = items.Last();

            Assert.Equal(sourceTestData, item1.Info.Source);
            Assert.Equal(targetTestData, item1.Info.Target);
            Assert.Equal(sourceProperty1, item1.Info.SourceProperty);
            Assert.Equal(targetProperty1, item1.Info.TargetProperty);
            Assert.Equal(LinkDirection.OneWayFromTarget, item1.Info.Direction);
        }

        [Fact]
        public void Build_TwoDefinitionWithSameDataInput_OneInfoOutput()
        {
            var sourceTestData = new SourceTestData();
            var targetTestData = new TargetTestData();
            const string sourcePropertyName = nameof(SourceTestData.SourceName);
            const string targetPropertyName = nameof(TargetTestData.TargetName);
            var sourceProperty = sourceTestData.GetType().GetProperty(sourcePropertyName);
            var targetProperty = targetTestData.GetType().GetProperty(targetPropertyName);

            var linkDefinitions = new List<PropertyLinkDefinition>
            {
                new()
                {
                    Source = sourceTestData,
                    Target = targetTestData,
                    SourceProperty = sourceProperty,
                    TargetType = targetTestData.GetType(),
                    TargetPropertyName = targetPropertyName,
                    LinkDirection = LinkDirection.OneWayToTarget
                },
                new()
                {
                    Source = sourceTestData,
                    Target = targetTestData,
                    SourceProperty = sourceProperty,
                    TargetType = targetTestData.GetType(),
                    TargetPropertyName = targetPropertyName,
                    LinkDirection = LinkDirection.OneWayToTarget
                }
            };

            var items = new PropertyLinkItemsBuilder(linkDefinitions).Build().ToArray();

            Assert.Single(items);
            var item = items.First();

            Assert.Equal(sourceTestData, item.Info.Source);
            Assert.Equal(targetTestData, item.Info.Target);
            Assert.Equal(sourceProperty, item.Info.SourceProperty);
            Assert.Equal(targetProperty, item.Info.TargetProperty);
            Assert.Equal(LinkDirection.OneWayToTarget, item.Info.Direction);
        }
        
        [Fact]
        public void Build_TwoDefinitionWithSameDataTargetTwoWayInput_OneInfoOutput()
        {
            var sourceTestData = new SourceTestData();
            var targetTestData = new TargetTestData();
            const string sourcePropertyName = nameof(SourceTestData.SourceName);
            const string targetPropertyName = nameof(TargetTestData.TargetName);
            var sourceProperty = sourceTestData.GetType().GetProperty(sourcePropertyName);
            var targetProperty = targetTestData.GetType().GetProperty(targetPropertyName);

            var linkDefinitions = new List<PropertyLinkDefinition>
            {
                new()
                {
                    Source = sourceTestData,
                    Target = targetTestData,
                    SourceProperty = sourceProperty,
                    TargetType = targetTestData.GetType(),
                    TargetPropertyName = targetPropertyName,
                    LinkDirection = LinkDirection.OneWayToTarget
                },
                new()
                {
                    Source = sourceTestData,
                    Target = targetTestData,
                    SourceProperty = sourceProperty,
                    TargetType = targetTestData.GetType(),
                    TargetPropertyName = targetPropertyName,
                    LinkDirection = LinkDirection.TwoWay
                }
            };

            var items = new PropertyLinkItemsBuilder(linkDefinitions).Build().ToArray();

            Assert.Single(items);
            var item = items.First();

            Assert.Equal(sourceTestData, item.Info.Source);
            Assert.Equal(targetTestData, item.Info.Target);
            Assert.Equal(sourceProperty, item.Info.SourceProperty);
            Assert.Equal(targetProperty, item.Info.TargetProperty);
            Assert.Equal(LinkDirection.TwoWay, item.Info.Direction);
        }
        
        [Fact]
        public void Build_TwoDefinitionWithSameDataSourceTwoWayInput_OneInfoOutput()
        {
            var sourceTestData = new SourceTestData();
            var targetTestData = new TargetTestData();
            const string sourcePropertyName = nameof(SourceTestData.SourceName);
            const string targetPropertyName = nameof(TargetTestData.TargetName);
            var sourceProperty = sourceTestData.GetType().GetProperty(sourcePropertyName);
            var targetProperty = targetTestData.GetType().GetProperty(targetPropertyName);

            var linkDefinitions = new List<PropertyLinkDefinition>
            {
                new()
                {
                    Source = sourceTestData,
                    Target = targetTestData,
                    SourceProperty = sourceProperty,
                    TargetType = targetTestData.GetType(),
                    TargetPropertyName = targetPropertyName,
                    LinkDirection = LinkDirection.TwoWay
                },
                new()
                {
                    Source = sourceTestData,
                    Target = targetTestData,
                    SourceProperty = sourceProperty,
                    TargetType = targetTestData.GetType(),
                    TargetPropertyName = targetPropertyName,
                    LinkDirection = LinkDirection.OneWayToTarget
                }
            };

            var items = new PropertyLinkItemsBuilder(linkDefinitions).Build().ToArray();

            Assert.Single(items);
            var item = items.First();

            Assert.Equal(sourceTestData, item.Info.Source);
            Assert.Equal(targetTestData, item.Info.Target);
            Assert.Equal(sourceProperty, item.Info.SourceProperty);
            Assert.Equal(targetProperty, item.Info.TargetProperty);
            Assert.Equal(LinkDirection.TwoWay, item.Info.Direction);
        }

        [Fact]
        public void Build_TwoDefinitionWithSameDataButDifferentDirectionInput_OneInfoWithTwoWayOutput()
        {
            var sourceTestData = new SourceTestData();
            var targetTestData = new TargetTestData();
            const string sourcePropertyName = nameof(SourceTestData.SourceName);
            const string targetPropertyName = nameof(TargetTestData.TargetName);
            var sourceProperty = sourceTestData.GetType().GetProperty(sourcePropertyName);
            var targetProperty = targetTestData.GetType().GetProperty(targetPropertyName);

            var linkDefinitions = new List<PropertyLinkDefinition>
            {
                new()
                {
                    Source = sourceTestData,
                    Target = targetTestData,
                    SourceProperty = sourceProperty,
                    TargetType = targetTestData.GetType(),
                    TargetPropertyName = targetPropertyName,
                    LinkDirection = LinkDirection.OneWayFromTarget
                },
                new()
                {
                    Source = sourceTestData,
                    Target = targetTestData,
                    SourceProperty = sourceProperty,
                    TargetType = targetTestData.GetType(),
                    TargetPropertyName = targetPropertyName,
                    LinkDirection = LinkDirection.OneWayToTarget
                }
            };

            var items = new PropertyLinkItemsBuilder(linkDefinitions).Build().ToArray();

            Assert.Single(items);
            var item = items.First();

            Assert.Equal(sourceTestData, item.Info.Source);
            Assert.Equal(targetTestData, item.Info.Target);
            Assert.Equal(sourceProperty, item.Info.SourceProperty);
            Assert.Equal(targetProperty, item.Info.TargetProperty);
            Assert.Equal(LinkDirection.TwoWay, item.Info.Direction);
        }
        
        [Fact]
        public void Build_TwoDefinitionWithOppositeLinkingInput_OneInfoWithOneWayOutput()
        {
            var sourceTestData = new SourceTestData();
            var targetTestData = new TargetTestData();
            const string sourcePropertyName = nameof(SourceTestData.SourceName);
            const string targetPropertyName = nameof(TargetTestData.TargetName);
            var sourceProperty = sourceTestData.GetType().GetProperty(sourcePropertyName);
            var targetProperty = targetTestData.GetType().GetProperty(targetPropertyName);
            
            var linkDefinitions = new List<PropertyLinkDefinition>
            {
                new()
                {
                    Source = sourceTestData,
                    Target = targetTestData,
                    SourceProperty = sourceProperty,
                    TargetType = targetTestData.GetType(),
                    TargetPropertyName = targetPropertyName,
                    LinkDirection = LinkDirection.OneWayFromTarget
                },
                new()
                {
                    Source = targetTestData,
                    Target = sourceTestData,
                    SourceProperty = targetProperty,
                    TargetType = sourceTestData.GetType(),
                    TargetPropertyName = sourcePropertyName,
                    LinkDirection = LinkDirection.OneWayToTarget
                }
            };
            
            var items = new PropertyLinkItemsBuilder(linkDefinitions).Build().ToArray();

            Assert.Single(items);
            var item = items.First();
            
            Assert.Equal(sourceTestData, item.Info.Source);
            Assert.Equal(targetTestData, item.Info.Target);
            Assert.Equal(sourceProperty, item.Info.SourceProperty);
            Assert.Equal(targetProperty, item.Info.TargetProperty);
            Assert.Equal(LinkDirection.OneWayFromTarget, item.Info.Direction);
        }
        
        [Fact]
        public void Build_TwoDefinitionWithOppositeLinkingInput_OneInfoWithTwoWayOutput()
        {
            var sourceTestData = new SourceTestData();
            var targetTestData = new TargetTestData();
            const string sourcePropertyName = nameof(SourceTestData.SourceName);
            const string targetPropertyName = nameof(TargetTestData.TargetName);
            var sourceProperty = sourceTestData.GetType().GetProperty(sourcePropertyName);
            var targetProperty = targetTestData.GetType().GetProperty(targetPropertyName);
            
            var linkDefinitions = new List<PropertyLinkDefinition>
            {
                new()
                {
                    Source = sourceTestData,
                    Target = targetTestData,
                    SourceProperty = sourceProperty,
                    TargetType = targetTestData.GetType(),
                    TargetPropertyName = targetPropertyName,
                    LinkDirection = LinkDirection.OneWayToTarget
                },
                new()
                {
                    Source = targetTestData,
                    Target = sourceTestData,
                    SourceProperty = targetProperty,
                    TargetType = sourceTestData.GetType(),
                    TargetPropertyName = sourcePropertyName,
                    LinkDirection = LinkDirection.OneWayToTarget
                }
            };
            
            var items = new PropertyLinkItemsBuilder(linkDefinitions).Build().ToArray();

            Assert.Single(items);
            var item = items.First();
            
            Assert.Equal(sourceTestData, item.Info.Source);
            Assert.Equal(targetTestData, item.Info.Target);
            Assert.Equal(sourceProperty, item.Info.SourceProperty);
            Assert.Equal(targetProperty, item.Info.TargetProperty);
            Assert.Equal(LinkDirection.TwoWay, item.Info.Direction);
        }
        
        [Fact]
        public void Build_TwoDefinitionWithOppositeLinkingWithOneTwoWayLinkInput_OneInfoWithTwoWayOutput()
        {
            var sourceTestData = new SourceTestData();
            var targetTestData = new TargetTestData();
            const string sourcePropertyName = nameof(SourceTestData.SourceName);
            const string targetPropertyName = nameof(TargetTestData.TargetName);
            var sourceProperty = sourceTestData.GetType().GetProperty(sourcePropertyName);
            var targetProperty = targetTestData.GetType().GetProperty(targetPropertyName);
            
            var linkDefinitions = new List<PropertyLinkDefinition>
            {
                new()
                {
                    Source = sourceTestData,
                    Target = targetTestData,
                    SourceProperty = sourceProperty,
                    TargetType = targetTestData.GetType(),
                    TargetPropertyName = targetPropertyName,
                    LinkDirection = LinkDirection.OneWayToTarget
                },
                new()
                {
                    Source = targetTestData,
                    Target = sourceTestData,
                    SourceProperty = targetProperty,
                    TargetType = sourceTestData.GetType(),
                    TargetPropertyName = sourcePropertyName,
                    LinkDirection = LinkDirection.TwoWay
                }
            };
            
            var items = new PropertyLinkItemsBuilder(linkDefinitions).Build().ToArray();

            Assert.Single(items);
            var item = items.First();
            
            Assert.Equal(sourceTestData, item.Info.Source);
            Assert.Equal(targetTestData, item.Info.Target);
            Assert.Equal(sourceProperty, item.Info.SourceProperty);
            Assert.Equal(targetProperty, item.Info.TargetProperty);
            Assert.Equal(LinkDirection.TwoWay, item.Info.Direction);
        }
        
        [Fact]
        public void Build_TwoDefinitionWithSameDataButDifferentConverterInput_TwoInfoOutput()
        {
            var sourceTestData = new SourceTestData();
            var targetTestData = new TargetTestData();
            const string sourcePropertyName = nameof(SourceTestData.SourceName);
            const string targetPropertyName = nameof(TargetTestData.TargetName);
            var sourceProperty = sourceTestData.GetType().GetProperty(sourcePropertyName);
            var targetProperty = targetTestData.GetType().GetProperty(targetPropertyName);

            var linkDefinitions = new List<PropertyLinkDefinition>
            {
                new()
                {
                    Source = sourceTestData,
                    Target = targetTestData,
                    SourceProperty = sourceProperty,
                    TargetType = targetTestData.GetType(),
                    TargetPropertyName = targetPropertyName,
                    LinkDirection = LinkDirection.OneWayToTarget,
                    Converter = typeof(StringSourcePropertyConverter<int>)
                },
                new()
                {
                    Source = sourceTestData,
                    Target = targetTestData,
                    SourceProperty = sourceProperty,
                    TargetType = targetTestData.GetType(),
                    TargetPropertyName = targetPropertyName,
                    LinkDirection = LinkDirection.OneWayToTarget
                }
            };

            var items = new PropertyLinkItemsBuilder(linkDefinitions).Build().ToArray();

            Assert.Equal(2, items.Length);
            var firstItem = items[0];

            Assert.Equal(sourceTestData, firstItem.Info.Source);
            Assert.Equal(targetTestData, firstItem.Info.Target);
            Assert.Equal(sourceProperty, firstItem.Info.SourceProperty);
            Assert.Equal(targetProperty, firstItem.Info.TargetProperty);
            Assert.IsType<StringSourcePropertyConverter<int>>(firstItem.Info.Converter);
            Assert.Equal(LinkDirection.OneWayToTarget, firstItem.Info.Direction);
            
            var secondItem = items[1];

            Assert.Equal(sourceTestData, secondItem.Info.Source);
            Assert.Equal(targetTestData, secondItem.Info.Target);
            Assert.Equal(sourceProperty, secondItem.Info.SourceProperty);
            Assert.Equal(targetProperty, secondItem.Info.TargetProperty);
            Assert.Null(secondItem.Info.Converter);
            Assert.Equal(LinkDirection.OneWayToTarget, secondItem.Info.Direction);
        }
        
        [Fact]
        public void Build_TwoDefinitionWithSameDataButDifferentConverterParametersInput_TwoInfoOutput()
        {
            var sourceTestData = new SourceTestData();
            var targetTestData = new TargetTestData();
            const string sourcePropertyName = nameof(SourceTestData.SourceName);
            const string targetPropertyName = nameof(TargetTestData.TargetName);
            var sourceProperty = sourceTestData.GetType().GetProperty(sourcePropertyName);
            var targetProperty = targetTestData.GetType().GetProperty(targetPropertyName);

            var linkDefinitions = new List<PropertyLinkDefinition>
            {
                new()
                {
                    Source = sourceTestData,
                    Target = targetTestData,
                    SourceProperty = sourceProperty,
                    TargetType = targetTestData.GetType(),
                    TargetPropertyName = targetPropertyName,
                    LinkDirection = LinkDirection.OneWayToTarget,
                    Converter = typeof(StringSourcePropertyConverter<int>),
                    ConverterParameter = 1
                },
                new()
                {
                    Source = sourceTestData,
                    Target = targetTestData,
                    SourceProperty = sourceProperty,
                    TargetType = targetTestData.GetType(),
                    TargetPropertyName = targetPropertyName,
                    LinkDirection = LinkDirection.OneWayToTarget,
                    Converter = typeof(StringSourcePropertyConverter<int>),
                    ConverterParameter = 2
                }
            };

            var items = new PropertyLinkItemsBuilder(linkDefinitions).Build().ToArray();

            Assert.Equal(2, items.Length);
            var firstItem = items[0];

            Assert.Equal(sourceTestData, firstItem.Info.Source);
            Assert.Equal(targetTestData, firstItem.Info.Target);
            Assert.Equal(sourceProperty, firstItem.Info.SourceProperty);
            Assert.Equal(targetProperty, firstItem.Info.TargetProperty);
            Assert.IsType<StringSourcePropertyConverter<int>>(firstItem.Info.Converter);
            Assert.Equal(1, firstItem.Info.ConverterParameter);
            Assert.Equal(LinkDirection.OneWayToTarget, firstItem.Info.Direction);
            
            var secondItem = items[1];

            Assert.Equal(sourceTestData, secondItem.Info.Source);
            Assert.Equal(targetTestData, secondItem.Info.Target);
            Assert.Equal(sourceProperty, secondItem.Info.SourceProperty);
            Assert.Equal(targetProperty, secondItem.Info.TargetProperty);
            Assert.IsType<StringSourcePropertyConverter<int>>(secondItem.Info.Converter);
            Assert.Equal(2, secondItem.Info.ConverterParameter);
            Assert.Equal(LinkDirection.OneWayToTarget, secondItem.Info.Direction);
        }
    }
}
