using System.Diagnostics.CodeAnalysis;
using AwesomeAssertions;
using System.Reflection;
using System.Reflection.Emit;
using CreativeCoders.Cli.Core;
using CreativeCoders.Cli.Hosting.Commands;
using JetBrains.Annotations;
using Xunit;

namespace CreativeCoders.Cli.Tests.Hosting.Commands;

[SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
public class CommandInfoCreatorTests
{
    private static readonly Lazy<ModuleBuilder>
        __moduleBuilder = new Lazy<ModuleBuilder>(CreateDynamicModule);

    [Fact]
    public void Create_TypeWithoutCliCommandAttribute_ReturnsNull()
    {
        // Arrange
        var sut = new CommandInfoCreator();

        // Act
        var result = sut.Create(typeof(NonCommandType));

        // Assert
        result
            .Should()
            .BeNull();
    }

    [Fact]
    public void Create_NonGenericCommandWithAttribute_ReturnsInfoWithNullOptionsType()
    {
        // Arrange
        var sut = new CommandInfoCreator();
        var type = CreateAttributedType(["simple"], typeof(ICliCommand));

        // Act
        var result = sut.Create(type);

        // Assert
        result
            .Should()
            .NotBeNull();

        result.CommandType
            .Should()
            .Be(type);

        result.CommandAttribute.Commands
            .Should()
            .BeEquivalentTo("simple");

        result.OptionsType
            .Should()
            .BeNull();
    }

    [Fact]
    public void Create_GenericCommandWithAttribute_ReturnsInfoWithCorrectOptionsType()
    {
        // Arrange
        var sut = new CommandInfoCreator();
        var optionsType = typeof(GenericOptions);
        var interfaceType = typeof(ICliCommand<>).MakeGenericType(optionsType);
        var type = CreateAttributedType(["generic", "g"], interfaceType);

        // Act
        var result = sut.Create(type);

        // Assert
        result
            .Should()
            .NotBeNull();

        result.CommandType
            .Should()
            .Be(type);

        result.CommandAttribute.Commands
            .Should()
            .BeEquivalentTo("generic", "g");

        result.OptionsType
            .Should()
            .Be(optionsType);
    }

    [Fact]
    public void Create_AttributedTypeWithoutCommandInterfaces_ThrowsInvalidOperationException()
    {
        // Arrange
        var sut = new CommandInfoCreator();
        var type = CreateAttributedType(["invalid"], null);

        // Act
        var act = () => sut.Create(type);

        // Assert
        act
            .Should()
            .Throw<InvalidOperationException>();
    }

    [UsedImplicitly]
    public sealed class GenericOptions { }

    [UsedImplicitly]
    private sealed class NonCommandType { }

    private static ModuleBuilder CreateDynamicModule()
    {
        var asmName = new AssemblyName($"CC_Cli_Tests_Dyn_{Guid.NewGuid():N}");
        var asmBuilder = AssemblyBuilder.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.Run);
        return asmBuilder.DefineDynamicModule(asmName.Name!);
    }

    private static Type CreateAttributedType(string[] commands, Type? interfaceType)
    {
        var typeName = $"DynCmd_{Guid.NewGuid():N}";
        var tb = __moduleBuilder.Value.DefineType(typeName,
            TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.Abstract);

        if (interfaceType is not null)
        {
            tb.AddInterfaceImplementation(interfaceType);
        }

        var ctor = typeof(CliCommandAttribute).GetConstructor([typeof(string[])])!;
        var attrBuilder = new CustomAttributeBuilder(ctor, [commands]);
        tb.SetCustomAttribute(attrBuilder);

        return tb.CreateTypeInfo().AsType();
    }
}
