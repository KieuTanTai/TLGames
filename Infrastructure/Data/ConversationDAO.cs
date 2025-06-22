using Dapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TLGames.Applications.Services;
using TLGames.Core.Entities;
using TLGames.Core.Interfaces;

namespace TLGames.Infrastructure.Data
{
    internal class ConversationDAO(IDbConnectionFactory connectionFactory) : BaseDAO<ConversationModel>(connectionFactory), IGetSingleByIdsAsync<ConversationModel>, IGetAllByIdAsync<ConversationModel>
    {
        protected override string TableName => "conversations";
        protected override string ColumnIdName => "conversation_id";
        private readonly IStringConverter _converter = App.SystemServices.GetService<IStringConverter>();

        public record ConversationItemIds(string ConversationId, string FirstUserId, string SecondUserId);

        protected override string GetInsertQuery()
        {
            return $@"INSERT INTO {TableName}(first_user_id, second_user_id, start_time, last_message_time, status) 
                        VALUES (@FirstUserId, @SecondUserId, StartTime, LastMessageTime, Status)";
        }

        protected override string GetUpdateQuery()
        {
            return $@"UPDATE FROM {TableName}
                        SET start_time=@StartTime, last_message_time=@LastMessageTime, status=@Status
                        WHERE {ColumnIdName}=@{_converter.SnakeCaseToPascalCase(ColumnIdName)}";
        }
        public string GetSingleDataString()
        {
            return $@"SELECT * FROM {TableName} WHERE {ColumnIdName} = @{_converter.SnakeCaseToPascalCase(ColumnIdName)}
                        AND first_user_id = @FirstUserId, second_user_id = @SecondUserId";
        }

        public async Task<ConversationModel> GetSingleByIdAsync(object keys)
        {
            if (keys is ConversationItemIds)
            {
                try
                {
                    string query = GetSingleDataString();
                    using IDbConnection connection = connectionFactory.CreateConnection();
                    ConversationModel result = await connection.QueryFirstOrDefaultAsync<ConversationModel>(query, keys);
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

        public async Task<List<ConversationModel>> GetAllByIdAsync(string id, string colIdName)
        {
            try
            {
                string query = GetByIdQuery(colIdName);
                using IDbConnection connection = connectionFactory.CreateConnection();
                IEnumerable<ConversationModel> result = await connection.QueryAsync<ConversationModel>(query, new { Id = id });
                return result.AsList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return new();
            }
        }
    }
}
