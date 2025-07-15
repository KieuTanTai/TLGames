using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TLGames.Applications.Services;
using TLGames.Core.Entities;
using TLGames.Core.Interfaces.IData;

namespace TLGames.Infrastructure.Data
{
    internal class ProductImageDAO(IDbConnectionFactory connectionFactory) : BaseDAO<ProductImageModel>(connectionFactory), IGetAllByIdAsync<ProductImageModel>
    {
        protected override string TableName => "product_images";

        protected override string ColumnIdName => "product_image_id";


        protected override string GetInsertQuery()
        {
            return $@"INSERT INTO {(IsValidStringInputDB(TableName) ? TableName : throw new ArgumentException("error Input"))} (product_id, image_url) 
                        VALUES(@ProductId, @ImageUrl); SELECT LAST_INSERT_ID();";
        }

        protected override string GetUpdateQuery()
        {
            return $@"UPDATE {(IsValidStringInputDB(TableName) ? TableName : throw new ArgumentException("error Input"))}
                        SET image_url = @ImageUrl
                        WHERE {(IsValidStringInputDB(ColumnIdName) ? ColumnIdName : throw new ArgumentException("error Input"))} = @{_converter.SnakeCaseToPascalCase(ColumnIdName)}";
        }

        public async Task<List<ProductImageModel>> GetAllByIdAsync(string id, string colIdName)
        {
            try
            {
                string query = GetByIdQuery(colIdName);
                using IDbConnection connection = connectionFactory.CreateConnection();
                IEnumerable<ProductImageModel> result = await connection.QueryAsync<ProductImageModel>(query, new { Id = id });
                return result.AsList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return new();
            }
        }
    }
}
