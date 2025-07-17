using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLGames.Core.Entities;
using TLGames.Core.Interfaces.IData;
using TLGames.Core.Interfaces.IServices.ICategory;
using TLGames.Infrastructure.Data;

namespace TLGames.Application.Services.Category
{
    public class ProductCategoryManagementService : ValidateService<ProductCategoryModel>, IProductCategoryManagementService<ProductCategoryModel>
    {
        private readonly IDAO<ProductCategoryModel> _productCategoryDAO;
        private readonly IGetAllByIdAsync<ProductCategoryModel> _getAllByIdAsync;
        private readonly IGetSingleByIdsAsync<ProductCategoryModel> _getSingleByIdsAsync;
        private readonly IUpdateByOldKeyAsync<ProductCategoryModel> _updateByOldKeyAsync;
        private readonly IDeleteByIdsAsync _deleteByIdAsync;

        public ProductCategoryManagementService(IDAO<ProductCategoryModel> productCategoryDAO, IGetAllByIdAsync<ProductCategoryModel> getAllByIdAsync, 
            IGetSingleByIdsAsync<ProductCategoryModel> getSingleByIdsAsync, IUpdateByOldKeyAsync<ProductCategoryModel> updateByOldKeyAsync, IDeleteByIdsAsync deleteByIdAsync)
            : base(productCategoryDAO)
        {
            _productCategoryDAO = productCategoryDAO;
            _getAllByIdAsync = getAllByIdAsync;
            _getSingleByIdsAsync = getSingleByIdsAsync;
            _updateByOldKeyAsync = updateByOldKeyAsync;
            _deleteByIdAsync = deleteByIdAsync;
        }

        public async Task<bool> DeleteByIdsAsync(object ids)
        {
            if (ids is ProductCategoryItemIds itemIds)
            {
                if (string.IsNullOrEmpty(itemIds.CategoryId) || string.IsNullOrEmpty(itemIds.ProductId))
                    throw new ArgumentException("CategoryId and ProductId cannot be null or empty.", nameof(ids));
                return await _deleteByIdAsync.DeleteByIdsAsync(itemIds);
            }
            throw new ArgumentException("Invalid type for ids. Expected ProductCategoryItemIds.", nameof(ids));
        }

        public async Task<bool> DeleteManyByProductIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id), "Product ID cannot be null or empty.");
            return await _productCategoryDAO.DeleteAsync(id);
        }

        public async Task<bool> DeleteProductCategoryAsync(object ids)
        {
            if (ids is ProductCategoryItemIds itemIds)
            {
                if (string.IsNullOrEmpty(itemIds.CategoryId) || string.IsNullOrEmpty(itemIds.ProductId))
                    throw new ArgumentException("CategoryId and ProductId cannot be null or empty.", nameof(ids));
                return await _deleteByIdAsync.DeleteByIdsAsync(itemIds);
            }
            throw new ArgumentException("Invalid type for ids. Expected ProductCategoryItemIds.", nameof(ids));
        }           

        public async Task<List<ProductCategoryModel>> GetAllByIdAsync(string id, string colName = "product_id")
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id), "Product ID cannot be null or empty.");
            if (string.IsNullOrEmpty(colName))  
                throw new ArgumentException("Column name cannot be null or empty.", nameof(colName));
            if (IsValidStringInputDB(colName))
                throw new ArgumentException("Invalid column name for retrieval.", nameof(colName));
            return await _getAllByIdAsync.GetAllByIdAsync(id, colName);
        }

        public async Task<List<ProductCategoryModel>> GetAllProductCategoriesAsync()
        {
            return await _productCategoryDAO.GetAllAsync();
        }

        public async Task<ProductCategoryModel> GetSingleByIdsAsync(object ids)
        {
            if (ids is ProductCategoryItemIds itemIds)
            {
                if (string.IsNullOrEmpty(itemIds.CategoryId) || string.IsNullOrEmpty(itemIds.ProductId))
                    throw new ArgumentException("CategoryId and ProductId cannot be null or empty.", nameof(ids));
                return await _getSingleByIdsAsync.GetSingleByIdAsync(itemIds);
            }
            throw new Exception("Invalid type for ids. Expected ProductCategoryItemIds.");  
        }

        public async Task<bool> InsertProductCategoryAsync(ProductCategoryModel productCategory)
        {
            if (productCategory == null)
                throw new ArgumentNullException(nameof(productCategory), "Product category cannot be null.");
            int affectRow = await _productCategoryDAO.InsertAsync(productCategory);
            if (affectRow <= 0)
                throw new InvalidOperationException("Failed to insert product category.");
            return affectRow > 0;
        }

        public async Task<bool> InsertProductCategoryAsync(IEnumerable<ProductCategoryModel> productCategories)
        {
            if (productCategories == null || !productCategories.Any())
                throw new ArgumentNullException(nameof(productCategories), "Product categories cannot be null or empty.");
            int affectRow = await _productCategoryDAO.InsertManyAsync(productCategories);
            if (affectRow <= 0)
                throw new InvalidOperationException("Failed to insert product categories.");
            return affectRow > 0;
        }

        public async Task<bool> UpdateByOldKeyAsync(ProductCategoryModel productCategory, string oldCategoryId)
        {
            if (productCategory == null)
                throw new ArgumentNullException(nameof(productCategory), "Product category cannot be null.");
            if (string.IsNullOrEmpty(oldCategoryId))
                throw new ArgumentException("Old category ID cannot be null or empty.", nameof(oldCategoryId));
            bool result = await _updateByOldKeyAsync.UpdateAsync(productCategory, oldCategoryId);
            if (!result)
                throw new InvalidOperationException("Failed to update product category.");
            return result;
        }
    }
}
