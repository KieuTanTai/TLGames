using Dapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TLGames.Applications.Services;
using TLGames.Core.Entities;
using TLGames.Core.Enums;
using TLGames.Core.Interfaces;

namespace TLGames.Infrastructure.Data
{
    internal class DetailUserStorageDAO(IDbConnectionFactory connectionFactory) : BaseDAO<DetailUserStorageModel>(connectionFactory), IGetSingleByIdsAsync<DetailUserStorageModel>, IGetAllByIdAsync<DetailUserStorageModel>, 
                                        IGetRelativeAsync<DetailUserStorageModel>, IGetDataByDateTime<DetailUserStorageModel>
    {
        protected override string TableName => "detail_user_storages";

        protected override string ColumnIdName => "user_storage_id";
        private static string SecondColumnIdName => "product_id";
        private readonly IStringConverter _converter = App.SystemServices.GetService<IStringConverter>();
        public record DetailUserStorageItemIds(string UserStorageId, string ProductId);

        protected override string GetInsertQuery()
        {
            return $@"INSERT INTO {TableName} ({SecondColumnIdName}, {ColumnIdName}, last_played, play_time, is_favored, purchase_date, is_installed, installed_date) 
                            VALUES(@{_converter.SnakeCaseToPascalCase(ColumnIdName)}, @{_converter.SnakeCaseToPascalCase(ColumnIdName)}, @LastPlayed, @PlayTime, @IsFavorated, @PurchaseDate, @IsInstalled, @InstalledDate); SELECT LAST_INSERT_ID();";
        }
        public string GetSingleDataString()
        {
            return $"SELECT * FROM {TableName} WHERE {ColumnIdName} = {_converter.SnakeCaseToPascalCase(ColumnIdName)} AND {SecondColumnIdName} = {_converter.SnakeCaseToPascalCase(SecondColumnIdName)}";
        }

        protected override string GetUpdateQuery()
        {
            return $@"UPDATE {TableName}
                            SET last_played = @LastPlayed, play_time = @PlayTime, is_favored = @IsFavored, purchase_date = @PurchaseDate, is_installed = @IsInstalled, installed_date = @InstalledDate
                            WHERE {ColumnIdName} = @{_converter.SnakeCaseToPascalCase(ColumnIdName)} 
                            AND {SecondColumnIdName} = @{_converter.SnakeCaseToPascalCase(ColumnIdName)}";
        }
        protected override string DeleteByIdQuery(string colIdName)
        {
            return "";
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
            if (!_colService.IsValidColumn(TableName, colName))
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
            if (!_colService.IsValidColumn(TableName, colName))
                return "";
            return $"SELECT * FROM {TableName} WHERE Month({colName}) = @Input";
        }

        public string GetByYear(string colName)
        {
            if (!_colService.IsValidColumn(TableName, colName))
                return "";
            return $"SELECT * FROM {TableName} WHERE Year({colName}) = @Input";
        }

        public string GetByDateTime(string colName)
        {
            if (!_colService.IsValidColumn(TableName, colName))
                return "";
            return $"SELECT * FROM {TableName} WHERE {colName} = DATE_ADD(@Input, INTERVAL 1 DAY);";
        }

        public string GetByDateTimeRange(string colName)
        {
            if (!_colService.IsValidColumn(TableName, colName))
                return "";
            return $"SELECT * FROM {TableName} WHERE {colName} >= @FirstTime AND {colName} < DATE_ADD(@SecondTime, INTERVAL 1 DAY);";
        }

        public string GetByMonthAndYear(string colName)
        {
            if (!_colService.IsValidColumn(TableName, colName))
                return "";
            return $"SELECT * FROM {TableName} WHERE YEAR({colName}) = @FirstTime AND MONTH({colName}) = @SecondTime;";
        }

        public async Task<List<DetailUserStorageModel>> GetAllByTimeRange<TEnum>(string firstInputTime, string secondInputTime, string colName, TEnum timeType) where TEnum : Enum
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

        public async Task<List<DetailUserStorageModel>> GetAllByTime<TEnum>(string time, string colName, TEnum timeType) where TEnum : Enum
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
    }
}
