using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TLGames.Applications.Services;
using TLGames.Core.Entities;
using TLGames.Core.Enums;
using TLGames.Core.Interfaces.IData;

namespace TLGames.Infrastructure.Data
{
    public record DetailUserStorageItemIds(string UserStorageId, string ProductId);
    internal class DetailUserStorageDAO(IDbConnectionFactory connectionFactory,
                                        string tableName,
                                        string columnIdName,
                                        string secondColumnIdName) : BaseDAO<DetailUserStorageModel>(connectionFactory, tableName, columnIdName, secondColumnIdName), IGetSingleByIdsAsync<DetailUserStorageModel>, IGetAllByIdAsync<DetailUserStorageModel>,
                                        IGetRelativeAsync<DetailUserStorageModel>, IGetDataByDateTime<DetailUserStorageModel>
    {
        //protected override string TableName => "detail_user_storages";
        //protected override string ColumnIdName => "user_storage_id";
        //private static string SecondColumnIdName => "product_id";

        protected override string GetInsertQuery()
        {
            return $@"INSERT INTO {(IsValidStringInputDB(TableName) ? TableName : throw new ArgumentException("error Input"))} ({(IsValidStringInputDB(SecondColumnIdName) ? SecondColumnIdName : throw new ArgumentException("error Input"))}, {(IsValidStringInputDB(ColumnIdName) ? ColumnIdName : throw new ArgumentException("error Input"))}, last_played, play_time, is_favored, purchase_date, is_installed, installed_date) 
                            VALUES(@{_converter.SnakeCaseToPascalCase(ColumnIdName)}, @{_converter.SnakeCaseToPascalCase(ColumnIdName)}, @LastPlayed, @PlayTime, @IsFavorated, @PurchaseDate, @IsInstalled, @InstalledDate); SELECT LAST_INSERT_ID();";
        }
        public string GetSingleDataString()
        {
            return $"SELECT * FROM {(IsValidStringInputDB(TableName) ? TableName : throw new ArgumentException("error Input"))} WHERE {(IsValidStringInputDB(ColumnIdName) ? ColumnIdName : throw new ArgumentException("error Input"))} = {_converter.SnakeCaseToPascalCase(ColumnIdName)} AND {(IsValidStringInputDB(SecondColumnIdName) ? SecondColumnIdName : throw new ArgumentException("error Input"))} = {_converter.SnakeCaseToPascalCase(SecondColumnIdName)}";
        }

        protected override string GetUpdateQuery()
        {
            return $@"UPDATE {(IsValidStringInputDB(TableName) ? TableName : throw new ArgumentException("error Input"))}
                            SET last_played = @LastPlayed, play_time = @PlayTime, is_favored = @IsFavored, purchase_date = @PurchaseDate, is_installed = @IsInstalled, installed_date = @InstalledDate
                            WHERE {(IsValidStringInputDB(ColumnIdName) ? ColumnIdName : throw new ArgumentException("error Input"))} = @{_converter.SnakeCaseToPascalCase(ColumnIdName)} 
                            AND {(IsValidStringInputDB(SecondColumnIdName) ? SecondColumnIdName : throw new ArgumentException("error Input"))} = @{_converter.SnakeCaseToPascalCase(ColumnIdName)}";
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
            return $@"SELECT * FROM {(IsValidStringInputDB(TableName) ? TableName : throw new ArgumentException("error Input"))} WHERE {colName} LIKE @Input";
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
            return $"SELECT * FROM {(IsValidStringInputDB(TableName) ? TableName : throw new ArgumentException("error Input"))} WHERE Month({colName}) = @Input";
        }

        public string GetByYear(string colName)
        {
            if (!_colService.IsValidColumn(TableName, colName))
                return "";
            return $"SELECT * FROM {(IsValidStringInputDB(TableName) ? TableName : throw new ArgumentException("error Input"))} WHERE Year({colName}) = @Input";
        }

        public string GetByDateTime(string colName)
        {
            if (!_colService.IsValidColumn(TableName, colName))
                return "";
            return $"SELECT * FROM {(IsValidStringInputDB(TableName) ? TableName : throw new ArgumentException("error Input"))} WHERE {colName} = DATE_ADD(@Input, INTERVAL 1 DAY);";
        }

        public string GetByDateTimeRange(string colName)
        {
            if (!_colService.IsValidColumn(TableName, colName))
                return "";
            return $"SELECT * FROM {(IsValidStringInputDB(TableName) ? TableName : throw new ArgumentException("error Input"))} WHERE {colName} >= @FirstTime AND {colName} < DATE_ADD(@SecondTime, INTERVAL 1 DAY);";
        }

        public string GetByMonthAndYear(string colName)
        {
            if (!_colService.IsValidColumn(TableName, colName))
                return "";
            return $"SELECT * FROM {(IsValidStringInputDB(TableName) ? TableName : throw new ArgumentException("error Input"))} WHERE YEAR({colName}) = @FirstTime AND MONTH({colName}) = @SecondTime;";
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
