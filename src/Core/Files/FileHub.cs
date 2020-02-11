using Core.Files.Interfaces;
using Enable.Extensions.FileSystem;

namespace Core.Files
{
    public class FileHub : IFileHub
    {
        public virtual IFileSystem Uploads { get; private set; }
        public virtual IFileSystem WebImages { get; private set; }
        public virtual IFileSystem WebThumbnails { get; private set; }
    }
}