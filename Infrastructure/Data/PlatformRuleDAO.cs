using System.Threading.Tasks;
using TLGames.Applications.Services;
using TLGames.Core.Entities;
using TLGames.Core.Interfaces.IData;

namespace TLGames.Infrastructure.Data
{
    internal class PlatformRuleDAO(IDbConnectionFactory connectionFactory) : BaseDAO<PlatformRuleModel>(connectionFactory), ISoftDeleteAsync<PlatformRuleModel>
    {
        protected override string TableName => "platform_rules";

        protected override string ColumnIdName => "platform_rule_id";


        protected override string GetInsertQuery()
        {
            return $@"INSERT INTO {(IsValidStringInputDB(TableName) ? TableName : throw new ArgumentException("error Input"))} (fee, pending_time) 
                        VALUES(@Fee, @PendingTime); SELECT LAST_INSERT_ID();";
        }

        protected override string GetUpdateQuery()
        {
            return $@"UPDATE {(IsValidStringInputDB(TableName) ? TableName : throw new ArgumentException("error Input"))}
                        SET fee = @Fee, pending_time = @PendingTime
                        WHERE {(IsValidStringInputDB(ColumnIdName) ? ColumnIdName : throw new ArgumentException("error Input"))} = @{_converter.SnakeCaseToPascalCase(ColumnIdName)}";
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
