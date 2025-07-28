using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<int> DeleteManyAsync(string id)
        {
            CheckNullOrEmpty([id]); // if false it will throw an ArgumentException
            int affectRow = await _productCategoryDAO.DeleteAsync(id);
            if (affectRow <= 0)
                throw new InvalidOperationException("Failed to delete product categories by ProductId.");
            return affectRow;
        }

        public async Task<int> DeleteAsync(object ids)
        {
            if (ids is ProductCategoryItemIds itemIds)
            {
                if (string.IsNullOrEmpty(itemIds.CategoryId) || string.IsNullOrEmpty(itemIds.ProductId))
                    throw new ArgumentException("CategoryId and ProductId cannot be null or empty.", nameof(ids));
                int affectRow = await _deleteByIdAsync.DeleteByIdsAsync(itemIds);
                if (affectRow <= 0)
                    throw new InvalidOperationException("Failed to delete product category.");
                return affectRow;
            }
            throw new ArgumentException("Invalid type for ids. Expected ProductCategoryItemIds.", nameof(ids));
        }

        public async Task<List<ProductCategoryModel>> GetAllByIdAsync(string id, string colName = "product_id")
        {
            CheckNullOrEmpty([id, colName]); // if false it will throw an ArgumentException
            if (IsValidStringInputDB(colName))
                throw new ArgumentException("Invalid column name for retrieval.", nameof(colName));
            return await _getAllByIdAsync.GetAllByIdAsync(id, colName);
        }

        public async Task<List<ProductCategoryModel>> GetAllAsync()
        {
            return await _productCategoryDAO.GetAllAsync();
        }

        public async Task<ProductCategoryModel> GetSingleByIdsAsync(object ids)
        {
            if (ids is ProductCategoryItemIds itemIds)
            {
                if (string.IsNullOrEmpty(itemIds.CategoryId) || string.IsNullOrEmpty(itemIds.ProductId))
                    throw new ArgumentException("CategoryId and ProductId cannot be null or empty.", nameof(ids));
                ProductCategoryModel productCategory = await _getSingleByIdsAsync.GetSingleByIdAsync(itemIds);
                if (productCategory == null)
                    throw new InvalidOperationException("Product category not found for the given IDs.");
                return productCategory;
            }
            throw new Exception("Invalid type for ids. Expected ProductCategoryItemIds.");
        }

        public async Task<int> InsertManyAsync(IEnumerable<ProductCategoryModel> productCategories)
        {
            if (await IsValidEnumerable(productCategories, true))
            {
                int affectRow = await _productCategoryDAO.InsertManyAsync(productCategories);
                if (affectRow <= 0)
                    throw new InvalidOperationException("Failed to insert product categories.");
                return affectRow;
            }
            throw new InvalidOperationException("Product categories already exist with the same ProductId and CategoryId.");
        }

        public async Task<int> InsertAsync(ProductCategoryModel productCategory)
        {
            if (productCategory == null)
                throw new ArgumentNullException(nameof(productCategory), "Product category cannot be null.");
            if (await IsValidEnumerable([productCategory], true))
                throw new InvalidOperationException("Product category already exists with the same ProductId and CategoryId.");
            int affectRow = await _productCategoryDAO.InsertAsync(productCategory);
            if (affectRow <= 0)
                throw new InvalidOperationException("Failed to insert product category.");
            return affectRow;
        }

        public async Task<int> UpdateByOldKeyAsync(ProductCategoryModel productCategory, string oldCategoryId)
        {
            CheckNullOrEmpty([oldCategoryId]); // if false it will throw an ArgumentException
            if (productCategory == null)
                throw new ArgumentNullException(nameof(productCategory), "Product category cannot be null.");
            int affectRow = await _updateByOldKeyAsync.UpdateAsync(productCategory, oldCategoryId);
            if (affectRow <= 0)
                throw new InvalidOperationException("Failed to update product category.");
            return affectRow;
        }

        public async Task<int> UpdateManyByOldKeyAsync(IEnumerable<ProductCategoryModel> productCategories, string oldCategoryId)
        {
            if (await IsValidEnumerable(productCategories, false))
            {
                CheckNullOrEmpty([oldCategoryId]); // if false it will throw an ArgumentException
                int affectRow = await _updateByOldKeyAsync.UpdateAsync(productCategories, oldCategoryId);
                if (affectRow <= 0)
                    throw new InvalidOperationException("Failed to update product categories.");
                return affectRow;
            }
            throw new InvalidOperationException("Product categories already exist with the same ProductId and CategoryId.");
        }

        private async Task<bool> IsValidEnumerable(IEnumerable<ProductCategoryModel> entities, bool checkExist)
        {
            if (entities == null || !entities.Any())
                throw new ArgumentNullException(nameof(entities), "Product category entities cannot be null or empty.");
            foreach (ProductCategoryModel entity in entities)
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity), "Product category entity cannot be null.");

                if (checkExist)
                {
                    ProductCategoryItemIds itemIds = new ProductCategoryItemIds(entity.ProductId.ToString(), entity.CategoryId.ToString());
                    ProductCategoryModel existingObject = await _getSingleByIdsAsync.GetSingleByIdAsync(itemIds);

                    if (existingObject != null)
                        throw new InvalidOperationException($"Product category with Product ID {entity.ProductId} and Category ID {entity.CategoryId} already exists.");
                }
            }
            return true; // Tất cả các kiểm tra đều thành công
        }
    }
}
