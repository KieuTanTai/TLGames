using System.Collections.Generic;
using System.Threading.Tasks;

namespace TLGames.Core.Interfaces.IData
{
    internal interface ICrudOperationsAsync<T> where T : class
    {
#nullable enable
        Task<List<T?>> GetAllAsync();
        Task<T?> GetByIdAsync(string id);
        Task<int> InsertAsync(T entity);
        Task<int> InsertManyAsync(IEnumerable<T> entities);
        Task<bool> UpdateAsync(T entity);
        Task<bool> UpdateManyAsync(IEnumerable<T> entities);
        Task<bool> DeleteAsync(string id);
        Task<bool> DeleteManyAsync(IEnumerable<string> ids);
#nullable disable
    }
}
