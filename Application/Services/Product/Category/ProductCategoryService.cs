using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TLGames.Core.Entities;
using TLGames.Core.Interfaces.IData;
using TLGames.Core.Interfaces.IServices;
using TLGames.Infrastructure.Data;
namespace TLGames.Application.Services.Product.Category
{
    internal class ProductCategoryService(IDbConnectionFactory connectionFactory,
                                        string tableName,
                                        string columnIdName,
                                        string secondColumnIdName) : ValidateService, IUpdateDoubleKeyDataService<ProductCategoryModel>,
                                            IDeleteDoubleKeyDataService, IInsertDataService<ProductCategoryModel>, IGetIntersectionDataService<ProductCategoryModel>
    {
        public async Task<bool> DeleteByKeysAsync(object keys)
        {

            if (keys is not ProductCategoryItemIds productCategoryItem)
                throw new ArgumentException("Keys must be a ProductCategoryItemIds");
            ProductCategoryDAO productCategoryDAO = new(connectionFactory, tableName, columnIdName, secondColumnIdName);
            return await productCategoryDAO.DeleteByIdsAsync(productCategoryItem);
        }

        public Task<List<ProductCategoryModel>> GetAllByIdAsync(string id, string colIdName)
        {
            if (!IsValidStringInputDB(id))
                throw new ArgumentException("Invalid ID input for database query.");
            ProductCategoryDAO productCategoryDAO = new(connectionFactory, tableName, columnIdName, secondColumnIdName);
            return productCategoryDAO.GetAllByIdAsync(id, colIdName);
        }

        public async Task<ProductCategoryModel> GetSingleByIdAsync(object Keys)
        {
            if (Keys is not ProductCategoryItemIds productCategoryItem)
                throw new ArgumentException("Keys must be a ProductCategoryItemIds");
            ProductCategoryDAO productCategoryDAO = new(connectionFactory, tableName, columnIdName, secondColumnIdName);
            return await productCategoryDAO.GetSingleByIdAsync(productCategoryItem);
        }

        public Task<int> InsertAsync(ProductCategoryModel entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null.");
            ProductCategoryDAO productCategoryDAO = new(connectionFactory, tableName, columnIdName, secondColumnIdName);
            return productCategoryDAO.InsertAsync(entity);
        }

        public Task<int> InsertManyAsync(IEnumerable<ProductCategoryModel> entities)
        {
            if (entities == null || entities.Any())
                throw new ArgumentNullException(nameof(entities), "Entities cannot be null or Empty.");
            ProductCategoryDAO productCategoryDAO = new(connectionFactory, tableName, columnIdName, secondColumnIdName);
            return productCategoryDAO.InsertManyAsync(entities);
        }

        public Task<bool> UpdateAsync(ProductCategoryModel entity, string oldKey1)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null.");
            if (!IsValidStringInputDB(oldKey1))
                throw new Exception("Invalid old key input for database query.");
            ProductCategoryDAO productCategoryDAO = new(connectionFactory, tableName, columnIdName, secondColumnIdName);
            return productCategoryDAO.UpdateAsync(entity, oldKey1);
        }
    }
}
