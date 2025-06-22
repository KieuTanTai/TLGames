using System.Collections.Generic;
using System.Threading.Tasks;

namespace TLGames.Core.Interfaces
{
    internal interface ICrudOperationsAsync<T> where T : class
    {
#nullable enable
        Task<List<T?>> GetAllAsync();
        Task<T?> GetByIdAsync(string id);
        Task<int> InsertAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(string id);
#nullable disable
    }
}
