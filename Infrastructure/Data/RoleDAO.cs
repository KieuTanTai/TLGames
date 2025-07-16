using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TLGames.Core.Entities;
using TLGames.Core.Enums;
using TLGames.Core.Interfaces.IData;
using TLGames.Core.Interfaces.IValidate;

namespace TLGames.Infrastructure.Data
{
    internal class RoleDAO(IDbConnectionFactory connectionFactory, IColumnService colService, IStringConverter converter, IStringChecker checker)
        : BaseDAO<RoleModel>(connectionFactory, colService, converter, checker, "roles", "role_id", null), 
        IGetRelativeAsync<RoleModel>, ISoftDeleteAsync<RoleModel>, IGetDataByEnum<RoleModel>
    {
        protected override string GetInsertQuery()
        {
            return $@"INSERT INTO {(IsValidStringInputDB(TableName) ? TableName : throw new ArgumentException("error Input"))} (role_name, status) 
                        VALUES(@RoleName, @Status); SELECT LAST_INSERT_ID();";
        }

        protected override string GetUpdateQuery()
        {
            return $@"UPDATE {(IsValidStringInputDB(TableName) ? TableName : throw new ArgumentException("error Input"))}
                        SET role_name = @RoleName, status = @Status
                        WHERE {(IsValidStringInputDB(ColumnIdName) ? ColumnIdName : throw new ArgumentException("error Input"))} = @{Converter.SnakeCaseToPascalCase(ColumnIdName)}";
        }

        public string GetQueryDataString(string colName)
        {
            if (!ColService.IsValidColumn(TableName, colName))
                return "";
            return $"SELECT * FROM {(IsValidStringInputDB(TableName) ? TableName : throw new ArgumentException("error Input"))} WHERE {colName} LIKE @Input";
        }

        protected override string DeleteByIdQuery(string colIdName)
        {
            return "";
        }

        public async Task<bool> SoftDeleteAsync(RoleModel entity)
        {
            return await UpdateAsync(entity);
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

        public async Task<List<RoleModel>> GetAllByEnum<TEnum>(TEnum value, string colName) where TEnum : Enum
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
