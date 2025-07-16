using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TLGames.Core.Entities;
using TLGames.Core.Interfaces.IData;
using TLGames.Core.Interfaces.IServices;
using TLGames.Infrastructure.Data;

namespace TLGames.Application.Services.Product.Description
{
    internal class ProductDescriptionService(IDbConnectionFactory connectionFactory,
                                        string tableName,
                                        string columnIdName) : ValidateService, IUpdateSingleKeyDataService<ProductDescriptionModel>, IDeleteSingleKeyDataService<ProductDescriptionModel>,
                                        IInsertDataService<ProductDescriptionModel>, IGetDataService<ProductDescriptionModel>
    {
        public Task<bool> DeleteByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id), "ID cannot be null or empty.");
            ProductDescriptionDAO productDescriptionDAO = new(connectionFactory, tableName, columnIdName);
            return productDescriptionDAO.DeleteAsync(id);
        }

        public Task<bool> DeleteByIdsAsync(IEnumerable<string> ids)
        {
            if (ids == null || !ids.Any())
                throw new ArgumentNullException(nameof(ids), "IDs cannot be null or empty.");
            ProductDescriptionDAO productDescriptionDAO = new(connectionFactory, tableName, columnIdName);
            return productDescriptionDAO.DeleteManyAsync(ids);
        }

        public Task<List<ProductDescriptionModel>> GetAllAsync()
        {
            ProductDescriptionDAO productDescriptionDAO = new(connectionFactory, tableName, columnIdName);
            return productDescriptionDAO.GetAllAsync();
        }

        public Task<List<ProductDescriptionModel>> GetAllByNameAsync(string name, string colName)
        {
            if(!IsValidStringInputDB(colName))
                throw new ArgumentException("Invalid name input for database query.");
            ProductDescriptionDAO productDescriptionDAO = new(connectionFactory, tableName, columnIdName);
            return productDescriptionDAO.GetRelativeAsync(name, colName);
        }

        public Task<ProductDescriptionModel> GetByIdAsync(string id)
        {
            if(string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id), "ID cannot be null or empty.");
            ProductDescriptionDAO productDescriptionDAO = new(connectionFactory, tableName, columnIdName);
            return productDescriptionDAO.GetByIdAsync(id);
        }

        public Task<int> InsertAsync(ProductDescriptionModel entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null.");
            ProductDescriptionDAO productDescriptionDAO = new(connectionFactory, tableName, columnIdName);
            return productDescriptionDAO.InsertAsync(entity);
        }
        public Task<int> InsertManyAsync(IEnumerable<ProductDescriptionModel> entities)
        {
            if (entities == null || !entities.Any())
                throw new ArgumentNullException(nameof(entities), "Entities cannot be null or empty.");
            ProductDescriptionDAO productDescriptionDAO = new(connectionFactory, tableName, columnIdName);
            return productDescriptionDAO.InsertManyAsync(entities);
        }
        public Task<bool> UpdateAsync(ProductDescriptionModel entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null.");
            ProductDescriptionDAO productDescriptionDAO = new(connectionFactory, tableName, columnIdName);
            return productDescriptionDAO.UpdateAsync(entity);
        }
    }
}
