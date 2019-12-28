using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Interfaces
{
    public interface ISupportsCreate<T> where T : class
    {
        Task<T> Create(T model);
        Task<IEnumerable<T>> CreateMany(IEnumerable<T> models);
    }
}