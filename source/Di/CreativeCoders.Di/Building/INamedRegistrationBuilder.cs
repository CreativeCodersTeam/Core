using System;
using JetBrains.Annotations;

namespace CreativeCoders.Di.Building;

[PublicAPI]
public interface INamedRegistrationBuilder<in TService>
{
    INamedRegistrationBuilder<TService> Add<TImplementation>(string name)
        where TImplementation : class, TService;

    INamedRegistrationBuilder<TService> Add(Type implementationType, string name);

    void Build();
}