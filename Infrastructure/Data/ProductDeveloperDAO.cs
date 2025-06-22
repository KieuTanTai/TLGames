using Dapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TLGames.Applications.Services;
using TLGames.Core.Entities;
using TLGames.Core.Interfaces;
using static TLGames.Infrastructure.Data.FollowerOfDeveloperDAO;

namespace TLGames.Infrastructure.Data
{
    internal class ProductDeveloperDAO(IDbConnectionFactory connectionFactory) : BaseDAO<ProductDeveloperModel>(connectionFactory), IGetAllByIdAsync<ProductDeveloperModel>, IGetSingleByIdsAsync<ProductDeveloperModel>, 
                                        IUpdateByOldKeyAsync<ProductDeveloperModel>, IDeleteByIdsAsync
    {
        protected override string TableName => "developer_of_products";

        protected override string ColumnIdName => "developer_id";
        private static string SecondColumnIdName => "product_id";
        private readonly IStringConverter _converter = App.SystemServices.GetService<IStringConverter>();

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
                        SET {ColumnIdName} = @{_converter.SnakeCaseToPascalCase(ColumnIdName)}
                        WHERE {ColumnIdName} = @OldId
                        AND {SecondColumnIdName} = @{_converter.SnakeCaseToPascalCase(ColumnIdName)}";
        }

        public string GetDeleteQuery()
        {
            return $"DELETE FROM {TableName} WHERE {ColumnIdName} = @{_converter.SnakeCaseToPascalCase(ColumnIdName)} AND {SecondColumnIdName} = @{_converter.SnakeCaseToPascalCase(SecondColumnIdName)}";
        }

        public string GetSingleDataString()
        {
            return $"SELECT * FROM {TableName} WHERE {ColumnIdName} = {_converter.SnakeCaseToPascalCase(ColumnIdName)} AND {SecondColumnIdName} = {_converter.SnakeCaseToPascalCase(SecondColumnIdName)}";
        }

        public async Task<List<ProductDeveloperModel>> GetAllByIdAsync(string id, string colIdName)
        {
            try
            {
                string query = GetByIdQuery(colIdName);
                using IDbConnection connection = connectionFactory.CreateConnection();
                IEnumerable<ProductDeveloperModel> result = await connection.QueryAsync<ProductDeveloperModel>(query, new { Id = id });
                return result.AsList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return new();
            }
        }

        public async Task<ProductDeveloperModel> GetSingleByIdAsync(object keys)
        {
            if (keys is FollowerOfDeveloperItemIds)
            {
                try
                {
                    string query = GetSingleDataString();
                    using IDbConnection connection = connectionFactory.CreateConnection();
                    ProductDeveloperModel result = await connection.QueryFirstOrDefaultAsync<ProductDeveloperModel>(query, keys);
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

        public async Task<bool> UpdateAsync(ProductDeveloperModel entity, string oldKey)
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
