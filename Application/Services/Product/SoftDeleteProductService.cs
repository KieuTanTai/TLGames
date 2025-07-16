using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TLGames.Core.Entities;
using TLGames.Core.Enums;
using TLGames.Core.Interfaces.IData;
using TLGames.Core.Interfaces.IServices;
using TLGames.Infrastructure.Data;

namespace TLGames.Application.Services.Product
{
    internal class SoftDeleteProductService(IDbConnectionFactory connectionFactory,
                                        string tableName,
                                        string columnIdName) : ISoftDeleteService
    {
        public async Task<bool> SoftDeleteByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id), "Product ID cannot be null or empty.");
            ProductDAO productDAO = new(connectionFactory, tableName, columnIdName);
            ProductModel product = await productDAO.GetByIdAsync(id);

            if (product == null) return false;
            product.SetStatus(EActiveStatus.INACTIVE);
            return await productDAO.SoftDeleteAsync(product);
        }

        public async Task<bool> SoftDeleteByIdsAsync(IEnumerable<string> ids)
        {
            if (ids == null || !ids.Any())
                throw new ArgumentNullException(nameof(ids), "Product IDs cannot be null or empty.");
            ProductDAO productDAO = new(connectionFactory, tableName, columnIdName);
            List<ProductModel> products = new();
            foreach (string id in ids)
            {
                ProductModel product = await productDAO.GetByIdAsync(id);
                if (product != null)
                {
                    product.SetStatus(EActiveStatus.INACTIVE);
                    products.Add(product);
                }
            }
            if (products == null || products.Count == 0)
                return false;
            return await productDAO.UpdateManyAsync(products);
        }
    }
}
