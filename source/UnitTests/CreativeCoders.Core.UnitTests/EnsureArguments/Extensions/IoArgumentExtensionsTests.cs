using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using CreativeCoders.Core.IO;
using CreativeCoders.UnitTests;
using FluentAssertions;
using Xunit;

#nullable enable
namespace CreativeCoders.Core.UnitTests.EnsureArguments.Extensions
{
    [Collection("FileSys")]
    public class IoArgumentExtensionsTests
    {
        [Fact]
        public void FileExists_ExistingFileName_ReturnsArgument()
        {
            const string fileName = @"C:\temp_dir\test.txt";

            var mockFileSystem = new MockFileSystemEx(
                new Dictionary<string, MockFileData>
                {
                    {fileName, MockFileData.NullObject}
                },
                @"C:\");

            mockFileSystem.Install();

            // Act
            var argument = Ensure.Argument(fileName, nameof(fileName)).FileExists();

            // Assert
            argument
                .Should()
                .BeOfType<ArgumentNotNull<string>>();

            argument.Value
                .Should()
                .Be(fileName);
        }

        [Fact]
        public void FileExists_NotExistingFileName_ThrowsException()
        {
            const string fileName = @"C:\temp_dir\test.txt";

            var mockFileSystem = new MockFileSystemEx(
                new Dictionary<string, MockFileData>(),
                @"C:\");

            mockFileSystem.Install();

            // Act
            Action act = () => Ensure.Argument(fileName, nameof(fileName)).FileExists();

            // Assert
            act
                .Should()
                .Throw<FileNotFoundException>();
        }

        [Fact]
        public void FileExists_ArgNotNullExistingFileName_ReturnsArgument()
        {
            const string fileName = @"C:\temp_dir\test.txt";

            var mockFileSystem = new MockFileSystemEx(
                new Dictionary<string, MockFileData>
                {
                    {fileName, MockFileData.NullObject}
                },
                @"C:\");

            mockFileSystem.Install();

            // Act
            var argument = Ensure.Argument(fileName, nameof(fileName)).NotNull().FileExists();

            // Assert
            argument
                .Should()
                .BeOfType<ArgumentNotNull<string>>();

            argument.Value
                .Should()
                .Be(fileName);
        }

        [Fact]
        public void FileExists_ArgNotNullNotExistingFileName_ThrowsException()
        {
            const string fileName = @"C:\temp_dir\test.txt";

            var mockFileSystem = new MockFileSystemEx(
                new Dictionary<string, MockFileData>(),
                @"C:\");

            mockFileSystem.Install();

            // Act
            Action act = () => Ensure.Argument(fileName, nameof(fileName)).NotNull().FileExists();

            // Assert
            act
                .Should()
                .Throw<FileNotFoundException>();
        }

        [Fact]
        public void FileExists_NullFileName_ThrowsException()
        {
            const string? fileName = null;

            var mockFileSystem = new MockFileSystemEx(
                new Dictionary<string, MockFileData>(),
                @"C:\");

            mockFileSystem.Install();

            // Act
            Action act = () => Ensure.Argument(fileName, nameof(fileName)).FileExists();

            // Assert
            act
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public void DirectoryExists_ExistingDirectoryName_ReturnsArgument()
        {
            const string fileName = @"C:\temp_dir\test.txt";
            var directoryName = FileSys.Path.GetDirectoryName(fileName);

            var mockFileSystem = new MockFileSystemEx(
                new Dictionary<string, MockFileData>
                {
                    {fileName, MockFileData.NullObject}
                },
                @"C:\");

            mockFileSystem.Install();

            // Act
            var argument = Ensure.Argument(directoryName, nameof(directoryName)).DirectoryExists();

            // Assert
            argument
                .Should()
                .BeOfType<ArgumentNotNull<string>>();

            argument.Value
                .Should()
                .Be(directoryName);
        }

        [Fact]
        public void DirectoryExists_NotExistingDirectoryName_ThrowsException()
        {
            const string directoryName = @"C:\Temp12";

            var mockFileSystem = new MockFileSystemEx(
                new Dictionary<string, MockFileData>(),
                @"C:\");

            mockFileSystem.Install();

            // Act
            Action act = () => Ensure.Argument(directoryName, nameof(directoryName)).DirectoryExists();

            // Assert
            act
                .Should()
                .Throw<DirectoryNotFoundException>();
        }

        [Fact]
        public void DirectoryExists_ArgNotNullExistingDirectoryName_ReturnsArgument()
        {
            const string fileName = @"C:\temp_dir\test.txt";
            var directoryName = FileSys.Path.GetDirectoryName(fileName);

            var mockFileSystem = new MockFileSystemEx(
                new Dictionary<string, MockFileData>
                {
                    {fileName, MockFileData.NullObject}
                },
                @"C:\");

            mockFileSystem.Install();

            // Act
            var argument = Ensure.Argument(directoryName, nameof(directoryName)).NotNull().DirectoryExists();

            // Assert
            argument
                .Should()
                .BeOfType<ArgumentNotNull<string>>();

            argument.Value
                .Should()
                .Be(directoryName);
        }

        [Fact]
        public void DirectoryExists_ArgNotNullNotExistingDirectoryName_ThrowsException()
        {
            const string directoryName = @"C:\Temp12";

            var mockFileSystem = new MockFileSystemEx(
                new Dictionary<string, MockFileData>(),
                @"C:\");

            mockFileSystem.Install();

            // Act
            Action act = () => Ensure.Argument(directoryName, nameof(directoryName)).NotNull().DirectoryExists();

            // Assert
            act
                .Should()
                .Throw<DirectoryNotFoundException>();
        }

        [Fact]
        public void DirectoryExists_NullDirectoryName_ThrowsException()
        {
            const string? directoryName = null;

            var mockFileSystem = new MockFileSystemEx(
                new Dictionary<string, MockFileData>(),
                @"C:\");

            mockFileSystem.Install();

            // Act
            Action act = () => Ensure.Argument(directoryName, nameof(directoryName)).DirectoryExists();

            // Assert
            act
                .Should()
                .Throw<ArgumentNullException>();
        }
    }
}
