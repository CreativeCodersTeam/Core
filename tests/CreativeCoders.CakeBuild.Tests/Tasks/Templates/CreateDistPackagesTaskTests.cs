using Cake.Core.IO;
using CreativeCoders.CakeBuild.Tasks.Templates;
using CreativeCoders.CakeBuild.Tasks.Templates.Settings;
using CreativeCoders.IO.Archives;
using CreativeCoders.IO.Archives.Tar;
using CreativeCoders.IO.Archives.Zip;
using FakeItEasy;
using Xunit;

namespace CreativeCoders.CakeBuild.Tests.Tasks.Templates;

public class CreateDistPackagesTaskTests
{
    private class TestCreateDistPackagesBuildContext(Cake.Core.ICakeContext context)
        : CakeBuildContext(context), ICreateDistPackagesTaskSettings
    {
        public IEnumerable<DistPackage> DistPackages => new[]
        {
            new DistPackage("pkg1", new DirectoryPath("/repo/dist/pkg1")) { Format = DistPackageFormat.Zip },
            new DistPackage("pkg2", new DirectoryPath("/repo/dist/pkg2")) { Format = DistPackageFormat.TarGz }
        };
    }

    [Fact]
    public async Task RunAsync_ValidContext_RunsWithoutError()
    {
        // Arrange
        var cakeContext = CakeTestHelper.CreateCakeContext();
        CakeTestHelper.SetupFileSystem(cakeContext, "/repo", "/repo/test.sln");

        var archiveWriterFactory = A.Fake<IArchiveWriterFactory>();
        var zipWriter = A.Fake<IZipArchiveWriter>();
        var tarWriter = A.Fake<ITarArchiveWriter>();

        A.CallTo(() =>
                archiveWriterFactory.CreateZipWriter(A<string>._, A<System.IO.Compression.CompressionLevel>._,
                    A<bool>._))
            .Returns(zipWriter);
        A.CallTo(() =>
                archiveWriterFactory.CreateTarGzWriter(A<string>._,
                    A<System.IO.Compression.CompressionLevel>._))
            .Returns(tarWriter);

        var context = new TestCreateDistPackagesBuildContext(cakeContext);
        var task = new CreateDistPackagesTask<TestCreateDistPackagesBuildContext>(archiveWriterFactory);

        // Act
        await task.RunAsync(context);

        // Assert
        A.CallTo(() =>
                archiveWriterFactory.CreateZipWriter(A<string>._, A<System.IO.Compression.CompressionLevel>._,
                    A<bool>._))
            .MustHaveHappenedOnceExactly();
        A.CallTo(() =>
                archiveWriterFactory.CreateTarGzWriter(A<string>._,
                    A<System.IO.Compression.CompressionLevel>._))
            .MustHaveHappenedOnceExactly();
    }
}
