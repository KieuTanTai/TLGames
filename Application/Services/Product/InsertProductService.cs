using System.Collections.Generic;
using System.Threading.Tasks;
using TLGames.Core.Entities;
using TLGames.Core.Interfaces.IData;
using TLGames.Core.Interfaces.IServices;
using TLGames.Infrastructure.Data;

namespace TLGames.Application.Services.Product
{
    internal class InsertProductService(IDbConnectionFactory connectionFactory) : IInsertDataService<ProductModel>
    {
        public async Task<int> InsertAsync(ProductModel entity)
        {
            ProductDAO product = new(connectionFactory);
            return await product.InsertAsync(entity);
        }

        public async Task<int> InsertManyAsync(IEnumerable<ProductModel> entities)
        {
            ProductDAO product = new(connectionFactory);
            return await product.InsertManyAsync(entities);
        }
    }
}
