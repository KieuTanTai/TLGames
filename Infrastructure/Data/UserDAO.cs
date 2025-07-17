using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TLGames.Core.Entities;
using TLGames.Core.Enums;
using TLGames.Core.Interfaces.IData;
using TLGames.Core.Interfaces.IValidate;
using TLGames.Infrastructure.Persistence;

namespace TLGames.Infrastructure.Data
{
    public class UserDAO(IDbConnectionFactory connectionFactory, IColumnService colService, IStringConverter converter, IStringChecker checker)
        : BaseDAO<UserModel>(connectionFactory, colService, converter, checker, "users", "user_id", null), 
        ISoftDeleteAsync<UserModel>, IGetAllByIdAsync<UserModel>, IGetRelativeAsync<UserModel>, IGetDataByEnum<UserModel>, IGetDataByDateTime<UserModel>
    {
        protected override string GetInsertQuery()
        {
            return $@"INSERT INTO {(IsValidStringInputDB(TableName) ? TableName : throw new ArgumentException("error Input"))} (user_name, password, create_date, status) 
                        VALUES(@UserName, @Password, @CreateDate, Status); SELECT LAST_INSERT_ID();";
        }

        protected override string GetUpdateQuery()
        {
            return $@"UPDATE {(IsValidStringInputDB(TableName) ? TableName : throw new ArgumentException("error Input"))}
                        SET user_name = @UserName, password = @Password, create_date = @CreateDate, status = @Status
                        WHERE {(IsValidStringInputDB(ColumnIdName) ? ColumnIdName : throw new ArgumentException("error Input"))} = @{Converter.SnakeCaseToPascalCase(ColumnIdName)}";
        }

        public string GetQueryDataString(string colName)
        {
            if (!ColService.IsValidColumn(TableName, colName))
                return "";
            return $"SELECT * FROM {(IsValidStringInputDB(TableName) ? TableName : throw new ArgumentException("error Input"))} WHERE {colName} = @Input";
        }

        protected override string DeleteByIdQuery(string colIdName)
        {
            return "";
        }

        public async Task<bool> SoftDeleteAsync(UserModel entity)
        {
            return await UpdateAsync(entity);
        }

        public async Task<List<UserModel>> GetRelativeAsync(string status, string colStatusName)
        {
            try
            {
                string query = GetQueryDataString(colStatusName);
                using IDbConnection connection = connectionFactory.CreateConnection();
                IEnumerable<UserModel> result = await connection.QueryAsync<UserModel>(query, new { Input = status });
                return result.AsList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return new();
            }
        }

        public async Task<List<UserModel>> GetAllByIdAsync(string id, string colIdName)
        {
            try
            {
                string query = GetByIdQuery(colIdName);
                using IDbConnection connection = connectionFactory.CreateConnection();
                IEnumerable<UserModel> result = await connection.QueryAsync<UserModel>(query, new { Id = id });
                return result.AsList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return new();
            }
        }

        // search by enum
        public async Task<List<UserModel>> GetAllByEnum<TEnum>(TEnum value, string colName) where TEnum : Enum
        {
            if (value is EUserStatus)
            {
                try
                {
                    string query = GetByIdQuery(colName);
                    using IDbConnection connection = connectionFactory.CreateConnection();
                    IEnumerable<UserModel> result = await connection.QueryAsync<UserModel>(query, new { Id = value });
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

        // search by time
        public string GetByMonth(string colName)
        {
            if (!ColService.IsValidColumn(TableName, colName))
                return "";
            return $"SELECT * FROM {(IsValidStringInputDB(TableName) ? TableName : throw new ArgumentException("error Input"))} WHERE Month({colName}) = @Input";
        }

        public string GetByYear(string colName)
        {
            if (!ColService.IsValidColumn(TableName, colName))
                return "";
            return $"SELECT * FROM {(IsValidStringInputDB(TableName) ? TableName : throw new ArgumentException("error Input"))} WHERE Year({colName}) = @Input";
        }

        public string GetByDateTime(string colName)
        {
            if (!ColService.IsValidColumn(TableName, colName))
                return "";
            return $"SELECT * FROM {(IsValidStringInputDB(TableName) ? TableName : throw new ArgumentException("error Input"))} WHERE {colName} = DATE_ADD(@Input, INTERVAL 1 DAY);";
        }

        public string GetByDateTimeRange(string colName)
        {
            if (!ColService.IsValidColumn(TableName, colName))
                return "";
            return $"SELECT * FROM {(IsValidStringInputDB(TableName) ? TableName : throw new ArgumentException("error Input"))} WHERE {colName} >= @FirstTime AND {colName} < DATE_ADD(@SecondTime, INTERVAL 1 DAY);";
        }

        public string GetByMonthAndYear(string colName)
        {
            if (!ColService.IsValidColumn(TableName, colName))
                return "";
            return $"SELECT * FROM {(IsValidStringInputDB(TableName) ? TableName : throw new ArgumentException("error Input"))} WHERE YEAR({colName}) = @FirstTime AND MONTH({colName}) = @SecondTime;";
        }

        public async Task<List<UserModel>> GetAllByTimeRange<TEnum>(string firstInputTime, string secondInputTime, string colName, TEnum timeType) where TEnum : Enum
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
                    IEnumerable<UserModel> result = await connection.QueryAsync<UserModel>(query, new { FirstTime = firstInputTime, SecondTime = secondInputTime });
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

        public async Task<List<UserModel>> GetAllByTime<TEnum>(string time, string colName, TEnum timeType) where TEnum : Enum
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
                    IEnumerable<UserModel> result = await connection.QueryAsync<UserModel>(query, new { Input = time });
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
