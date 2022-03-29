using System;
using System.Collections.Generic;
using CreativeCoders.Core;

namespace CreativeCoders.Di.Building;

public class NamedRegistrationBuilder<TService> : INamedRegistrationBuilder<TService>
    where TService : class
{
    private readonly Action<IDictionary<string, Type>> _register;

    private readonly IDictionary<string, Type> _nameMap;

    public NamedRegistrationBuilder(Action<IDictionary<string, Type>> register)
    {
        Ensure.IsNotNull(register, nameof(register));

        _register = register;
        _nameMap = new Dictionary<string, Type>();
    }

    public INamedRegistrationBuilder<TService> Add<TImplementation>(string name)
        where TImplementation : class, TService
    {
        return Add(typeof(TImplementation), name);
    }

    public INamedRegistrationBuilder<TService> Add(Type implementationType, string name)
    {
        _nameMap.Add(name, implementationType);
        return this;
    }

    public void Build()
    {
        _register(_nameMap);
    }
}
