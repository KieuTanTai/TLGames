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
    public record ProductCategoryItemIds(string ProductId, string CategoryId);
    public class ProductCategoryDAO(IDbConnectionFactory connectionFactory, IColumnService colService, IStringConverter converter, IStringChecker checker)
        : BaseDAO<ProductCategoryModel>(connectionFactory, colService, converter, checker, "product_categories", "product_id", "category_id"),
        IGetAllByIdAsync<ProductCategoryModel>, IGetSingleByIdsAsync<ProductCategoryModel>, IUpdateByOldKeyAsync<ProductCategoryModel>, IDeleteByIdsAsync
    {
        protected override string GetInsertQuery()
        {
            return $@"INSERT INTO {TableName} ({ColumnIdName}, {SecondColumnIdName}) 
                        VALUES(@{Converter.SnakeCaseToPascalCase(ColumnIdName)},
                        @{Converter.SnakeCaseToPascalCase(ColumnIdName)}); SELECT LAST_INSERT_ID();";
        }

        protected override string GetUpdateQuery()
        {
            return $@""; // No update query needed for this DAO, as it uses GetUpdateWithOldKeyString
        }

        public string GetUpdateWithOldKeyString()
        {
            return $@"UPDATE {TableName}
                        SET {SecondColumnIdName} = @{Converter.SnakeCaseToPascalCase(ColumnIdName)}
                        WHERE {ColumnIdName} = @{Converter.SnakeCaseToPascalCase(ColumnIdName)}
                        AND {SecondColumnIdName} = @OldId";
        }
        public string GetDeleteQuery()
        {
            return $"DELETE FROM {TableName} WHERE {ColumnIdName} = @{Converter.SnakeCaseToPascalCase(ColumnIdName)} " +
                    $"AND {SecondColumnIdName} = @{Converter.SnakeCaseToPascalCase(SecondColumnIdName)}";
        }

        public string GetSingleDataString()
        {
            return $"SELECT * FROM {TableName} WHERE {ColumnIdName} = {Converter.SnakeCaseToPascalCase(ColumnIdName)} " +
                    $"AND {SecondColumnIdName} = {Converter.SnakeCaseToPascalCase(SecondColumnIdName)}";
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

        public override Task<int> UpdateAsync(ProductCategoryModel entity)
        {
            return Task.FromResult(-1); // Soft delete is handled in SoftDeleteAsync
        }

        public override Task<int> UpdateManyAsync(IEnumerable<ProductCategoryModel> entities)
        {
            return Task.FromResult(-1); // Soft delete is handled in SoftDeleteAsync
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

        //return row affect in the database
        public async Task<int> UpdateAsync(IEnumerable<ProductCategoryModel> entities, string oldKey)
        {
            if (entities == null)
                return 0;

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
                        int affected = await connection.ExecuteAsync(
                            updateQuery,
                            new { entity.ProductId, entity.CategoryId, OldId = oldKey },
                            transaction
                        );
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
