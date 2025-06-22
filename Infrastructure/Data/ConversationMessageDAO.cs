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
    internal class ConversationMessageDAO(IDbConnectionFactory connectionFactory) : BaseDAO<ConversationMessageModel>(connectionFactory),
                    IGetAllByIdAsync<ConversationMessageModel>, IGetRelativeAsync<ConversationMessageModel>, IGetDataByDateTime<ConversationMessageModel>, IGetDataByEnum<ConversationMessageModel>
    {
        protected override string TableName => "messages";
        protected override string ColumnIdName => "message_id";
        private readonly IStringConverter _converter = App.SystemServices.GetService<IStringConverter>();

        protected override string GetInsertQuery()
        {
            return $@"INSERT INTO {TableName} (replied_message_id, send_user_id, conversation_id, content, send_time, message_type, attachment_url) 
                        VALUES(@RepliedMessageId, @SendUserId, @ConversationId, @Content, @SendTime, @MessageType, @AttachmentUrl); SELECT LAST_INSERT_ID();";
        }

        protected override string GetUpdateQuery()
        {
            return $@"UPDATE {TableName}
                        SET content = @Content, send_time = @SendTime, message_type = @MessageType, attachment_url = @AttachmentUrl     
                        WHERE {ColumnIdName} = @{_converter.SnakeCaseToPascalCase(ColumnIdName)}";
        }

        public async Task<List<ConversationMessageModel>> GetAllByIdAsync(string id, string colIdName)
        {
            try
            {
                string query = GetByIdQuery(colIdName);
                using IDbConnection connection = connectionFactory.CreateConnection();
                IEnumerable<ConversationMessageModel> result = await connection.QueryAsync<ConversationMessageModel>(query, new { Id = id });
                return result.AsList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return new();
            }
        }

        public async Task<List<ConversationMessageModel>> GetRelativeAsync(string input, string colName)
        {
            try
            {
                string query = GetQueryDataString(colName);
                using IDbConnection connection = connectionFactory.CreateConnection();
                if (!input.Contains('%'))
                    input = $"%{input}%";
                IEnumerable<ConversationMessageModel> result = await connection.QueryAsync<ConversationMessageModel>(query, new { Input = input });
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
            if (!_colService.IsValidColumn(TableName, colName))
                return "";
            return $"SELECT * FROM {TableName} WHERE {colName} LIKE @Input";
        }

        // search by enum
        public async Task<List<ConversationMessageModel>> GetAllByEnum<TEnum>(TEnum value, string colName) where TEnum : Enum
        {
            if (value is EMessageType)
            {
                try
                {
                    string query = GetByIdQuery(colName);
                    using IDbConnection connection = connectionFactory.CreateConnection();
                    IEnumerable<ConversationMessageModel> result = await connection.QueryAsync<ConversationMessageModel>(query, new { Id = value });
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

        public async Task<List<ConversationMessageModel>> GetAllByTimeRange<TEnum>(string firstInputTime, string secondInputTime, string colName, TEnum timeType) where TEnum : Enum
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
                    IEnumerable<ConversationMessageModel> result = await connection.QueryAsync<ConversationMessageModel>(query, new { FirstTime = firstInputTime, SecondTime = secondInputTime });
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

        public async Task<List<ConversationMessageModel>> GetAllByTime<TEnum>(string time, string colName, TEnum timeType) where TEnum : Enum
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
                    IEnumerable<ConversationMessageModel> result = await connection.QueryAsync<ConversationMessageModel>(query, new { Input = time });
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
