using System;
using TLGames.Applications.Services;
using TLGames.Core.Entities;
using TLGames.Core.Interfaces.IData;

namespace TLGames.Infrastructure.Data
{
    internal class CartDAO(IDbConnectionFactory connectionFactory,
                                        string tableName,
                                        string columnIdName) : BaseDAO<CartModel>(connectionFactory, tableName, columnIdName)
    {
        //protected override string TableName => "carts";
        //protected override string ColumnIdName => "cart_id";


        public record CartItemId(string CartId, string CustomerId);

        protected override string GetInsertQuery()
        {
            return $"INSERT INTO {(IsValidStringInputDB(TableName) ? TableName : throw new ArgumentException("error Input"))}(customer_id, total_price) VALUES(@CustomerId, @TotalPrice); SELECT LAST_INSERT_ID();";
        }

        protected override string GetUpdateQuery()
        {
            return $@"UPDATE {(IsValidStringInputDB(TableName) ? TableName : throw new ArgumentException("error Input"))} 
                        SET total_price = @TotalPrice  
                        WHERE {(IsValidStringInputDB(ColumnIdName) ? ColumnIdName : throw new ArgumentException("error Input"))} = @{_converter.SnakeCaseToPascalCase(ColumnIdName)}";
        }

        protected override string DeleteByIdQuery(string colIdName)
        {
            return "";
        }
    }
}
