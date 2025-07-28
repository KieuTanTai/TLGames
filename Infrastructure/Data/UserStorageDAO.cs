using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TLGames.Core.Entities;
using TLGames.Core.Enums;
using TLGames.Core.Interfaces.IData;
using TLGames.Core.Interfaces.IValidate;
using TLGames.Infrastructure.Persistence;

namespace TLGames.Infrastructure.Data
{
    public class UserStorageDAO(IDbConnectionFactory connectionFactory, IColumnService colService, IStringConverter converter, IStringChecker checker)
        : BaseDAO<UserStorageModel>(connectionFactory, colService, converter, checker, "user_storages", "user_storage_id", null)
    {
        protected override string GetInsertQuery()
        {
            return $@"INSERT INTO {TableName} (user_id) 
                        VALUES(@UserId); SELECT LAST_INSERT_ID();";
        }

        protected override string GetUpdateQuery()
        {
            return ""; // User storage does not require updates in this context
        }

        protected override string DeleteByIdQuery(string colIdName)
        {
            return ""; // Soft delete is handled in DeleteAsync
        }

        public override Task<int> DeleteAsync(string id)
        {
            return Task.FromResult(-1); // User storage does not support deletion
        }

        public override Task<int> DeleteManyAsync(IEnumerable<string> ids)
        {
            return Task.FromResult(-1); // User storage does not support bulk deletion
        }

        public override Task<int> UpdateAsync(UserStorageModel entity)
        {
            return Task.FromResult(-1); // Soft delete is handled in SoftDeleteAsync
        }

        public override Task<int> UpdateManyAsync(IEnumerable<UserStorageModel> entities)
        {
            return Task.FromResult(-1); // Soft delete is handled in SoftDeleteAsync
        }
    }
}
