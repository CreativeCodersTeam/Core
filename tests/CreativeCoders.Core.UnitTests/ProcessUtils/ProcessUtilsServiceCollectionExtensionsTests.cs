using System;
using System.Linq;
using AwesomeAssertions;
using CreativeCoders.ProcessUtils;
using CreativeCoders.ProcessUtils.Execution;
using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CreativeCoders.Core.UnitTests.ProcessUtils;

/// <summary>
/// Tests for <see cref="ProcessUtilsServiceCollectionExtensions"/> to verify DI registrations,
/// lifetimes, open-generic support and TryAdd semantics.
/// </summary>
public class ProcessUtilsServiceCollectionExtensionsTests
{
    [Fact]
    public void AddProcessUtils_RegistersExpectedServices_WithCorrectLifetimes()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddProcessUtils();
        var provider = services.BuildServiceProvider();

        // Assert
        // IProcessFactory is a singleton of DefaultProcessFactory
        var factory1 = provider.GetRequiredService<IProcessFactory>();
        var factory2 = provider.GetRequiredService<IProcessFactory>();

        factory1
            .Should()
            .BeOfType<DefaultProcessFactory>();

        factory2
            .Should()
            .BeSameAs(factory1);

        using (var scope = provider.CreateScope())
        {
            var factoryScoped = scope.ServiceProvider.GetRequiredService<IProcessFactory>();
            factoryScoped
                .Should()
                .BeSameAs(factory1);
        }

        // IProcessExecutorBuilder is transient of ProcessExecutorBuilder
        var builder1 = provider.GetRequiredService<IProcessExecutorBuilder>();
        var builder2 = provider.GetRequiredService<IProcessExecutorBuilder>();

        builder1
            .Should()
            .BeOfType<ProcessExecutorBuilder>();

        builder2
            .Should()
            .NotBeSameAs(builder1);

        using (var scope = provider.CreateScope())
        {
            var scopedBuilder = scope.ServiceProvider.GetRequiredService<IProcessExecutorBuilder>();
            scopedBuilder
                .Should()
                .BeOfType<ProcessExecutorBuilder>();

            scopedBuilder
                .Should()
                .NotBeSameAs(builder1);
        }

        // Open generic IProcessExecutorBuilder<T> is registered as transient
        var gen1 = provider.GetRequiredService<IProcessExecutorBuilder<int>>();
        var gen2 = provider.GetRequiredService<IProcessExecutorBuilder<int>>();

        gen1
            .Should()
            .BeOfType<ProcessExecutorBuilder<int>>();

        gen2
            .Should()
            .NotBeSameAs(gen1);
    }

    [Fact]
    public void AddProcessUtils_DoesNotOverrideExistingRegistrations()
    {
        // Arrange
        var services = new ServiceCollection();

        var fakeFactory = A.Fake<IProcessFactory>();
        var fakeBuilder = A.Fake<IProcessExecutorBuilder>();
        var fakeGenericBuilderInt = A.Fake<IProcessExecutorBuilder<int>>();

        // Pre-register instances to verify TryAdd semantics
        services.AddSingleton<IProcessFactory>(fakeFactory);
        services.AddSingleton<IProcessExecutorBuilder>(fakeBuilder);
        services.AddSingleton<IProcessExecutorBuilder<int>>(fakeGenericBuilderInt);

        // Act
        services.AddProcessUtils();
        var provider = services.BuildServiceProvider();

        // Assert
        // Existing registrations must be preserved
        provider.GetRequiredService<IProcessFactory>()
            .Should()
            .BeSameAs(fakeFactory);

        provider.GetRequiredService<IProcessExecutorBuilder>()
            .Should()
            .BeSameAs(fakeBuilder);

        provider.GetRequiredService<IProcessExecutorBuilder<int>>()
            .Should()
            .BeSameAs(fakeGenericBuilderInt);

        // Types not pre-registered should be provided by the extension
        var genString = provider.GetRequiredService<IProcessExecutorBuilder<string>>();
        genString
            .Should()
            .BeOfType<ProcessExecutorBuilder<string>>();
    }

    [Fact]
    public void AddProcessUtils_CalledMultipleTimes_IsIdempotent()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddProcessUtils();
        services.AddProcessUtils();

        var provider = services.BuildServiceProvider();

        // Assert
        // Only a single descriptor per service should exist after multiple calls
        services.Count(x => x.ServiceType == typeof(IProcessFactory))
            .Should()
            .Be(1);

        services.Count(x => x.ServiceType == typeof(IProcessExecutorBuilder))
            .Should()
            .Be(1);

        services.Count(x => x.ServiceType.IsGenericType && x.ServiceType.GetGenericTypeDefinition() == typeof(IProcessExecutorBuilder<>))
            .Should()
            .Be(1);

        // Under idempotency, behavior stays the same
        var factory1 = provider.GetRequiredService<IProcessFactory>();
        var factory2 = provider.GetRequiredService<IProcessFactory>();
        factory2
            .Should()
            .BeSameAs(factory1);
    }
}
