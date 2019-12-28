using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Interfaces
{
    public interface ISupportsGet<T, TKey> where T : class
    {
        Task<T> GetByKey(TKey key);
        Task<IEnumerable<T>> GetAll();
        Task<(IEnumerable<T>, int)> GetFiltered(Expression<Func<T, bool>> filter, int? skip, int? take, bool desc = false);
    }
}