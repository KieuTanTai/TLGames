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
    internal class ProductDescriptionImageDAO(IDbConnectionFactory connectionFactory) : BaseDAO<ProductDescriptionImageModel>(connectionFactory), IGetAllByIdAsync<ProductDescriptionImageModel>
    {
        protected override string TableName => "description_images";

        protected override string ColumnIdName => "description_image_id";
        private readonly IStringConverter _converter = App.SystemServices.GetService<IStringConverter>();

        protected override string GetInsertQuery()
        {
            return $@"INSERT INTO {TableName} (description_id, image_url) 
                        VALUES(@DescriptionId, @ImageUrl); SELECT LAST_INSERT_ID();";
        }

        protected override string GetUpdateQuery()
        {
            return $@"UPDATE {TableName}
                        SET image_url = @ImageUrl
                        WHERE {ColumnIdName} = @{_converter.SnakeCaseToPascalCase(ColumnIdName)}";
        }

        public async Task<List<ProductDescriptionImageModel>> GetAllByIdAsync(string id, string colIdName)
        {
            try
            {
                string query = GetByIdQuery(colIdName);
                using IDbConnection connection = connectionFactory.CreateConnection();
                IEnumerable<ProductDescriptionImageModel> result = await connection.QueryAsync<ProductDescriptionImageModel>(query, new { Id = id });
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
