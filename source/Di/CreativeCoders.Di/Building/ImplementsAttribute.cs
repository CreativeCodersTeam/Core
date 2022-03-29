using System;

namespace CreativeCoders.Di.Building;

[AttributeUsage(AttributeTargets.Class)]
public class ImplementsAttribute : Attribute
{
    public ImplementsAttribute(Type serviceType)
    {
        ServiceType = serviceType;
    }

    public Type ServiceType { get; }

    public ImplementationLifecycle Lifecycle { get; set; } = ImplementationLifecycle.Transient;

    //public string Name { get; set; } = string.Empty;
}