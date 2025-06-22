using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using TLGames.Applications.Services;
using TLGames.Core.Entities;
using TLGames.Core.Interfaces;
using TLGames.Infrastructure.Services;

namespace TLGames.Infrastructure.Data
{
    internal class PlatformRuleDAO(IDbConnectionFactory connectionFactory) : BaseDAO<PlatformRuleModel>(connectionFactory), ISoftDeleteAsync<PlatformRuleModel>
    {
        protected override string TableName => "platform_rules";

        protected override string ColumnIdName => "platform_rule_id";
        private readonly IStringConverter _converter = App.SystemServices.GetService<IStringConverter>();

        protected override string GetInsertQuery()
        {
            return $@"INSERT INTO {TableName} (fee, pending_time) 
                        VALUES(@Fee, @PendingTime); SELECT LAST_INSERT_ID();";
        }

        protected override string GetUpdateQuery()
        {
            return $@"UPDATE {TableName}
                        SET fee = @Fee, pending_time = @PendingTime
                        WHERE {ColumnIdName} = @{_converter.SnakeCaseToPascalCase(ColumnIdName)}";
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
