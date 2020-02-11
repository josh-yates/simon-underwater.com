using Enable.Extensions.FileSystem;

namespace Core.Files.Interfaces
{
    public interface IFileHub
    {
        IFileSystem Uploads { get; }
        IFileSystem WebImages { get; }
        IFileSystem WebThumbnails { get; }
    }
}