using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System;
using System.Threading.Tasks;
using TLGames.Applications.Services;
using TLGames.Core.Entities;
using TLGames.Core.Interfaces;
using TLGames.Infrastructure.Services;
using static Dapper.SqlMapper;

namespace TLGames.Infrastructure.Data
{
    internal class CartDAO(IDbConnectionFactory connectionFactory) : BaseDAO<CartModel>(connectionFactory)
    {
        protected override string TableName => "carts";
        protected override string ColumnIdName => "cart_id";
        private readonly IStringConverter _converter = App.SystemServices.GetService<IStringConverter>();

        public record CartItemId(string CartId, string CustomerId);

        protected override string GetInsertQuery()
        {
            return $"INSERT INTO {TableName}(customer_id, total_price) VALUES(@CustomerId, @TotalPrice); SELECT LAST_INSERT_ID();";
        }

        protected override string GetUpdateQuery()
        {
            return $@"UPDATE {TableName} 
                        SET total_price = @TotalPrice  
                        WHERE {ColumnIdName} = @{_converter.SnakeCaseToPascalCase(ColumnIdName)}";
        }

        protected override string DeleteByIdQuery(string colIdName)
        {
            return "";
        }
    }
}
