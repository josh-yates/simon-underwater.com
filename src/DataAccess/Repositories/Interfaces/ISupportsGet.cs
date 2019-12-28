using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Interfaces
{
    public interface ISupportsGet<T, TKey>
    {
        Task<T> GetByKey(TKey key);
        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> GetPaged(int skip, int take);
    }
}