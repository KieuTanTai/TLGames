using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TLGames.Core.Entities;
using TLGames.Core.Enums;
using TLGames.Core.Interfaces.IData;
using TLGames.Core.Interfaces.IValidate;
using TLGames.Infrastructure.Persistence;

namespace TLGames.Infrastructure.Data
{
    public class PlatformRuleDAO(IDbConnectionFactory connectionFactory, IColumnService colService, IStringConverter converter, IStringChecker checker)
        : BaseDAO<PlatformRuleModel>(connectionFactory, colService, converter, checker, "platform_rules", "platform_rule_id", null), IGetDataByEnumAsync<PlatformRuleModel>
    {
        protected override string GetInsertQuery()
        {
            return $@"INSERT INTO {TableName} (fee, pending_time) 
                        VALUES(@Fee, @PendingTime); SELECT LAST_INSERT_ID();";
        }

        protected override string GetUpdateQuery()
        {
            return $@"UPDATE {TableName}
                        SET fee = @Fee, pending_time = @PendingTime, status = @Status
                        WHERE {ColumnIdName} = @{Converter.SnakeCaseToPascalCase(ColumnIdName)}";
        }

        protected override string DeleteByIdQuery(string colIdName)
        {
            return ""; // Soft delete is handled in SoftDeleteAsync
        }
        public async override Task<int> DeleteAsync(string id)
        {
            PlatformRuleModel platformRule = await GetByIdAsync(id);
            if (platformRule == null)
                return -1;
            platformRule.SetStatus(EActiveStatus.INACTIVE);
            return await UpdateAsync(platformRule);
        }

        public override async Task<int> DeleteManyAsync(IEnumerable<string> ids)
        {
            if (ids == null || !ids.Any())
                return -1;

            List<PlatformRuleModel> platformRulesForUpdate = new List<PlatformRuleModel>();

            foreach (string id in ids)
            {
                PlatformRuleModel platformRule = await GetByIdAsync(id);
                if (platformRule == null)
                    return -1;
                platformRule.SetStatus(EActiveStatus.INACTIVE);
                platformRulesForUpdate.Add(platformRule);
            }
            return await UpdateManyAsync(platformRulesForUpdate);
        }

        public async Task<List<PlatformRuleModel>> IGetAllByEnumAsync<TEnum>(TEnum value, string colName) where TEnum : Enum
        {
            if (value is EActiveStatus)
            {
                try
                {
                    string query = GetByIdQuery(colName);
                    using IDbConnection connection = connectionFactory.CreateConnection();
                    IEnumerable<PlatformRuleModel> result = await connection.QueryAsync<PlatformRuleModel>(query, new { Id = value });
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
