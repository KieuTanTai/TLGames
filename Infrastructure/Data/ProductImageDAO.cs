using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System;
using TLGames.Applications.Services;
using TLGames.Core.Entities;
using TLGames.Core.Interfaces;
using TLGames.Infrastructure.Services;
using Dapper;

namespace TLGames.Infrastructure.Data
{
    internal class ProductImageDAO(IDbConnectionFactory connectionFactory) : BaseDAO<ProductImageModel>(connectionFactory), IGetAllByIdAsync<ProductImageModel>
    {
        protected override string TableName => "product_images";

        protected override string ColumnIdName => "product_image_id";
        private readonly IStringConverter _converter = App.SystemServices.GetService<IStringConverter>();

        protected override string GetInsertQuery()
        {
            return $@"INSERT INTO {TableName} (product_id, image_url) 
                        VALUES(@ProductId, @ImageUrl); SELECT LAST_INSERT_ID();";
        }

        protected override string GetUpdateQuery()
        {
            return $@"UPDATE {TableName}
                        SET image_url = @ImageUrl
                        WHERE {ColumnIdName} = @{_converter.SnakeCaseToPascalCase(ColumnIdName)}";
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
