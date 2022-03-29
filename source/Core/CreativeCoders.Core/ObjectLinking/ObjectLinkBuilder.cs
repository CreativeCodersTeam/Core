using System;
using System.Collections.Generic;
using System.Linq;

namespace CreativeCoders.Core.ObjectLinking;

public class ObjectLinkBuilder
{
    private readonly object _instance0;
        
    private readonly object _instance1;
        
    private readonly Type _instance0Type;
        
    private readonly Type _instance1Type;

    public ObjectLinkBuilder(object instance0, object instance1)
    {
        Ensure.IsNotNull(instance0, nameof(instance0));
        Ensure.IsNotNull(instance1, nameof(instance1));
            
        _instance0 = instance0;
        _instance1 = instance1;
        _instance0Type = _instance0.GetType();
        _instance1Type = _instance1.GetType();
    }

    public ObjectLink Build()
    {
        var linkItems = GetLinkItems();
            
        var link = new ObjectLink(_instance0, _instance1, linkItems);

        return link;
    }

    private IEnumerable<PropertyLinkItem> GetLinkItems()
    {
        var instance0LinkDefinitions = GetLinkDefinitions(_instance0Type, _instance1Type, _instance0, _instance1);
        var instance1LinkDefinitions = GetLinkDefinitions(_instance1Type, _instance0Type, _instance1, _instance0);

        var linkDefinitions = MergeLinkDefinitions(instance0LinkDefinitions, instance1LinkDefinitions);

        var linkItems = new PropertyLinkItemsBuilder(linkDefinitions).Build();

        return linkItems;
    }
        
    private static IEnumerable<PropertyLinkDefinition> MergeLinkDefinitions(IEnumerable<PropertyLinkDefinition> instance0LinkDefinitions, IEnumerable<PropertyLinkDefinition> instance1LinkDefinitions)
    {
        var definitions = new List<PropertyLinkDefinition>(instance0LinkDefinitions);
        definitions.AddRange(instance1LinkDefinitions);

        return definitions;
    }

    private static IEnumerable<PropertyLinkDefinition> GetLinkDefinitions(Type sourceType, Type targetType, object source, object target)
    {
        return sourceType
            .GetProperties()
            .SelectMany(
                prop =>
                    prop.GetCustomAttributes(typeof(PropertyLinkAttribute), true)
                        .OfType<PropertyLinkAttribute>()
                        .Where(attr => TargetMatches(attr.TargetType, targetType))
                        .Select(linkAttr => new PropertyLinkDefinition
                        {
                            Source = source,
                            Target = target,
                            TargetType = linkAttr.TargetType,
                            LinkDirection = linkAttr.Direction,
                            SourceProperty = prop,
                            TargetPropertyName = linkAttr.TargetPropertyName,
                            Converter = linkAttr.Converter,
                            ConverterParameter = linkAttr.ConverterParameter,
                            InitWithTargetValue = linkAttr.InitWithTargetValue
                        }))
            .ToArray();
    }

    private static bool TargetMatches(Type attributeTargetType, Type targetType)
    {
        return attributeTargetType == targetType || targetType.IsSubclassOf(attributeTargetType);
    }
}