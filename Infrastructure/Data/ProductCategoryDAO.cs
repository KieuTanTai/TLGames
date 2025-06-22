using System.Data;
using System.Threading.Tasks;
using System;
using TLGames.Applications.Services;
using TLGames.Core.Entities;
using TLGames.Core.Interfaces;
using TLGames.Infrastructure.Services;
using Dapper;
using Microsoft.Extensions.DependencyInjection;
using static TLGames.Infrastructure.Data.FollowerOfDeveloperDAO;
using System.Collections.Generic;

namespace TLGames.Infrastructure.Data
{
    internal class ProductCategoryDAO(IDbConnectionFactory connectionFactory) : BaseDAO<ProductCategoryModel>(connectionFactory), IGetAllByIdAsync<ProductCategoryModel>, IGetSingleByIdsAsync<ProductCategoryModel>, 
                                        IUpdateByOldKeyAsync<ProductCategoryModel>, IDeleteByIdsAsync
    {
        protected override string TableName => "product_categories";
        protected override string ColumnIdName => "product_id";
        private static string SecondColumnIdName => "category_id";
        private readonly IStringConverter _converter = App.SystemServices.GetService<IStringConverter>();
        public record ProductCategoryItemIds(string ProductId, string CategoryId);

        protected override string GetInsertQuery()
        {
            return $@"INSERT INTO {TableName} ({ColumnIdName}, {SecondColumnIdName}) 
                        VALUES(@{_converter.SnakeCaseToPascalCase(ColumnIdName)},
                        @{_converter.SnakeCaseToPascalCase(ColumnIdName)}); SELECT LAST_INSERT_ID();";
        }

        protected override string GetUpdateQuery()
        {
            return $@"";
        }

        public string GetUpdateWithOldKeyString()
        {
            return $@"UPDATE {TableName}
                        SET {SecondColumnIdName} = @{_converter.SnakeCaseToPascalCase(ColumnIdName)}
                        WHERE {ColumnIdName} = @{_converter.SnakeCaseToPascalCase(ColumnIdName)}
                        AND {SecondColumnIdName} = @OldId";
        }
        public string GetDeleteQuery()
        {
            return $"DELETE FROM {TableName} WHERE {ColumnIdName} = @{_converter.SnakeCaseToPascalCase(ColumnIdName)} AND {SecondColumnIdName} = @{_converter.SnakeCaseToPascalCase(SecondColumnIdName)}";
        }

        public string GetSingleDataString()
        {
            return $"SELECT * FROM {TableName} WHERE {ColumnIdName} = {_converter.SnakeCaseToPascalCase(ColumnIdName)} AND {SecondColumnIdName} = {_converter.SnakeCaseToPascalCase(SecondColumnIdName)}";
        }

        public async Task<List<ProductCategoryModel>> GetAllByIdAsync(string id, string colIdName)
        {
            try
            {
                string query = GetByIdQuery(colIdName);
                using IDbConnection connection = connectionFactory.CreateConnection();
                IEnumerable<ProductCategoryModel> result = await connection.QueryAsync<ProductCategoryModel>(query, new { Id = id });
                return result.AsList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return new();
            }
        }

        public async Task<ProductCategoryModel> GetSingleByIdAsync(object keys)
        {
            if (keys is ProductCategoryItemIds)
            {
                try
                {
                    string query = GetSingleDataString();
                    using IDbConnection connection = connectionFactory.CreateConnection();
                    ProductCategoryModel result = await connection.QueryFirstOrDefaultAsync<ProductCategoryModel>(query, keys);
                    return result;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                    return null;
                }
            }
            return null;
        }

        public async Task<bool> DeleteByIdsAsync(object keys)
        {
            try
            {
                string query = GetDeleteQuery();
                using IDbConnection connection = connectionFactory.CreateConnection();
                using IDbTransaction transaction = connection.BeginTransaction();
                try
                {
                    int affectRow = await connection.ExecuteAsync(query, keys, transaction);
                    transaction.Commit();
                    return affectRow > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error Commit!\n{ex.StackTrace}");
                    transaction.Rollback();
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return false;
            }
        }

        public async Task<bool> UpdateAsync(ProductCategoryModel entity, string oldKey)
        {
            try
            {
                using IDbConnection connection = connectionFactory.CreateConnection();
                using IDbTransaction transaction = connection.BeginTransaction();
                try
                {
                    int affectRow = await connection.ExecuteAsync(GetUpdateWithOldKeyString(),
                        new { entity, OldId = oldKey }, transaction);
                    transaction.Commit();
                    return affectRow > 0;

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error Commit!\n{ex.StackTrace}");
                    transaction.Rollback();
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return false;
            }
        }
    }
}
