namespace CreativeCoders.Di;

public interface IServiceByNameFactory<out TService>
{
    TService GetInstance(IDiContainer container, string name);
}

public interface IServiceByNameFactory
{
    object GetServiceInstance(IDiContainer container, string name);
}