using Enable.Extensions.FileSystem;

namespace Core.FileSystem
{
    public interface IFileSystemHub
    {
        IFileSystem Get(string key);
        void Add(string key, IFileSystem fileSystem);
    }
}