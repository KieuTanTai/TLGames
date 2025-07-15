using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TLGames.Core.Entities;
using TLGames.Core.Interfaces.IData;
using TLGames.Core.Interfaces.IServices;
using TLGames.Infrastructure.Data;

namespace TLGames.Application.Services.Category
{
    internal class InsertCategoryService(IDbConnectionFactory connectionFactory) : IInsertDataService<CategoryModel>
    {
        public async Task<int> InsertAsync(CategoryModel entity)
        {
            if (entity == null)
                throw new ArgumentException("Category entity cannot be null.", nameof(entity));
            CategoryDAO category = new CategoryDAO(connectionFactory);
            return await category.InsertAsync(entity);
        }

        public async Task<int> InsertManyAsync(IEnumerable<CategoryModel> entities)
        {
            if (entities == null || !entities.Any())
                throw new ArgumentException("Category entities collection cannot be null or empty.", nameof(entities));
            CategoryDAO category = new CategoryDAO(connectionFactory);
            return await category.InsertManyAsync(entities);
        }
    }
}
