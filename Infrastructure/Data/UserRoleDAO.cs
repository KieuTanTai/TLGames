using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TLGames.Core.Entities;
using TLGames.Core.Enums;
using TLGames.Core.Interfaces.IData;
using TLGames.Core.Interfaces.IValidate;
using TLGames.Infrastructure.Persistence;

namespace TLGames.Infrastructure.Data
{
    public class UserRoleDAO(IDbConnectionFactory connectionFactory, IColumnService colService, IStringConverter converter, IStringChecker checker)
        : BaseDAO<UserRoleModel>(connectionFactory, colService, converter, checker, "role_of_users", "user_id", "role_id"),
        IGetSingleByIdsAsync<UserRoleModel>, IGetAllByIdAsync<UserRoleModel>, IUpdateByOldKeyAsync<UserRoleModel>, IGetDataByDateTimeAsync<UserRoleModel>
    {
        protected override string GetInsertQuery()
        {
            return $@"INSERT INTO {TableName} ({ColumnIdName}, {SecondColumnIdName}, create_date) 
                        VALUES(@{Converter.SnakeCaseToPascalCase(ColumnIdName)},
                        @{Converter.SnakeCaseToPascalCase(ColumnIdName)},
                        @CreateDate); SELECT LAST_INSERT_ID();";
        }

        protected override string GetUpdateQuery()
        {
            return ""; // Soft delete is handled in SoftDeleteAsync
        }

        public string GetUpdateWithOldKeyString()
        {
            return $@"UPDATE {TableName}
                        SET {SecondColumnIdName} = @{Converter.SnakeCaseToPascalCase(ColumnIdName)},
                            create_date = @CreateDate
                        WHERE {ColumnIdName} = @{Converter.SnakeCaseToPascalCase(ColumnIdName)}
                        AND {SecondColumnIdName} = @OldId";
        }

        public string GetSingleDataString()
        {
            return $"SELECT * FROM {TableName} WHERE {ColumnIdName} = {Converter.SnakeCaseToPascalCase(ColumnIdName)} AND {SecondColumnIdName} = {Converter.SnakeCaseToPascalCase(SecondColumnIdName)}";
        }

        public async Task<List<UserRoleModel>> GetAllByIdAsync(string id, string colIdName)
        {
            try
            {
                string query = GetByIdQuery(colIdName);
                using IDbConnection connection = connectionFactory.CreateConnection();
                IEnumerable<UserRoleModel> result = await connection.QueryAsync<UserRoleModel>(query, new { Id = id });
                return result.AsList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return new();
            }
        }

        public async Task<UserRoleModel> GetSingleByIdAsync(object keys)
        {
            if (keys is FollowerOfPublisherItemIds)
            {
                try
                {
                    string query = GetSingleDataString();
                    using IDbConnection connection = connectionFactory.CreateConnection();
                    UserRoleModel result = await connection.QueryFirstOrDefaultAsync<UserRoleModel>(query, keys);
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

        public override Task<int> UpdateAsync(UserRoleModel entity)
        {
            return Task.FromResult(-1); // Soft delete is handled in SoftDeleteAsync
        }

        public override Task<int> UpdateManyAsync(IEnumerable<UserRoleModel> entities)
        {
            return Task.FromResult(-1); // Soft delete is handled in SoftDeleteAsync
        }

        public async Task<int> UpdateAsync(UserRoleModel entity, string oldKey)
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

        // NOTE: TAKE CARE: This method is not used in the current implementation.
        public async Task<int> UpdateAsync(IEnumerable<UserRoleModel> entities, string oldKey)
        {
            if (entities == null || !entities.Any())
                return 0;

            int affectedRows = 0;
            using IDbConnection connection = connectionFactory.CreateConnection();
            using IDbTransaction transaction = connection.BeginTransaction();
            try
            {
                string query = GetUpdateWithOldKeyString();
                foreach (UserRoleModel entity in entities)
                {
                    var parameters = new
                    {
                        entity.UserId,
                        entity.RoleId,
                        entity.CreateDate,
                        OldId = oldKey
                    };
                    affectedRows += await connection.ExecuteAsync(query, parameters, transaction);
                }
                transaction.Commit();
                return affectedRows;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Commit!\n{ex.StackTrace}");
                transaction.Rollback();
                return -1;
            }
        }

        // search by time
        public string GetByMonth(string colName)
        {
            if (!ColService.IsValidColumn(TableName, colName))
                return "";
            return $"SELECT * FROM {TableName} WHERE Month({colName}) = @Input";
        }

        public string GetByYear(string colName)
        {
            if (!ColService.IsValidColumn(TableName, colName))
                return "";
            return $"SELECT * FROM {TableName} WHERE Year({colName}) = @Input";
        }

        public string GetByDateTime(string colName)
        {
            if (!ColService.IsValidColumn(TableName, colName))
                return "";
            return $"SELECT * FROM {TableName} WHERE {colName} = DATE_ADD(@Input, INTERVAL 1 DAY);";
        }

        public string GetByDateTimeRange(string colName)
        {
            if (!ColService.IsValidColumn(TableName, colName))
                return "";
            return $"SELECT * FROM {TableName} WHERE {colName} >= @FirstTime AND {colName} < DATE_ADD(@SecondTime, INTERVAL 1 DAY);";
        }

        public string GetByMonthAndYear(string colName)
        {
            if (!ColService.IsValidColumn(TableName, colName))
                return "";
            return $"SELECT * FROM {TableName} WHERE YEAR({colName}) = @FirstTime AND MONTH({colName}) = @SecondTime;";
        }

        public async Task<List<UserRoleModel>> GetAllByTimeRangeAsync<TEnum>(string firstInputTime, string secondInputTime, string colName, TEnum timeType) where TEnum : Enum
        {
            if (timeType is EDataTimeType)
            {
                string query = string.Empty;

                query = timeType switch
                {
                    EDataTimeType.MONTHANDYEAR => GetByMonthAndYear(colName),
                    EDataTimeType.DATETIME => GetByDateTimeRange(colName),
                    _ => throw new ArgumentOutOfRangeException(nameof(timeType), "Invalid time type provided."),
                };
                try
                {
                    using IDbConnection connection = connectionFactory.CreateConnection();
                    IEnumerable<UserRoleModel> result = await connection.QueryAsync<UserRoleModel>(query, new { FirstTime = firstInputTime, SecondTime = secondInputTime });
                    return result.AsList();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                    return new();
                }
            }
            return new();
        }
        public async Task<List<UserRoleModel>> GetAllByTimeAsync<TEnum>(string time, string colName, TEnum timeType) where TEnum : Enum
        {
            if (timeType is EDataTimeType)
            {
                string query = string.Empty;

                query = timeType switch
                {
                    EDataTimeType.MONTH => GetByMonth(colName),
                    EDataTimeType.YEAR => GetByYear(colName),
                    EDataTimeType.DATETIME => GetByDateTime(colName),
                    _ => throw new ArgumentOutOfRangeException(nameof(timeType), "Invalid time type provided."),
                };
                try
                {
                    using IDbConnection connection = connectionFactory.CreateConnection();
                    IEnumerable<UserRoleModel> result = await connection.QueryAsync<UserRoleModel>(query, new { Input = time });
                    return result.AsList();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                    return new();
                }
            }
            return new();
        }
    }
}
