using System.Collections.Generic;
using Enable.Extensions.FileSystem;

namespace Core.FileSystem
{
    public class BasicFileSystemHub : IFileSystemHub
    {
        private readonly Dictionary<string, IFileSystem> _fileSystems;

        public BasicFileSystemHub()
        {
            _fileSystems = new Dictionary<string, IFileSystem>();
        }
        
        public void Add(string key, IFileSystem fileSystem)
        {
            _fileSystems.Add(key, fileSystem);
        }

        public IFileSystem Get(string key)
        {
            return _fileSystems.TryGetValue(key, out var fileSystem) ? fileSystem : null;
        }
    }
}