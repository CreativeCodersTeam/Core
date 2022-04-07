namespace CreativeCoders.Core.UnitTests.Di.Helper;

public interface ITestService<T>
{
    T Data { get; set; }
}
