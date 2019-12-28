using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Interfaces
{
    public interface ISupportsUpdate<T> where T : class
    {
        Task<T> Update(T model);
        Task<IEnumerable<T>> UpdateMany(IEnumerable<T> models);
    }
}