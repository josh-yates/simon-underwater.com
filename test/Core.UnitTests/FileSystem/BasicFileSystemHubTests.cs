using Core.FileSystem;
using Moq;
using Xunit;
using Enable.Extensions.FileSystem;
using AutoFixture;

namespace Core.UnitTests.FileSystem
{
    public class BasicFileSystemHubTests
    {
        private readonly Fixture _fixture;
        private readonly BasicFileSystemHub _sut;

        public BasicFileSystemHubTests()
        {
            _sut = new BasicFileSystemHub();
            _fixture =  new Fixture();
        }

        [Fact]
        public void AddAndGet_WorkAsExpected()
        {
            // Arrange
            var key = _fixture.Create<string>();
            var fileSystem = (new Mock<IFileSystem>()).Object;

            _sut.Add(key, fileSystem);

            // Act
            var fileSystemOut = _sut.Get(key);

            // Assert
            Assert.Equal(fileSystem, fileSystemOut);
        }

        [Fact]
        public void Get_ReturnsNullIfFilesystemNotAdded()
        {
            // Arrange
            var key = _fixture.Create<string>();

            // Act
            var fileSystemOut = _sut.Get(key);

            // Assert
            Assert.Null(fileSystemOut);
        }
    }
}