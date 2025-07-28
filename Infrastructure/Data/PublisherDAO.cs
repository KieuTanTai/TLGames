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
    public class PublisherDAO(IDbConnectionFactory connectionFactory, IColumnService colService, IStringConverter converter, IStringChecker checker)
        : BaseDAO<PublisherModel>(connectionFactory, colService, converter, checker, "publishers", "publisher_id", null), IGetRelativeAsync<PublisherModel>, IGetDataByDateTimeAsync<PublisherModel>, IGetDataByEnumAsync<PublisherModel>
    {
        protected override string GetInsertQuery()
        {
            return $@"INSERT INTO {TableName} (user_id, publisher_name, became_publisher_date, description, website_url, business_phone, business_address, business_email, status)
                VALUES(@UserId, @PublisherName, @BecamePublisherDate, @Description, @WebsiteUrl, @BusinessPhone, @BusinessAddress, @BusinessEmail, @Status);";
        }

        protected override string GetUpdateQuery()
        {
            return $@"UPDATE {TableName}
                SET publisher_name = @PublisherName,
                    became_publisher_date = @BecamePublisherDate,
                    description = @Description,
                    website_url = @WebsiteUrl,
                    business_phone = @BusinessPhone,
                    business_address = @BusinessAddress,
                    business_email = @BusinessEmail,
                    status = @Status
                WHERE {ColumnIdName} = @{Converter.SnakeCaseToPascalCase(ColumnIdName)}";
        }

        public string GetQueryDataString(string colName)
        {
            if (!ColService.IsValidColumn(TableName, colName))
                return "";
            return $"SELECT * FROM {TableName} WHERE {colName} LIKE @Input";
        }

        protected override string DeleteByIdQuery(string colIdName)
        {
            return ""; // Soft delete is handled in SoftDeleteAsync
        }

        public async override Task<int> DeleteAsync(string id)
        {
            PublisherModel publisher = await GetByIdAsync(id);
            if (publisher == null)
                return -1;
            publisher.SetStatus(EUserStatus.INACTIVE);
            return await UpdateAsync(publisher);
        }

        public override async Task<int> DeleteManyAsync(IEnumerable<string> ids)
        {
            if (ids == null || !ids.Any())
                return -1;

            List<PublisherModel> publishersForUpdate = new List<PublisherModel>();

            foreach (string id in ids)
            {
                PublisherModel publisher = await GetByIdAsync(id);
                if (publisher == null)
                    return -1;
                publisher.SetStatus(EUserStatus.INACTIVE);
                publishersForUpdate.Add(publisher);
            }
            return await UpdateManyAsync(publishersForUpdate);
        }

        public async Task<List<PublisherModel>> GetRelativeAsync(string input, string colName)
        {
            try
            {
                string query = GetQueryDataString(colName);
                using IDbConnection connection = connectionFactory.CreateConnection();
                if (!input.Contains('%'))
                    input = $"%{input}%";
                IEnumerable<PublisherModel> result = await connection.QueryAsync<PublisherModel>(query, new { Input = input });
                return result.AsList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return new();
            }
        }

        // search by enum
        public async Task<List<PublisherModel>> IGetAllByEnumAsync<TEnum>(TEnum value, string colName) where TEnum : Enum
        {
            if (value is EUserStatus)
            {
                try
                {
                    string query = GetByIdQuery(colName);
                    using IDbConnection connection = connectionFactory.CreateConnection();
                    IEnumerable<PublisherModel> result = await connection.QueryAsync<PublisherModel>(query, new { Id = value });
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

        public async Task<List<PublisherModel>> GetAllByTimeRangeAsync<TEnum>(string firstInputTime, string secondInputTime, string colName, TEnum timeType) where TEnum : Enum
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
                    IEnumerable<PublisherModel> result = await connection.QueryAsync<PublisherModel>(query, new { FirstTime = firstInputTime, SecondTime = secondInputTime });
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

        public async Task<List<PublisherModel>> GetAllByTimeAsync<TEnum>(string time, string colName, TEnum timeType) where TEnum : Enum
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
                    IEnumerable<PublisherModel> result = await connection.QueryAsync<PublisherModel>(query, new { Input = time });
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
