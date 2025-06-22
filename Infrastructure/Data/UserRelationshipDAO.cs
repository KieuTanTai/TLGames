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
using TLGames.Infrastructure.Services;
using static TLGames.Infrastructure.Data.FollowerOfPublisherDAO;

namespace TLGames.Infrastructure.Data
{
    internal class UserRelationshipDAO(IDbConnectionFactory connectionFactory) : BaseDAO<UserRelationshipModel>(connectionFactory), IGetAllByIdAsync<UserRelationshipModel>, 
                                        IGetSingleByIdsAsync<UserRelationshipModel>, IGetDataByDateTime<UserRelationshipModel>, IGetDataByEnum<UserRelationshipModel>, IDeleteByIdsAsync
    {
        protected override string TableName => "user_relationships";
        protected override string ColumnIdName => "requester_id";
        private static string SecondColumnIdName => "receiver_id";
        private readonly IStringConverter _converter = App.SystemServices.GetService<IStringConverter>();
        public record UserRelationshipItemIds(string RequesterId, string ReceiverId);

        protected override string GetInsertQuery()
        {
            return $@"INSERT INTO {TableName} ({ColumnIdName}, {SecondColumnIdName}, request_date, accept_date, status) 
                        VALUES(@{_converter.SnakeCaseToPascalCase(ColumnIdName)},
                        @{_converter.SnakeCaseToPascalCase(ColumnIdName)},
                        @RequestDate, @AcceptDate, @Status); SELECT LAST_INSERT_ID();";
        }

        protected override string GetUpdateQuery()
        {
            return $@"UPDATE {TableName}
                        SET request_date = @RequestDate,
                            accept_date = @SAcceptDate, status = @Status
                        WHERE {ColumnIdName} = @{_converter.SnakeCaseToPascalCase(ColumnIdName)}
                        AND {SecondColumnIdName} = @{_converter.SnakeCaseToPascalCase(ColumnIdName)}";
        }

        public string GetDeleteQuery()
        {
            return $"DELETE FROM {TableName} WHERE {ColumnIdName} = @{_converter.SnakeCaseToPascalCase(ColumnIdName)} AND {SecondColumnIdName} = @{_converter.SnakeCaseToPascalCase(SecondColumnIdName)}";
        }

        public string GetSingleDataString()
        {
            return $"SELECT * FROM {TableName} WHERE {ColumnIdName} = {_converter.SnakeCaseToPascalCase(ColumnIdName)} AND {SecondColumnIdName} = {_converter.SnakeCaseToPascalCase(SecondColumnIdName)}";
        }

        public async Task<List<UserRelationshipModel>> GetAllByIdAsync(string id, string colIdName)
        {
            try
            {
                string query = GetByIdQuery(colIdName);
                using IDbConnection connection = connectionFactory.CreateConnection();
                IEnumerable<UserRelationshipModel> result = await connection.QueryAsync<UserRelationshipModel>(query, new { Id = id });
                return result.AsList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return new();
            }
        }

        public async Task<UserRelationshipModel> GetSingleByIdAsync(object keys)
        {
            if (keys is FollowerOfPublisherItemIds)
            {
                try
                {
                    string query = GetSingleDataString();
                    using IDbConnection connection = connectionFactory.CreateConnection();
                    UserRelationshipModel result = await connection.QueryFirstOrDefaultAsync<UserRelationshipModel>(query, keys);
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

        public async Task<bool> DeleteByIdsAsync(object keys)
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

        // search by enum
        public async Task<List<UserRelationshipModel>> GetAllByEnum<TEnum>(TEnum value, string colName) where TEnum : Enum
        {
            if (value is EUserRelationshipStatus)
            {
                try
                {
                    string query = GetByIdQuery(colName);
                    using IDbConnection connection = connectionFactory.CreateConnection();
                    IEnumerable<UserRelationshipModel> result = await connection.QueryAsync<UserRelationshipModel>(query, new { Id = value });
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

        public async Task<List<UserRelationshipModel>> GetAllByTimeRange<TEnum>(string firstInputTime, string secondInputTime, string colName, TEnum timeType) where TEnum : Enum
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
                    IEnumerable<UserRelationshipModel> result = await connection.QueryAsync<UserRelationshipModel>(query, new { FirstTime = firstInputTime, SecondTime = secondInputTime });
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

        public async Task<List<UserRelationshipModel>> GetAllByTime<TEnum>(string time, string colName, TEnum timeType) where TEnum : Enum
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
                    IEnumerable<UserRelationshipModel> result = await connection.QueryAsync<UserRelationshipModel>(query, new { Input = time });
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
