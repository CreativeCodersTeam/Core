using CreativeCoders.Core.UnitTests.Di.Helper;
using CreativeCoders.Di;
using CreativeCoders.Di.Exceptions;
using FakeItEasy;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Di;

public class DiContainerClassFactoryTests
{
    [Fact]
    public void Create_WithClassTypeAsParameter_ReturnsCorrectClass()
    {
        var testService = new TestService();
            
        var diContainerMock = A.Fake<IDiContainer>();
        A.CallTo(() => diContainerMock.GetInstance(typeof(ITestService))).Returns(testService);

        var classFactory = new DiContainerClassFactory(diContainerMock);

        var instance = classFactory.Create(typeof(ITestService));
            
        Assert.Same(testService, instance);
    }
        
    [Fact]
    public void Create_WithWrongClassTypeAsParameter_ThrowsException()
    {
        var diContainerMock = A.Fake<IDiContainer>();
        A.CallTo(() => diContainerMock.GetInstance(typeof(ITestService)))
            .Throws(_ => new ResolveFailedException(typeof(ITestService), null));

        var classFactory = new DiContainerClassFactory(diContainerMock);

        Assert.Throws<ResolveFailedException>(() => classFactory.Create(typeof(ITestService)));
    }
        
    [Fact]
    public void Create_WithSetupWithClassTypeAsParameter_ReturnsCorrectClass()
    {
        var testService = new TestService();
            
        var diContainerMock = A.Fake<IDiContainer>();
        A.CallTo(() => diContainerMock.GetInstance(typeof(ITestService))).Returns(testService);

        var classFactory = new DiContainerClassFactory(diContainerMock);

        var instance = classFactory.Create(typeof(ITestService), x => ((ITestService) x).Text = "1234");
            
        Assert.Same(testService, instance);
        Assert.Equal("1234", ((ITestService) instance).Text);
    }
        
    [Fact]
    public void Create_WithClassTypeAsGenericParameter_ReturnsCorrectClass()
    {
        var testService = new TestService();
            
        var diContainerMock = A.Fake<IDiContainer>();
        A.CallTo(() => diContainerMock.GetInstance<ITestService>()).Returns(testService);

        var classFactory = new DiContainerClassFactory(diContainerMock);

        var instance = classFactory.Create<ITestService>();
            
        Assert.Same(testService, instance);
    }
        
    [Fact]
    public void Create_WithSetupWithClassTypeAsGenericParameter_ReturnsCorrectClass()
    {
        var testService = new TestService();
            
        var diContainerMock = A.Fake<IDiContainer>();
        A.CallTo(() => diContainerMock.GetInstance<ITestService>()).Returns(testService);

        var classFactory = new DiContainerClassFactory(diContainerMock);

        var instance = classFactory.Create<ITestService>(x => x.Text = "abcd");
            
        Assert.Same(testService, instance);
        Assert.Equal("abcd", instance.Text);
    }
        
    [Fact]
    public void Create_GenericWithClassTypeAsGenericParameter_ReturnsCorrectClass()
    {
        var testService = new TestService();
            
        var diContainerMock = A.Fake<IDiContainer>();
        A.CallTo(() => diContainerMock.GetInstance<ITestService>()).Returns(testService);

        var classFactory = new DiContainerClassFactory<ITestService>(diContainerMock);

        var instance = classFactory.Create();
            
        Assert.Same(testService, instance);
    }
        
    [Fact]
    public void Create_GenericWithSetupWithClassTypeAsGenericParameter_ReturnsCorrectClass()
    {
        var testService = new TestService();
            
        var diContainerMock = A.Fake<IDiContainer>();
        A.CallTo(() => diContainerMock.GetInstance<ITestService>()).Returns(testService);

        var classFactory = new DiContainerClassFactory<ITestService>(diContainerMock);

        var instance = classFactory.Create(x => x.Text = "abcd");
            
        Assert.Same(testService, instance);
        Assert.Equal("abcd", instance.Text);
    }
        
    [Fact]
    public void Create_GenericWithClassTypeAsGenericParameterForWrongType_ThrowsException()
    {
        var diContainerMock = A.Fake<IDiContainer>();
        A.CallTo(() => diContainerMock.GetInstance<ITestService>())
            .Throws(_ => new ResolveFailedException(typeof(ITestService), null));

        var classFactory = new DiContainerClassFactory<ITestService>(diContainerMock);

        Assert.Throws<ResolveFailedException>(() => classFactory.Create());
    }
        
    [Fact]
    public void Create_GenericWithSetupWithClassTypeAsGenericParameterForWrongType_ThrowsException()
    {
        var diContainerMock = A.Fake<IDiContainer>();
        A.CallTo(() => diContainerMock.GetInstance<ITestService>())
            .Throws(_ => new ResolveFailedException(typeof(ITestService), null));

        var classFactory = new DiContainerClassFactory<ITestService>(diContainerMock);

        Assert.Throws<ResolveFailedException>(() => classFactory.Create(x => x.Text = "abcd"));
    }
}