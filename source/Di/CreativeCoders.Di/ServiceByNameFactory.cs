using System;
using System.Collections.Generic;
using CreativeCoders.Core;

namespace CreativeCoders.Di;

public class ServiceByNameFactory<TService> : IServiceByNameFactory<TService>, IServiceByNameFactory
    where TService : class
{
    private readonly IDictionary<string, Type> _nameMap;

    public ServiceByNameFactory(IDictionary<string, Type> nameMap)
    {
        Ensure.IsNotNull(nameMap, nameof(nameMap));

        _nameMap = nameMap;
    }

    public TService GetInstance(IDiContainer container, string name)
    {
        return container.GetInstance(_nameMap[name]) as TService;
    }

    public object GetServiceInstance(IDiContainer container, string name)
    {
        return GetInstance(container, name);
    }
}