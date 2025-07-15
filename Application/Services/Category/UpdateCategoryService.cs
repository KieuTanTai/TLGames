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
    internal class UpdateCategoryService(IDbConnectionFactory connectionFactory) : IUpdateSingleKeyDataService<CategoryModel>
    {
        public async Task<bool> UpdateAsync(CategoryModel entity)
        {
            if (entity == null)
                throw new ArgumentException("Category entity cannot be null.", nameof(entity));
            CategoryDAO category = new(connectionFactory);
            return await category.UpdateAsync(entity);
        }

        public async Task<bool> UpdateManyAsync(IEnumerable<CategoryModel> entities)
        {
            if (entities == null || !entities.Any())
                throw new ArgumentException("Category entities collection cannot be null or empty.", nameof(entities));
            CategoryDAO category = new(connectionFactory);
            return await category.UpdateManyAsync(entities);
        }
    }
}
