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
    internal class ConversationPaticipantDAO(IDbConnectionFactory connectionFactory) : BaseDAO<ConversationParticipantModel>(connectionFactory), IGetAllByIdAsync<ConversationParticipantModel>,
                                            IGetSingleByIdsAsync<ConversationParticipantModel>, IDeleteByIdsAsync, IGetDataByDateTime<ConversationParticipantModel>
    {
        protected override string TableName => "conversation_participant";
        protected override string ColumnIdName => "conversation_id";
        private static string SecondColumnIdName => "user_id";
        private readonly IStringConverter _converter = App.SystemServices.GetService<IStringConverter>();
        public record ConversationParticipantItemIds(string ConversationId, string UserId);

        protected override string GetInsertQuery()
        {
            return $@"INSERT INTO {TableName} ({ColumnIdName}, {SecondColumnIdName}, last_read_time, last_join_time) 
                        VALUES(@{_converter.SnakeCaseToPascalCase(ColumnIdName)}, @{_converter.SnakeCaseToPascalCase(SecondColumnIdName)}, @LastReadTime, @LastJoinTime); SELECT LAST_INSERT_ID();";
        }

        protected override string GetUpdateQuery()
        {
            return $@"UPDATE {TableName}
                        SET last_read_time = @LastReadTime, last_join_time = @LastJoinTime  
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

        public async Task<List<ConversationParticipantModel>> GetAllByIdAsync(string id, string colIdName)
        {
            try
            {
                string query = GetByIdQuery(colIdName);
                using IDbConnection connection = connectionFactory.CreateConnection();
                IEnumerable<ConversationParticipantModel> result = await connection.QueryAsync<ConversationParticipantModel>(query, new { Id = id });
                return result.AsList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return new();
            }
        }

        public async Task<ConversationParticipantModel> GetSingleByIdAsync(object keys)
        {
            if (keys is ConversationParticipantItemIds)
            {
                try
                {
                    string query = GetSingleDataString();
                    using IDbConnection connection = connectionFactory.CreateConnection();
                    ConversationParticipantModel result = await connection.QueryFirstOrDefaultAsync<ConversationParticipantModel>(query, keys);
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

        public async Task<List<ConversationParticipantModel>> GetAllByTimeRange<TEnum>(string firstInputTime, string secondInputTime, string colName, TEnum timeType) where TEnum : Enum
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
                    IEnumerable<ConversationParticipantModel> result = await connection.QueryAsync<ConversationParticipantModel>(query, new { FirstTime = firstInputTime, SecondTime = secondInputTime });
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

        public async Task<List<ConversationParticipantModel>> GetAllByTime<TEnum>(string time, string colName, TEnum timeType) where TEnum : Enum
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
                    IEnumerable<ConversationParticipantModel> result = await connection.QueryAsync<ConversationParticipantModel>(query, new { Input = time });
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
