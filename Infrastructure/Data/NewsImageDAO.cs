using Dapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TLGames.Applications.Services;
using TLGames.Core.Entities;
using TLGames.Core.Interfaces;
using TLGames.Infrastructure.Services;

namespace TLGames.Infrastructure.Data
{
    internal class NewsImageDAO(IDbConnectionFactory connectionFactory) : BaseDAO<NewsImageModel>(connectionFactory), IGetAllByIdAsync<NewsImageModel>
    {
        protected override string TableName => "news_images";

        protected override string ColumnIdName => "news_image_id";
        private readonly IStringConverter _converter = App.SystemServices.GetService<IStringConverter>();

        protected override string GetInsertQuery()
        {
            return $@"INSERT INTO {TableName} (news_id, image_url) 
                        VALUES(@NewsId, @ImageUrl); SELECT LAST_INSERT_ID();";
        }

        protected override string GetUpdateQuery()
        {
            return $@"UPDATE {TableName}
                        SET image_url = @ImageUrl
                        WHERE {ColumnIdName} = @{_converter.SnakeCaseToPascalCase(ColumnIdName)}";
        }

        public async Task<List<NewsImageModel>> GetAllByIdAsync(string id, string colIdName)
        {
            try
            {
                string query = GetByIdQuery(colIdName);
                using IDbConnection connection = connectionFactory.CreateConnection();
                IEnumerable<NewsImageModel> result = await connection.QueryAsync<NewsImageModel>(query, new { Id = id });
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
