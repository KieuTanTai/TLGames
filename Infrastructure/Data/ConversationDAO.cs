using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TLGames.Core.Entities;
using TLGames.Core.Interfaces.IData;
using TLGames.Core.Interfaces.IValidate;
using TLGames.Infrastructure.Persistence;

namespace TLGames.Infrastructure.Data
{
    public record ConversationItemIds(string ConversationId, string FirstUserId, string SecondUserId);
    public class ConversationDAO(IDbConnectionFactory connectionFactory, IColumnService colService, IStringConverter converter, IStringChecker checker)
        : BaseDAO<ConversationModel>(connectionFactory, colService, converter, checker, "conversations", "conversation_id", null), IGetSingleByIdsAsync<ConversationModel>, IGetAllByIdAsync<ConversationModel>
    {
        protected override string GetInsertQuery()
        {
            return $@"INSERT INTO {(IsValidStringInputDB(TableName) ? TableName : throw new ArgumentException("error Input"))}(first_user_id, second_user_id, start_time, last_message_time, status) 
                        VALUES (@FirstUserId, @SecondUserId, StartTime, LastMessageTime, Status)";
        }

        protected override string GetUpdateQuery()
        {
            return $@"UPDATE FROM {(IsValidStringInputDB(TableName) ? TableName : throw new ArgumentException("error Input"))}
                        SET start_time=@StartTime, last_message_time=@LastMessageTime, status=@Status
                        WHERE {(IsValidStringInputDB(ColumnIdName) ? ColumnIdName : throw new ArgumentException("error Input"))}=@{Converter.SnakeCaseToPascalCase(ColumnIdName)}";
        }
        public string GetSingleDataString()
        {
            return $@"SELECT * FROM {(IsValidStringInputDB(TableName) ? TableName : throw new ArgumentException("error Input"))} WHERE {(IsValidStringInputDB(ColumnIdName) ? ColumnIdName : throw new ArgumentException("error Input"))} = @{Converter.SnakeCaseToPascalCase(ColumnIdName)}
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
