using System;
using TLGames.Core.Entities;
using TLGames.Core.Interfaces.IData;
using TLGames.Core.Interfaces.IValidate;

namespace TLGames.Infrastructure.Data
{
    public record CartItemId(string CartId, string CustomerId);
    internal class CartDAO(IDbConnectionFactory connectionFactory, IColumnService colService, IStringConverter converter, IStringChecker checker)
        : BaseDAO<CartModel>(connectionFactory, colService, converter, checker, "carts", "cart_id", null)
    {
        protected override string GetInsertQuery()
        {
            return $"INSERT INTO {(IsValidStringInputDB(TableName) ? TableName : throw new ArgumentException("error Input"))}(customer_id, total_price) VALUES(@CustomerId, @TotalPrice); SELECT LAST_INSERT_ID();";
        }

        protected override string GetUpdateQuery()
        {
            return $@"UPDATE {(IsValidStringInputDB(TableName) ? TableName : throw new ArgumentException("error Input"))} 
                        SET total_price = @TotalPrice  
                        WHERE {(IsValidStringInputDB(ColumnIdName) ? ColumnIdName : throw new ArgumentException("error Input"))} = @{Converter.SnakeCaseToPascalCase(ColumnIdName)}";
        }

        protected override string DeleteByIdQuery(string colIdName)
        {
            return "";
        }
    }
}
