using System;
using System.Threading.Tasks;
using TLGames.Core.Entities;
using TLGames.Core.Interfaces.IData;
using TLGames.Core.Interfaces.IValidate;
using TLGames.Infrastructure.Persistence;

namespace TLGames.Infrastructure.Data
{
    public class PlatformRuleDAO(IDbConnectionFactory connectionFactory, IColumnService colService, IStringConverter converter, IStringChecker checker)
        : BaseDAO<PlatformRuleModel>(connectionFactory, colService, converter, checker, "platform_rules", "platform_rule_id", null), ISoftDeleteAsync<PlatformRuleModel>
    {
        protected override string GetInsertQuery()
        {
            return $@"INSERT INTO {(IsValidStringInputDB(TableName) ? TableName : throw new ArgumentException("error Input"))} (fee, pending_time) 
                        VALUES(@Fee, @PendingTime); SELECT LAST_INSERT_ID();";
        }

        protected override string GetUpdateQuery()
        {
            return $@"UPDATE {(IsValidStringInputDB(TableName) ? TableName : throw new ArgumentException("error Input"))}
                        SET fee = @Fee, pending_time = @PendingTime
                        WHERE {(IsValidStringInputDB(ColumnIdName) ? ColumnIdName : throw new ArgumentException("error Input"))} = @{Converter.SnakeCaseToPascalCase(ColumnIdName)}";
        }

        protected override string DeleteByIdQuery(string colIdName)
        {
            return "";
        }
        public async Task<bool> SoftDeleteAsync(PlatformRuleModel entity)
        {
            return await UpdateAsync(entity);
        }
    }
}
