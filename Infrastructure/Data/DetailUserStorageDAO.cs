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
    public record DetailUserStorageItemIds(string UserStorageId, string ProductId);
    public class DetailUserStorageDAO(IDbConnectionFactory connectionFactory, IColumnService colService, IStringConverter converter, IStringChecker checker)
        : BaseDAO<DetailUserStorageModel>(connectionFactory, colService, converter, checker, "detail_user_storages", "user_storage_id", "product_id"), IGetDataByEnumAsync<DetailUserStorageModel>,
        IGetSingleByIdsAsync<DetailUserStorageModel>, IGetAllByIdAsync<DetailUserStorageModel>, IGetRelativeAsync<DetailUserStorageModel>, IGetDataByDateTimeAsync<DetailUserStorageModel>
    {
        protected override string GetInsertQuery()
        {
            return $@"INSERT INTO {TableName} ({SecondColumnIdName}, 
                            {ColumnIdName}, last_played, play_time, is_favored, purchase_date, is_installed, installed_date, status) 
                            VALUES(@{Converter.SnakeCaseToPascalCase(ColumnIdName)}, @{Converter.SnakeCaseToPascalCase(ColumnIdName)}, @LastPlayed, @PlayTime, @IsFavorated, @PurchaseDate, @IsInstalled, @InstalledDate, @Status); 
                            SELECT LAST_INSERT_ID();";
        }
        public string GetSingleDataString()
        {
            return $"SELECT * FROM {TableName} WHERE {ColumnIdName} = {Converter.SnakeCaseToPascalCase(ColumnIdName)} AND {SecondColumnIdName} = {Converter.SnakeCaseToPascalCase(SecondColumnIdName)}";
        }

        protected override string GetUpdateQuery()
        {
            return $@"UPDATE {TableName}
                            SET last_played = @LastPlayed, play_time = @PlayTime, is_favored = @IsFavored, purchase_date = @PurchaseDate, is_installed = @IsInstalled, installed_date = @InstalledDate, status = @Status
                            WHERE {ColumnIdName} = @{Converter.SnakeCaseToPascalCase(ColumnIdName)} 
                            AND {SecondColumnIdName} = @{Converter.SnakeCaseToPascalCase(ColumnIdName)}";
        }
        protected override string DeleteByIdQuery(string colIdName)
        {
            return ""; // Soft delete is handled in DeleteAsync
        }

        public async override Task<int> DeleteAsync(string id)
        {
            DetailUserStorageModel detail = await GetByIdAsync(id);
            if (detail == null)
                return -1;
            detail.SetStatus(EActiveStatus.INACTIVE);
            return await UpdateAsync(detail);
        }

        public override async Task<int> DeleteManyAsync(IEnumerable<string> ids)
        {
            if (ids == null || !ids.Any())
                return -1;

            List<DetailUserStorageModel> DetailsToUpdate = new List<DetailUserStorageModel>();

            foreach (string id in ids)
            {
                DetailUserStorageModel detail = await GetByIdAsync(id);
                if (detail == null)
                    return -1;
                detail.SetStatus(EActiveStatus.INACTIVE);
                DetailsToUpdate.Add(detail);
            }
            return await UpdateManyAsync(DetailsToUpdate);
        }

        public async Task<List<DetailUserStorageModel>> GetAllByIdAsync(string id, string colIdName)
        {
            try
            {
                string query = GetByIdQuery(colIdName);
                using IDbConnection connection = connectionFactory.CreateConnection();
                IEnumerable<DetailUserStorageModel> result = await connection.QueryAsync<DetailUserStorageModel>(query, new { Id = id });
                return result.AsList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return new();
            }
        }

        public async Task<DetailUserStorageModel> GetSingleByIdAsync(object keys)
        {
            if (keys is DetailUserStorageItemIds)
            {
                try
                {
                    string query = GetSingleDataString();
                    using IDbConnection connection = connectionFactory.CreateConnection();
                    DetailUserStorageModel result = await connection.QueryFirstOrDefaultAsync<DetailUserStorageModel>(query, keys);
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

        public string GetQueryDataString(string colName)
        {
            if (!ColService.IsValidColumn(TableName, colName))
                return "";
            return $@"SELECT * FROM {TableName} WHERE {colName} LIKE @Input";
        }

        public async Task<List<DetailUserStorageModel>> GetRelativeAsync(string input, string colName)
        {
            try
            {
                string query = GetQueryDataString(colName);
                using IDbConnection connection = connectionFactory.CreateConnection();
                if (!input.Contains('%'))
                    input = $"%{input}%";
                IEnumerable<DetailUserStorageModel> result = await connection.QueryAsync<DetailUserStorageModel>(query, new { Input = input });
                return result.AsList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return new();
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

        public async Task<List<DetailUserStorageModel>> GetAllByTimeRangeAsync<TEnum>(string firstInputTime, string secondInputTime, string colName, TEnum timeType) where TEnum : Enum
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
                    IEnumerable<DetailUserStorageModel> result = await connection.QueryAsync<DetailUserStorageModel>(query, new { FirstTime = firstInputTime, SecondTime = secondInputTime });
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

        public async Task<List<DetailUserStorageModel>> GetAllByTimeAsync<TEnum>(string time, string colName, TEnum timeType) where TEnum : Enum
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
                    IEnumerable<DetailUserStorageModel> result = await connection.QueryAsync<DetailUserStorageModel>(query, new { Input = time });
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

        public async Task<List<DetailUserStorageModel>> IGetAllByEnumAsync<TEnum>(TEnum value, string colName) where TEnum : Enum
        {
            if (value is EActiveStatus)
            {
                try
                {
                    string query = GetByIdQuery(colName);
                    using IDbConnection connection = connectionFactory.CreateConnection();
                    IEnumerable<DetailUserStorageModel> result = await connection.QueryAsync<DetailUserStorageModel>(query, new { Id = value });
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
