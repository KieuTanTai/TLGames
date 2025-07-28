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
    public class RoleDAO(IDbConnectionFactory connectionFactory, IColumnService colService, IStringConverter converter, IStringChecker checker)
        : BaseDAO<RoleModel>(connectionFactory, colService, converter, checker, "roles", "role_id", null), 
        IGetRelativeAsync<RoleModel>, IGetDataByEnumAsync<RoleModel>
    {
        protected override string GetInsertQuery()
        {
            return $@"INSERT INTO {TableName} (role_name, status) 
                        VALUES(@RoleName, @Status); SELECT LAST_INSERT_ID();";
        }

        protected override string GetUpdateQuery()
        {
            return $@"UPDATE {TableName}
                        SET role_name = @RoleName, status = @Status
                        WHERE {ColumnIdName} = @{Converter.SnakeCaseToPascalCase(ColumnIdName)}";
        }

        public string GetQueryDataString(string colName)
        {
            if (!ColService.IsValidColumn(TableName, colName))
                return "";
            return $"SELECT * FROM {TableName} WHERE {colName} LIKE @Input";
        }

        protected override string DeleteByIdQuery(string colIdName)
        {
            return ""; // Soft delete is handled in SoftDeleteAsync
        }

        public async override Task<int> DeleteAsync(string id)
        {
            RoleModel role = await GetByIdAsync(id);
            if (role == null)
                return -1;
            role.SetStatus(EActiveStatus.INACTIVE);
            return await UpdateAsync(role);
        }

        public override async Task<int> DeleteManyAsync(IEnumerable<string> ids)
        {
            if (ids == null || !ids.Any())
                return -1;

            List<RoleModel> rolesForUpdate = new List<RoleModel>();

            foreach (string id in ids)
            {
                RoleModel role = await GetByIdAsync(id);
                if (role == null)
                    return -1;
                role.SetStatus(EActiveStatus.INACTIVE);
                rolesForUpdate.Add(role);
            }
            return await UpdateManyAsync(rolesForUpdate);
        }

        public async Task<List<RoleModel>> GetRelativeAsync(string input, string colName)
        {
            try
            {
                string query = GetQueryDataString(colName);
                using IDbConnection connection = connectionFactory.CreateConnection();
                if (!input.Contains('%'))
                    input = $"%{input}%";
                IEnumerable<RoleModel> result = await connection.QueryAsync<RoleModel>(query, new { Input = input });
                return result.AsList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return new();
            }
        }

        public async Task<List<RoleModel>> IGetAllByEnumAsync<TEnum>(TEnum value, string colName) where TEnum : Enum
        {
            if (value is EActiveStatus)
            {
                try
                {
                    string query = GetByIdQuery(colName);
                    using IDbConnection connection = connectionFactory.CreateConnection();
                    IEnumerable<RoleModel> result = await connection.QueryAsync<RoleModel>(query, new { Id = value });
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
