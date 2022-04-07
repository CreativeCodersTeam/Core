using System;
using System.Collections.Generic;
using System.Linq;
using CreativeCoders.Core.Chaining;

namespace CreativeCoders.Core.ObjectLinking;

public class PropertyLinkItemsBuilder
{
    private readonly IEnumerable<PropertyLinkDefinition> _propertyLinkDefinitions;

    private readonly HandlerChain<(PropertyLinkInfo, PropertyLinkInfo), bool> _handlerChain;

    public PropertyLinkItemsBuilder(IEnumerable<PropertyLinkDefinition> propertyLinkDefinitions)
    {
        _propertyLinkDefinitions = propertyLinkDefinitions;
        _handlerChain = CreateHandlerChain();
    }

    public IEnumerable<PropertyLinkItem> Build()
    {
        var linkItems = new List<PropertyLinkItem>();
        foreach (var propertyLinkInfo in GetLinkInfos(linkItems))
        {
            linkItems.Add(new PropertyLinkItem(propertyLinkInfo));
        }

        return linkItems;
    }

    private IEnumerable<PropertyLinkInfo> GetLinkInfos(IEnumerable<PropertyLinkItem> linkItems)
    {
        return _propertyLinkDefinitions
            .Select(linkDefinition =>
                new PropertyLinkInfo
                {
                    Source = linkDefinition.Source,
                    Target = linkDefinition.Target,
                    SourceProperty = linkDefinition.SourceProperty,
                    TargetProperty = linkDefinition.TargetType.GetProperty(linkDefinition.TargetPropertyName),
                    Direction = linkDefinition.LinkDirection,
                    Converter = linkDefinition.Converter != null
                        ? Activator.CreateInstance(linkDefinition.Converter) as IPropertyValueConverter
                        : null,
                    ConverterParameter = linkDefinition.ConverterParameter,
                    InitWithTargetValue = linkDefinition.InitWithTargetValue
                })
            .Where(propertyLinkInfo =>
                !linkItems.Any(linkItem => MergeLinkInfos(linkItem.Info, propertyLinkInfo)));
    }

    private bool MergeLinkInfos(PropertyLinkInfo existingLinkInfo, PropertyLinkInfo newLinkInfo)
    {
        var handleResult = _handlerChain.Handle((existingLinkInfo, newLinkInfo));

        return handleResult;
    }

    private static HandlerChain<(PropertyLinkInfo, PropertyLinkInfo), bool> CreateHandlerChain()
    {
        var handlerChain = new HandlerChain<(PropertyLinkInfo, PropertyLinkInfo), bool>(new[]
        {
            new ChainDataHandler<(PropertyLinkInfo, PropertyLinkInfo), bool>(CheckDifferentConverters),
            new ChainDataHandler<(PropertyLinkInfo, PropertyLinkInfo), bool>(CheckOppositeLink),
            new ChainDataHandler<(PropertyLinkInfo, PropertyLinkInfo), bool>(CheckDifferentLink),
            new ChainDataHandler<(PropertyLinkInfo, PropertyLinkInfo), bool>(CheckExistingDirection)
        });

        return handlerChain;
    }

    private static HandleResult<bool> CheckExistingDirection(
        (PropertyLinkInfo existingLinkInfo, PropertyLinkInfo newLinkInfo) data)
    {
        var (existingLinkInfo, newLinkInfo) = data;

        if (existingLinkInfo.Direction == LinkDirection.TwoWay)
        {
            return new HandleResult<bool>(true, true);
        }

        if (existingLinkInfo.Direction != newLinkInfo.Direction)
        {
            existingLinkInfo.Direction = LinkDirection.TwoWay;
        }

        return new HandleResult<bool>(true, true);
    }

    private static HandleResult<bool> CheckDifferentLink(
        (PropertyLinkInfo existingLinkInfo, PropertyLinkInfo newLinkInfo) data)
    {
        var (existingLinkInfo, newLinkInfo) = data;

        if (existingLinkInfo.Source != newLinkInfo.Source ||
            existingLinkInfo.Target != newLinkInfo.Target ||
            existingLinkInfo.SourceProperty != newLinkInfo.SourceProperty ||
            existingLinkInfo.TargetProperty != newLinkInfo.TargetProperty)
        {
            return new HandleResult<bool>(true, false);
        }

        return new HandleResult<bool>(false, false);
    }

    private static HandleResult<bool> CheckOppositeLink(
        (PropertyLinkInfo existingLinkInfo, PropertyLinkInfo newLinkInfo) data)
    {
        var (existingLinkInfo, newLinkInfo) = data;

        if (existingLinkInfo.Source != newLinkInfo.Target ||
            existingLinkInfo.SourceProperty != newLinkInfo.TargetProperty ||
            existingLinkInfo.Target != newLinkInfo.Source ||
            existingLinkInfo.TargetProperty != newLinkInfo.SourceProperty)
        {
            return new HandleResult<bool>(false, false);
        }

        if (existingLinkInfo.Direction == LinkDirection.TwoWay ||
            newLinkInfo.Direction == LinkDirection.TwoWay)
        {
            existingLinkInfo.Direction = LinkDirection.TwoWay;
            return new HandleResult<bool>(true, true);
        }

        if (existingLinkInfo.Direction != newLinkInfo.Direction)
        {
            return new HandleResult<bool>(true, true);
        }

        existingLinkInfo.Direction = LinkDirection.TwoWay;

        return new HandleResult<bool>(true, true);
    }

    private static HandleResult<bool> CheckDifferentConverters(
        (PropertyLinkInfo existingLinkInfo, PropertyLinkInfo newLinkInfo) data)
    {
        var (existingLinkInfo, newLinkInfo) = data;

        return new HandleResult<bool>(
            existingLinkInfo.Converter != newLinkInfo.Converter ||
            existingLinkInfo.ConverterParameter != newLinkInfo.ConverterParameter,
            false);
    }
}
