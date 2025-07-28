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
    public class CustomerDAO(IDbConnectionFactory connectionFactory, IColumnService colService, IStringConverter converter, IStringChecker checker)
        : BaseDAO<CustomerModel>(connectionFactory, colService, converter, checker, "customers", "customer_id", null), IGetRelativeAsync<CustomerModel>, IGetDataByDateTimeAsync<CustomerModel>, IGetDataByEnumAsync<CustomerModel>
    {
        protected override string GetInsertQuery()
        {
            return $@"INSERT INTO {TableName} (user_id, birthday, personal_phone, personal_name, personal_address, avatar_url, background_url, gender, status) 
                        VALUES(@UserId, @BirthDay, @PersonalPhone, @PersonalName, @PersonalAddress, @AvatarUrl, @BackgroundUrl, @Gender, @Status); SELECT LAST_INSERT_ID();";
        }

        protected override string GetUpdateQuery()
        {
            return $@"UPDATE {TableName}
                        SET birthday = @BirthDay, personal_phone = @PersonalPhone, personal_name = @PersonalName, personal_address = @PersonalAddress, 
                        avatar_url = @AvatarUrl, background_url = @BackgroundUrl, gender = @Gender, status = @Status
                        WHERE {ColumnIdName} = @{Converter.SnakeCaseToPascalCase(ColumnIdName)}";
        }

        protected override string DeleteByIdQuery(string colIdName)
        {   
            return ""; // Soft delete is handled in DeleteAsync
        }

        public async override Task<int> DeleteAsync(string id)
        {
            CustomerModel customer = await GetByIdAsync(id);
            if (customer == null)
                return -1;
            customer.SetStatus(EUserStatus.INACTIVE);
            return await UpdateAsync(customer);
        }

        public override async Task<int> DeleteManyAsync(IEnumerable<string> ids)
        {
            if (ids == null || !ids.Any())
                return -1;
            List<CustomerModel> customersToUpdate = new List<CustomerModel>();
            foreach (string id in ids)
            {
                CustomerModel customer = await GetByIdAsync(id);
                if (customer == null)
                    return -1;
                customer.SetStatus(EUserStatus.INACTIVE);
                customersToUpdate.Add(customer);
            }
            return await UpdateManyAsync(customersToUpdate);
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
            return $"SELECT * FROM {TableName} WHERE {colName} LIKE @Input";
        }

        // search by enum
        public async Task<List<CustomerModel>> IGetAllByEnumAsync<TEnum>(TEnum value, string colName) where TEnum : Enum
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

        public async Task<List<CustomerModel>> GetAllByTimeRangeAsync<TEnum>(string firstInputTime, string secondInputTime, string colName, TEnum timeType) where TEnum : Enum
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

        public async Task<List<CustomerModel>> GetAllByTimeAsync<TEnum>(string time, string colName, TEnum timeType) where TEnum : Enum
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
