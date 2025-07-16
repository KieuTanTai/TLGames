using System;
using TLGames.Core.Entities;
using TLGames.Core.Interfaces.IData;
using TLGames.Core.Interfaces.IValidate;

namespace TLGames.Infrastructure.Data
{
    internal class UserStorageDAO(IDbConnectionFactory connectionFactory, IColumnService colService, IStringConverter converter, IStringChecker checker)
        : BaseDAO<ConversationParticipantModel>(connectionFactory, colService, converter, checker, "user_storages", "user_storage_id", null)
    {
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
