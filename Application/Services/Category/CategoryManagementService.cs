using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TLGames.Core.Entities;
using TLGames.Core.Interfaces.IData;

namespace TLGames.Application.Services.Category
{
    internal class CategoryManagementService
    {
        private readonly IDAO<CategoryModel> _categoryDao;
        private readonly IGetRelativeAsync<CategoryModel> _getRelativeService;
        private readonly ISoftDeleteAsync<CategoryModel> _softDeleteService;
        private readonly IGetDataByEnum<CategoryModel> _getDataByEnumService;

        public CategoryManagementService(
            IDAO<CategoryModel> categoryDao,
            IGetRelativeAsync<CategoryModel> getRelativeService, // Được inject từ CategoryDAO
            ISoftDeleteAsync<CategoryModel> softDeleteService,   // Được inject từ CategoryDAO
            IGetDataByEnum<CategoryModel> getDataByEnumService   // Được inject từ CategoryDAO
            )
        {
            _categoryDao = categoryDao;
            _getRelativeService = getRelativeService;
            _softDeleteService = softDeleteService;
            _getDataByEnumService = getDataByEnumService;
        }

        public async Task<List<CategoryModel>> GetAllCategoriesAsync()
        {
            return (await _categoryDao.GetAllAsync()).ToList();
        }

        public async Task<CategoryModel?> GetCategoryByIdAsync(string id)
        {
            return await _categoryDao.GetByIdAsync(id);
        }

        public async Task<bool> AddNewCategoryAsync(CategoryModel category)
        {
            if (category == null) throw new ArgumentNullException(nameof(category));
            // Thêm logic nghiệp vụ ở đây (ví dụ: validate dữ liệu, kiểm tra trùng lặp)
            int rowsAffected = await _categoryDao.InsertAsync(category);
            return rowsAffected > 0;
        }

        public async Task<bool> UpdateCategoryAsync(CategoryModel category)
        {
            if (category == null) throw new ArgumentNullException(nameof(category));
            // Thêm logic nghiệp vụ
            bool result = await _categoryDao.UpdateAsync(category);
            return result;
        }

        public async Task<bool> DeleteCategoryByIdAsync(string id)
        {
            // Có thể dùng SoftDelete thay vì Delete cứng nếu bạn muốn
            // return await _categoryDao.DeleteByIdAsync(id) > 0;
            var category = await _categoryDao.GetByIdAsync(id);
            if (category == null) return false;
            return await _softDeleteService.SoftDeleteAsync(category);
        }

        public async Task<List<CategoryModel>> SearchCategoriesByNameAsync(string namePart)
        {
            // Sử dụng interface chuyên biệt
            return await _getRelativeService.GetRelativeAsync(namePart, "name");
        }

        // Ví dụ: Lấy danh mục theo trạng thái (nếu CategoryModel có trường Status)
        public enum CategoryStatus { Active, Inactive, Archived }
        public async Task<List<CategoryModel>> GetCategoriesByStatusAsync(CategoryStatus status)
        {
            // Sử dụng interface chuyên biệt
            return await _getDataByEnumService.GetAllByEnum(status);
        }
    }
}
