using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TLGames.Core.Entities;
using TLGames.Core.Interfaces.IData;
using TLGames.Core.Interfaces.IValidate;
using TLGames.Infrastructure.Persistence;

namespace TLGames.Infrastructure.Data
{
    public class ProductDescriptionImageDAO(IDbConnectionFactory connectionFactory, IColumnService colService, IStringConverter converter, IStringChecker checker)
        : BaseDAO<ProductDescriptionImageModel>(connectionFactory, colService, converter, checker, "description_images", "description_image_id", null), 
        IGetAllByIdAsync<ProductDescriptionImageModel>
    {
        protected override string GetInsertQuery()
        {
            return $@"INSERT INTO {TableName} (description_id, image_url) 
                        VALUES(@DescriptionId, @ImageUrl); SELECT LAST_INSERT_ID();";
        }

        protected override string GetUpdateQuery()
        {
            return $@"UPDATE {TableName}
                        SET image_url = @ImageUrl
                        WHERE {ColumnIdName} = @{Converter.SnakeCaseToPascalCase(ColumnIdName)}";
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
