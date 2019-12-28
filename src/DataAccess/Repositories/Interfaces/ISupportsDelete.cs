using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Interfaces
{
    public interface ISupportsDelete<T, TKey> where T : class
    {
        Task DeleteByKey(TKey key);
        Task DeleteByKeys(IEnumerable<TKey> keys);
        Task Delete(T model);
        Task DeleteMany(IEnumerable<T> models);
    }
}