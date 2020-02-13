using System.Collections.Generic;
using Enable.Extensions.FileSystem;

namespace Core.FileSystem
{
    public class BasicFileSystemHub : IFileSystemHub
    {
        private readonly Dictionary<string, IFileSystem> _fileSystems;
        public void Add(string key, IFileSystem fileSystem)
        {
            throw new System.NotImplementedException();
        }

        public IFileSystem Get(string key)
        {
            throw new System.NotImplementedException();
        }
    }
}