using System.Collections.Generic;
using System.Threading.Tasks;
using TLGames.Core.Enums;

namespace TLGames.Core.Interfaces.IServices.ICategory
{
    public interface ICategoryManagementService<T> where T : class
    {
        Task<List<T>> GetAllCategoriesAsync();
        Task<T> GetCategoryByIdAsync(string id);
        Task<bool> InsertCategoryAsync(T category);
        Task<bool> UpdateCategoryAsync(T category);
        Task<bool> SoftDeleteCategoryAsync(T category);
        Task<List<T>> GetDataByEnumAsync(EActiveStatus status, string colName);
    }
}
