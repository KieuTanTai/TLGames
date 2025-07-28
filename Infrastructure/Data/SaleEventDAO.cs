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
    public class SaleEventDAO(IDbConnectionFactory connectionFactory, IColumnService colService, IStringConverter converter, IStringChecker checker)
        : BaseDAO<SaleEventModel>(connectionFactory, colService, converter, checker, "sale_events", "sale_event_id", null), IGetRelativeAsync<SaleEventModel>, IGetDataByDateTimeAsync<SaleEventModel>, IGetDataByEnumAsync<SaleEventModel>
    {
        protected override string GetInsertQuery()
        {
            return $@"INSERT INTO {TableName} (discount_code, start_date, end_date, sale_event_name, status, description) 
                        VALUES(@DiscountCode, @StartDate, @EndDate, @SaleEventName, @Status, @Description); SELECT LAST_INSERT_ID();";
        }

        protected override string GetUpdateQuery()
        {
            return $@"UPDATE {TableName}
                        SET discount_code = @DiscountCode, start_date = @StartDate, end_date = @EndDate, 
                        sale_event_name = @SaleEventName, status = @Status, description = @Description 
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
            SaleEventModel saleEvent = await GetByIdAsync(id);
            if (saleEvent == null)
                return -1;
            saleEvent.SetStatus(EActiveStatus.INACTIVE);
            return await UpdateAsync(saleEvent);
        }

        public override async Task<int> DeleteManyAsync(IEnumerable<string> ids)
        {
            if (ids == null || !ids.Any())
                return -1;

            List<SaleEventModel> saleEventsForUpdate = new List<SaleEventModel>();

            foreach (string id in ids)
            {
                SaleEventModel saleEvent = await GetByIdAsync(id);
                if (saleEvent == null)
                    return -1;
                saleEvent.SetStatus(EActiveStatus.INACTIVE);
                saleEventsForUpdate.Add(saleEvent);
            }
            return await UpdateManyAsync(saleEventsForUpdate);
        }

        public async Task<List<SaleEventModel>> GetRelativeAsync(string input, string colName)
        {
            try
            {
                string query = GetQueryDataString(colName);
                using IDbConnection connection = connectionFactory.CreateConnection();
                if (!input.Contains('%'))
                    input = $"%{input}%";
                IEnumerable<SaleEventModel> result = await connection.QueryAsync<SaleEventModel>(query, new { Input = input });
                return result.AsList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return new();
            }
        }

        // search by enum
        public async Task<List<SaleEventModel>> IGetAllByEnumAsync<TEnum>(TEnum value, string colName) where TEnum : Enum
        {
            if (value is EActiveStatus)
            {
                try
                {
                    string query = GetByIdQuery(colName);
                    using IDbConnection connection = connectionFactory.CreateConnection();
                    IEnumerable<SaleEventModel> result = await connection.QueryAsync<SaleEventModel>(query, new { Id = value });
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

        public string GetByStartAndEndTime()
        {
            return $"SELECT * FROM {TableName} WHERE start_time >= @FirstTime AND end_time < DATE_ADD(@SecondTime, INTERVAL 1 DAY);";
        }

        public async Task<List<SaleEventModel>> GetByStartAndEndDate<TEnum>(string firstInputTime, string secondInputTime)
        {

            string query = GetByStartAndEndTime();
            try
            {
                using IDbConnection connection = connectionFactory.CreateConnection();
                IEnumerable<SaleEventModel> result = await connection.QueryAsync<SaleEventModel>(query, new { FirstTime = firstInputTime, SecondTime = secondInputTime });
                return result.AsList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return new();
            }
        }

        public async Task<List<SaleEventModel>> GetAllByTimeRangeAsync<TEnum>(string firstInputTime, string secondInputTime, string colName, TEnum timeType) where TEnum : Enum
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
                    IEnumerable<SaleEventModel> result = await connection.QueryAsync<SaleEventModel>(query, new { FirstTime = firstInputTime, SecondTime = secondInputTime });
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

        public async Task<List<SaleEventModel>> GetAllByTimeAsync<TEnum>(string time, string colName, TEnum timeType) where TEnum : Enum
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
                    IEnumerable<SaleEventModel> result = await connection.QueryAsync<SaleEventModel>(query, new { Input = time });
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
