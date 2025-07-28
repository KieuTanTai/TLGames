using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TLGames.Core.Entities;
using TLGames.Core.Interfaces.IData;
using TLGames.Core.Interfaces.IValidate;
using TLGames.Infrastructure.Persistence;

namespace TLGames.Infrastructure.Data
{
    public record ProductPublisherItemIds(string PublisherId, string ProductId);
    public class ProductPublisherDAO(IDbConnectionFactory connectionFactory, IColumnService colService, IStringConverter converter, IStringChecker checker)
        : BaseDAO<ProductPublisherModel>(connectionFactory, colService, converter, checker, "publisher_of_products", "publisher_id", "product_id"),
        IGetAllByIdAsync<ProductDeveloperModel>, IGetSingleByIdsAsync<ProductDeveloperModel>, IDeleteByIdsAsync, IUpdateByOldKeyAsync<ProductCategoryModel>
    {
        protected override string GetInsertQuery()
        {
            return $@"INSERT INTO {TableName} ({ColumnIdName}, {SecondColumnIdName}) 
                        VALUES(@{Converter.SnakeCaseToPascalCase(ColumnIdName)}, 
                        @{Converter.SnakeCaseToPascalCase(ColumnIdName)}); SELECT LAST_INSERT_ID();";
        }

        public string GetUpdateWithOldKeyString()
        {
            return $@"UPDATE {TableName}
                        SET {ColumnIdName} = @{Converter.SnakeCaseToPascalCase(ColumnIdName)}
                        WHERE {ColumnIdName} = @OldId
                        AND {SecondColumnIdName} = @{Converter.SnakeCaseToPascalCase(ColumnIdName)}";
        }

        protected override string GetUpdateQuery()
        {
            return $@""; // No update query needed for this DAO, as it is handled by GetUpdateWithOldKeyString
        }

        public string GetDeleteQuery()
        {
            return $"DELETE FROM {TableName} WHERE {ColumnIdName} = @{Converter.SnakeCaseToPascalCase(ColumnIdName)} AND {SecondColumnIdName} = @{Converter.SnakeCaseToPascalCase(SecondColumnIdName)}";
        }

        public string GetSingleDataString()
        {
            return $"SELECT * FROM {TableName} WHERE {ColumnIdName} = {Converter.SnakeCaseToPascalCase(ColumnIdName)} AND {SecondColumnIdName} = {Converter.SnakeCaseToPascalCase(SecondColumnIdName)}";
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
            if (keys is ProductPublisherItemIds)
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

        public async Task<int> DeleteByIdsAsync(object keys)
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
                    return affectRow;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error Commit!\n{ex.StackTrace}");
                    transaction.Rollback();
                    return -1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return -1;
            }
        }

        public override Task<int> UpdateAsync(ProductPublisherModel entity)
        {
            return Task.FromResult(-1);
        }

        public override Task<int> UpdateManyAsync(IEnumerable<ProductPublisherModel> entities)
        {
            return Task.FromResult(-1);
        }

        public async Task<int> UpdateAsync(ProductCategoryModel entity, string oldKey)
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
                    return affectRow;

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error Commit!\n{ex.StackTrace}");
                    transaction.Rollback();
                    return -1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return -1;
            }
        }

        public async Task<int> UpdateAsync(IEnumerable<ProductCategoryModel> entities, string oldKey)
        {
            if (entities == null || !entities.Any())
                throw new ArgumentNullException(nameof(entities), "Entities cannot be null or empty.");

            int totalAffectedRows = 0;
            try
            {
                using IDbConnection connection = connectionFactory.CreateConnection();
                using IDbTransaction transaction = connection.BeginTransaction();
                try
                {
                    string updateQuery = GetUpdateWithOldKeyString();
                    foreach (ProductCategoryModel entity in entities)
                    {
                        int affected = await connection.ExecuteAsync(updateQuery, new { entity, OldId = oldKey }, transaction);
                        totalAffectedRows += affected;
                    }
                    transaction.Commit();
                    return totalAffectedRows;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error Commit!\n{ex.StackTrace}");
                    transaction.Rollback();
                    return -1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return -1;
            }
        }
    }
}
