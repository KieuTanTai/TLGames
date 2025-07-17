using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TLGames.Core.Entities;
using TLGames.Core.Enums;
using TLGames.Core.Interfaces.IData;
using TLGames.Core.Interfaces.IServices.ICategory;

namespace TLGames.Application.Services.Category
{
    public class CategoryManagementService : ValidateService<CategoryModel>, ICategoryManagementService<CategoryModel>
    {
        private readonly IDAO<CategoryModel> _categoryDao;
        private readonly IGetRelativeAsync<CategoryModel> _getRelativeService;
        private readonly ISoftDeleteAsync<CategoryModel> _softDeleteService;
        private readonly IGetDataByEnum<CategoryModel> _getDataByEnumService;

        public CategoryManagementService(IDAO<CategoryModel> categoryDao, IGetRelativeAsync<CategoryModel> getRelativeService,
            ISoftDeleteAsync<CategoryModel> softDeleteService, IGetDataByEnum<CategoryModel> getDataByEnumService) : base(categoryDao)
        {
            _categoryDao = categoryDao;
            _getRelativeService = getRelativeService;
            _softDeleteService = softDeleteService;
            _getDataByEnumService = getDataByEnumService;
        }

        public async Task<List<CategoryModel>> GetAllCategoriesAsync()
        {
            return await _categoryDao.GetAllAsync();
        }

        public async Task<CategoryModel> GetCategoryByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id), "Category ID cannot be null or empty.");
            return await _categoryDao.GetByIdAsync(id);
        }

        public async Task<List<CategoryModel>> GetRelativeCategoryAsync(string input, string colName)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentNullException(nameof(input), "Input value cannot be null or empty.");
            if (string.IsNullOrEmpty(colName))
                throw new ArgumentException("Column name cannot be null or empty.", nameof(colName));
            if (!IsValidStringInputDB(colName))
                throw new ArgumentException("Invalid column name for relative category retrieval.", nameof(colName));
            return await _getRelativeService.GetRelativeAsync(input, colName);
        }

        public async Task<List<CategoryModel>> GetDataByEnumAsync(EActiveStatus status, string colName = "status")
        {
            if (string.IsNullOrEmpty(colName))
                throw new ArgumentException("Column name cannot be null or empty.", nameof(colName));
            if (!Enum.IsDefined(typeof(EActiveStatus), status))
                throw new ArgumentOutOfRangeException(nameof(status), "Invalid status value.");
            if (!IsValidStringInputDB(colName))
                throw new ArgumentException("Invalid column name for enum filtering.", nameof(colName));
            return await _getDataByEnumService.GetAllByEnum(status, colName);
        }

        public async Task<bool> InsertCategoryAsync(CategoryModel category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category), "Category cannot be null.");
            if (!IsValidStringInputDB(category.CategoryName))
                throw new ArgumentException("Invalid category name.", nameof(category.CategoryName));
            if (await IsExistObject(category.CategoryId.ToString()))
                throw new InvalidOperationException($"Category with ID {category.CategoryId} already exists.");
            // Insert category into the database
            int affectRow = await _categoryDao.InsertAsync(category);
            if (affectRow <= 0)
                throw new InvalidOperationException("Failed to insert category.");
            return true;
        }

        public async Task<bool> InsertCategoryAsync(IEnumerable<CategoryModel> categories)
        {
            if (categories == null)
                throw new ArgumentNullException(nameof(categories), "Categories cannot be null.");
            foreach (var category in categories)
            {
                if (!IsValidStringInputDB(category.CategoryName))
                    throw new ArgumentException("Invalid category name.", nameof(category.CategoryName));
                if (await IsExistObject(category.CategoryId.ToString()))
                    throw new InvalidOperationException($"Category with ID {category.CategoryId} already exists.");
            }
            // Insert categories into the database
            int affectRow = await _categoryDao.InsertManyAsync(categories);
            if (affectRow <= 0)
                throw new InvalidOperationException("Failed to insert categories.");
            return true;
        }

        public async Task<bool> SoftDeleteCategoryAsync(CategoryModel category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category), "Category cannot be null.");
            if (!IsValidStringInputDB(category.CategoryName))
                throw new ArgumentException("Invalid category name.", nameof(category.CategoryName));
            bool result = await _softDeleteService.SoftDeleteAsync(category);
            if (!result)
                throw new InvalidOperationException("Failed to soft delete category.");
            return result;
        }

        public async Task<bool> UpdateCategoryAsync(CategoryModel category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category), "Category cannot be null.");
            if (!IsValidStringInputDB(category.CategoryName))
                throw new ArgumentException("Invalid category name.", nameof(category.CategoryName));
            bool result = await _categoryDao.UpdateAsync(category);
            if (!result)
                throw new InvalidOperationException("Failed to update category.");
            return result;
        }

        // TODO: NEED INTERFACE FOR UPDATE AND DELETE
    }
}
