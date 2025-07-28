using System.Collections.Generic;
using System.Threading.Tasks;

namespace TLGames.Core.Interfaces.IServices.ICategory
{
    public interface IProductCategoryManagementService<T> : IBaseLinkingDataService<T> where T : class
    {
        Task<int> UpdateByOldKeyAsync(T productCategory, string oldCategoryId);
        Task<int> UpdateManyByOldKeyAsync(IEnumerable<T> productCategories, string oldCategoryId);
        Task<int> DeleteManyAsync(string id);
        Task<int> DeleteAsync(object ids);
    }
}
