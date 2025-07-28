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
        private readonly IDAO<CategoryModel> _categoryDAO;
        private readonly IGetRelativeAsync<CategoryModel> _getRelativeService;
        private readonly IGetDataByEnumAsync<CategoryModel> _getDataByEnumService;

        public CategoryManagementService(IDAO<CategoryModel> categoryDao, IGetRelativeAsync<CategoryModel> getRelativeService, IGetDataByEnumAsync<CategoryModel> getDataByEnumService) : base(categoryDao)
        {
            _categoryDAO = categoryDao;
            _getRelativeService = getRelativeService;
            _getDataByEnumService = getDataByEnumService;
        }

        public async Task<List<CategoryModel>> GetAllAsync()
        {
            return await _categoryDAO.GetAllAsync();
        }

        public async Task<CategoryModel> GetByIdAsync(string id)
        {
            CheckNullOrEmpty([id]); // if false it will throw an ArgumentException
            CategoryModel cateogry = await _categoryDAO.GetByIdAsync(id);
            if (cateogry == null)
                throw new KeyNotFoundException($"Category with ID {id} not found.");
            return cateogry;
        }

        public async Task<List<CategoryModel>> GetRelativeAsync(string input, string colName = "category_name")
        {
            CheckNullOrEmpty([input, colName]); // if false it will throw an ArgumentException
            if (!IsValidStringInputDB(colName))
                throw new ArgumentException("Invalid column name for relative category retrieval.", nameof(colName));
            return await _getRelativeService.GetRelativeAsync(input, colName);
        }

        public async Task<List<CategoryModel>> GetAllByEnumAsync<TEnum>(TEnum status, string colName = "status") where TEnum : Enum
        {
            CheckNullOrEmpty([colName]); // if false it will throw an ArgumentException
            if (status is not EActiveStatus)
                throw new ArgumentException("Invalid enum type for status.", nameof(status));
            if (!Enum.IsDefined(typeof(EActiveStatus), status))
                throw new ArgumentOutOfRangeException(nameof(status), "Invalid status value.");
            if (!IsValidStringInputDB(colName))
                throw new ArgumentException("Invalid column name for enum filtering.", nameof(colName));
            return await _getDataByEnumService.IGetAllByEnumAsync(status, colName);
        }

        public async Task<int> InsertAsync(CategoryModel category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category), "Category cannot be null.");
            if (await IsExistObject(category.CategoryId.ToString()))
                throw new InvalidOperationException($"Category with ID {category.CategoryId} already exists.");
            // Insert category into the database
            int affectRow = await _categoryDAO.InsertAsync(category);
            if (affectRow <= 0)
                throw new InvalidOperationException("Failed to insert category.");
            return affectRow;
        }

        public async Task<int> InsertManyAsync(IEnumerable<CategoryModel> categories)
        {
            if (categories == null)
                throw new ArgumentNullException(nameof(categories), "Categories cannot be null.");
            foreach (var category in categories)
                if (await IsExistObject(category.CategoryId.ToString()))
                    throw new InvalidOperationException($"Category with ID {category.CategoryId} already exists.");
            // Insert categories into the database
            int affectRow = await _categoryDAO.InsertManyAsync(categories);
            if (affectRow <= 0)
                throw new InvalidOperationException("Failed to insert categories.");
            return affectRow;
        }

        public async Task<int> UpdateAsync(CategoryModel category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category), "Category cannot be null.");
            int affectRow = await _categoryDAO.UpdateAsync(category);
            if (affectRow <= 0)
                throw new InvalidOperationException($"Failed to update category with ID {category.CategoryId}.");
            return affectRow;
        }

        public async Task<int> UpdateManyAsync(IEnumerable<CategoryModel> categories)
        {
            if (await IsValidList(categories, false))
            {
                int affectRow = await _categoryDAO.UpdateManyAsync(categories);
                if (affectRow <= 0)
                    throw new InvalidOperationException("Failed to update categories.");
                return affectRow;
            }
            throw new ArgumentException("Invalid category list provided.");
        }

        //public async Task<int> SoftDeleteAsync<TEnum>(string id, TEnum status) where TEnum : Enum
        //{
        //    CheckNullOrEmpty([id]); // if false it will throw an ArgumentException
        //    if (status is EActiveStatus activeStatus)
        //    {
        //        CategoryModel category = await _categoryDAO.GetByIdAsync(id);
        //        if (category == null)
        //            throw new KeyNotFoundException($"category with ID {id} not found.");
        //        category.SetStatus(activeStatus);
        //        int affectRow = await _categoryDAO.UpdateAsync(category);
        //        if (affectRow <= 0)
        //            throw new InvalidOperationException($"Failed to soft delete category with ID {id}.");
        //        return affectRow;
        //    }
        //    throw new ArgumentException("Invalid enum type for category status.", nameof(status));
        //}

        //public async Task<int> SoftDeleteManyAsync<TEnum>(IEnumerable<string> ids, TEnum status) where TEnum : Enum
        //{
        //    CheckNullOrEmpty(ids); // if false it will throw an ArgumentException
        //    if (status is EActiveStatus activeStatus)
        //    {
        //        List<CategoryModel> categoriesToUpdate = new List<CategoryModel>();
        //        foreach (string id in ids)
        //        {
        //            CategoryModel category = await _categoryDAO.GetByIdAsync(id) ?? throw new KeyNotFoundException($"category with ID {id} not found.");
        //            category.SetStatus(activeStatus);
        //            categoriesToUpdate.Add(category);
        //        }
        //        int affectRow = await _categoryDAO.UpdateManyAsync(categoriesToUpdate);
        //        if (affectRow <= 0)
        //            throw new InvalidOperationException("Failed to soft delete one or more banks.");
        //        return affectRow;
        //    }
        //    throw new ArgumentException("Invalid enum type for category status.", nameof(status));
        //}
    }
}
