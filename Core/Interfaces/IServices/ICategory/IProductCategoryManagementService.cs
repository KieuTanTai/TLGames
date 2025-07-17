using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLGames.Core.Interfaces.IServices.ICategory
{
    public interface IProductCategoryManagementService<T> where T : class
    {
        Task<List<T>> GetAllProductCategoriesAsync();
        Task<bool> InsertProductCategoryAsync(T productCategory);
        Task<bool> DeleteManyByProductIdAsync(string id);
        Task<bool> DeleteProductCategoryAsync(object ids);
        Task<List<T>> GetAllByIdAsync(string id, string colName);
        Task<T> GetSingleByIdsAsync(object ids);
        Task<bool> UpdateByOldKeyAsync(T productCategory, string oldCategoryId);
        Task<bool> DeleteByIdsAsync(object ids);
    }
}
