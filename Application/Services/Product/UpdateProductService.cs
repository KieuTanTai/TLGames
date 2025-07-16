using System;
using System.Threading.Tasks;
using TLGames.Core.Entities;
using TLGames.Core.Interfaces.IData;
using TLGames.Core.Interfaces.IServices;
using TLGames.Infrastructure.Data;

namespace TLGames.Application.Services.Product
{
    internal class UpdateProductService(IDbConnectionFactory connectionFactory,
                                        string tableName,
                                        string columnIdName) : IUpdateSingleKeyDataService<ProductModel>
    {
        public async Task<bool> UpdateAsync(ProductModel entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Product entity cannot be null.");
            ProductDAO product = new(connectionFactory, tableName, columnIdName);
            return await product.UpdateAsync(entity);
        }
    }
}
