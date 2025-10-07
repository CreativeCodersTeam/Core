using CreativeCoders.DependencyInjection.UnitTests.TestData;
using FakeItEasy;
using AwesomeAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CreativeCoders.DependencyInjection.UnitTests;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void ConfigureServicesFromAssembly_ForCurrentAssembly_TestServiceRegistrationIsCalled()
    {
        var services = A.Fake<IServiceCollection>();

        // Act
        services.ConfigureServicesFromAssembly(typeof(TestServiceRegistration).Assembly);

        // Assert
        TestServiceRegistration.Services
            .Should()
            .BeSameAs(services);
    }
}
