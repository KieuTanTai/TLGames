using System.Collections.Generic;
using System.Threading.Tasks;
using TLGames.Core.Entities;
using TLGames.Core.Enums;
using TLGames.Core.Interfaces.IData;
using TLGames.Core.Interfaces.IServices;
using TLGames.Infrastructure.Data;

namespace TLGames.Application.Services.Category
{
    class GetCategoryService(IDbConnectionFactory connectionFactory,
                                        string tableName,
                                        string columnIdName) : ValidateService, IGetDataService<CategoryModel>
    {
        public async Task<List<CategoryModel>> GetAllAsync()
        {
            CategoryDAO category = new(connectionFactory, tableName, columnIdName);
            return await category.GetAllAsync();
        }

        public async Task<CategoryModel> GetByIdAsync(string id)
        {
            CategoryDAO category = new(connectionFactory, tableName, columnIdName);
            return await category.GetByIdAsync(id);
        }

        public async Task<List<CategoryModel>> GetAllByNameAsync(string name, string colName)
        {
            if (!IsValidStringInputDB(colName))
                return new List<CategoryModel>();
            CategoryDAO category = new(connectionFactory, tableName, columnIdName);
            return await category.GetRelativeAsync(name, colName);
        }

        public async Task<List<CategoryModel>> GetAllByStatus(EActiveStatus status, string colName)
        {
            if (!IsValidStringInputDB(colName))
                return new List<CategoryModel>();
            CategoryDAO category = new(connectionFactory, tableName, columnIdName);
            return await category.GetRelativeAsync(status.ToString(), colName);
        }
    }
}
