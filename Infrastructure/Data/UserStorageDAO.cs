using TLGames.Applications.Services;
using TLGames.Core.Entities;
using TLGames.Core.Interfaces.IData;

namespace TLGames.Infrastructure.Data
{
    internal class UserStorageDAO(IDbConnectionFactory connectionFactory) : BaseDAO<UserStorageModel>(connectionFactory)
    {
        protected override string TableName => "user_storages";
        protected override string ColumnIdName => "user_storage_id";

        protected override string GetInsertQuery()
        {
            return $@"INSERT INTO {(IsValidStringInputDB(TableName) ? TableName : throw new ArgumentException("error Input"))} (user_id) 
                        VALUES(@UserId); SELECT LAST_INSERT_ID();";
        }

        protected override string GetUpdateQuery()
        {
            return "";
        }

        protected override string DeleteByIdQuery(string colIdName)
        {
            return "";
        }
    }
}
