using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TLGames.Core.Entities;
using TLGames.Core.Enums;
using TLGames.Core.Interfaces.IData;
using TLGames.Core.Interfaces.IValidate;

namespace TLGames.Infrastructure.Data
{
    internal class CustomerDAO(IDbConnectionFactory connectionFactory, IColumnService colService, IStringConverter converter, IStringChecker checker)
        : BaseDAO<CustomerModel>(connectionFactory, colService, converter, checker, "customers", "customer_id", null),
        ISoftDeleteAsync<CustomerModel>, IGetRelativeAsync<CustomerModel>, IGetDataByDateTime<CustomerModel>, IGetDataByEnum<CustomerModel>
    {
        protected override string GetInsertQuery()
        {
            return $@"INSERT INTO {(IsValidStringInputDB(TableName) ? TableName : throw new ArgumentException("error Input"))} (user_id, birthday, personal_phone, personal_name, personal_address, avatar_url, background_url, gender, status) 
                        VALUES(@UserId, @BirthDay, @PersonalPhone, @PersonalName, @PersonalAddress, @AvatarUrl, @BackgroundUrl, @Gender, @Status); SELECT LAST_INSERT_ID();";
        }

        protected override string GetUpdateQuery()
        {
            return $@"UPDATE {(IsValidStringInputDB(TableName) ? TableName : throw new ArgumentException("error Input"))}
                        SET birthday = @BirthDay, personal_phone = @PersonalPhone, personal_name = @PersonalName, personal_address = @PersonalAddress, 
                        avatar_url = @AvatarUrl, background_url = @BackgroundUrl, gender = @Gender, status = @Status
                        WHERE {(IsValidStringInputDB(ColumnIdName) ? ColumnIdName : throw new ArgumentException("error Input"))} = @{Converter.SnakeCaseToPascalCase(ColumnIdName)}";
        }

        protected override string DeleteByIdQuery(string colIdName)
        {
            if (!ColService.IsValidColumn(TableName, colIdName))
                return "";
            return $@"UPDATE {(IsValidStringInputDB(TableName) ? TableName : throw new ArgumentException("error Input"))} 
                        SET status = @Status WHERE {(IsValidStringInputDB(ColumnIdName) ? ColumnIdName : throw new ArgumentException("error Input"))} = @{Converter.SnakeCaseToPascalCase(ColumnIdName)}";
        }

        public async Task<bool> SoftDeleteAsync(CustomerModel entity)
        {
            try
            {
                string query = DeleteByIdQuery(ColumnIdName);
                using IDbConnection connection = connectionFactory.CreateConnection();
                using IDbTransaction transaction = connection.BeginTransaction();
                try
                {
                    int affectRow = await connection.ExecuteAsync(query, entity, transaction);
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

        public async Task<List<CustomerModel>> GetRelativeAsync(string input, string colName)
        {
            try
            {
                string query = GetQueryDataString(colName);
                using IDbConnection connection = connectionFactory.CreateConnection();
                if (!input.Contains('%'))
                    input = $"%{input}%";
                IEnumerable<CustomerModel> result = await connection.QueryAsync<CustomerModel>(query, new { Input = input });
                return result.AsList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return new();
            }
        }

        public string GetQueryDataString(string colName)
        {
            if (!ColService.IsValidColumn(TableName, colName))
                return "";
            return $"SELECT * FROM {(IsValidStringInputDB(TableName) ? TableName : throw new ArgumentException("error Input"))} WHERE {colName} LIKE @Input";
        }

        // search by enum
        public async Task<List<CustomerModel>> GetAllByEnum<TEnum>(TEnum value, string colName) where TEnum : Enum
        {
            if (value is EUserStatus || value is EGenderType)
            {
                try
                {
                    string query = GetByIdQuery(colName);
                    using IDbConnection connection = connectionFactory.CreateConnection();
                    IEnumerable<CustomerModel> result = await connection.QueryAsync<CustomerModel>(query, new { Id = value });
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

        public async Task<List<CustomerModel>> GetAllByTimeRange<TEnum>(string firstInputTime, string secondInputTime, string colName, TEnum timeType) where TEnum : Enum
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
                    IEnumerable<CustomerModel> result = await connection.QueryAsync<CustomerModel>(query, new { FirstTime = firstInputTime, SecondTime = secondInputTime });
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

        public async Task<List<CustomerModel>> GetAllByTime<TEnum>(string time, string colName, TEnum timeType) where TEnum : Enum
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
                    IEnumerable<CustomerModel> result = await connection.QueryAsync<CustomerModel>(query, new { Input = time });
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
