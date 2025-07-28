using System.Collections.Generic;
using System.Threading.Tasks;

namespace TLGames.Core.Interfaces.IServices
{
    public interface IBaseLinkingDataService<T> where T : class
    {
        Task<T> GetSingleByIdsAsync(object ids);
        Task<List<T>> GetAllAsync();
        Task<List<T>> GetAllByIdAsync(string id, string colName);
        Task<int> InsertAsync(T productCategory);
        Task<int> InsertManyAsync(IEnumerable<T> productCategories);
    }
}
