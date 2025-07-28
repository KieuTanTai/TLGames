using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TLGames.Core.Entities;
using TLGames.Core.Enums;
using TLGames.Core.Interfaces.IData;
using TLGames.Core.Interfaces.IValidate;
using TLGames.Infrastructure.Persistence;

namespace TLGames.Infrastructure.Data
{
    public record CartItemId(string CartId, string CustomerId);
    public class CartDAO(IDbConnectionFactory connectionFactory, IColumnService colService, IStringConverter converter, IStringChecker checker)
        : BaseDAO<CartModel>(connectionFactory, colService, converter, checker, "carts", "cart_id", null)
    {
        protected override string GetInsertQuery()
        {
            return $"INSERT INTO {TableName}(customer_id, total_price) VALUES(@CustomerId, @TotalPrice); SELECT LAST_INSERT_ID();";
        }

        protected override string GetUpdateQuery()
        {
            return $@"UPDATE {TableName} 
                        SET total_price = @TotalPrice  
                        WHERE {ColumnIdName} = @{Converter.SnakeCaseToPascalCase(ColumnIdName)}";
        }

        protected override string DeleteByIdQuery(string colIdName)
        {
            return ""; // Soft delete is handled in DeleteAsync
        }

        public override Task<int> DeleteAsync(string id)
        {
            return Task.FromResult(-1); // Cart deletion is not allowed, as it may contain items     
        }

        public override Task<int> DeleteManyAsync(IEnumerable<string> ids)
        {
            return Task.FromResult(-1); // Cart deletion is not allowed, as it may contain items 
        }
    }
}
