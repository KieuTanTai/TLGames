using Dapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TLGames.Applications.Services;
using TLGames.Core.Entities;
using TLGames.Core.Enums;
using TLGames.Core.Interfaces;

namespace TLGames.Infrastructure.Data
{
    internal class BankDAO(IDbConnectionFactory connectionFactory) : BaseDAO<BankModel>(connectionFactory), IGetRelativeAsync<BankModel>, ISoftDeleteAsync<BankModel>,
                            IGetDataByEnum<BankModel>
    {
        protected override string TableName => "banks";
        protected override string ColumnIdName => "bank_id";
        private readonly IStringConverter _converter = App.SystemServices.GetService<IStringConverter>();
        protected override string GetInsertQuery()
        {
            return $"INSERT INTO {TableName} (bank_name, status) VALUES(@BankName, @Status); SELECT LAST_INSERT_ID();";
        }

        protected override string GetUpdateQuery()
        {
            return $@"UPDATE {TableName}
                        SET bank_name = @BankName, status = @Status     
                        WHERE {ColumnIdName} = @{_converter.SnakeCaseToPascalCase(ColumnIdName)}";
        }

        public string GetQueryDataString(string colName)
        {
            if (!_colService.IsValidColumn(TableName, colName))
                return "";
            return $"SELECT * FROM {TableName} WHERE {colName} LIKE @Input";
        }

        protected override string DeleteByIdQuery(string colIdName)
        {
            return "";
        }

        public async Task<bool> SoftDeleteAsync(BankModel entity)
        {
            return await UpdateAsync(entity);
        }

        public async Task<List<BankModel>> GetRelativeAsync(string input, string colName)
        {
            try
            {
                string query = GetQueryDataString(colName);
                using IDbConnection connection = connectionFactory.CreateConnection();
                if (!input.Contains('%'))
                    input = $"%{input}%";
                IEnumerable<BankModel> result = await connection.QueryAsync<BankModel>(query, new { Input = input });
                return result.AsList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return new();
            }
        }

        // search by enum
        public async Task<List<BankModel>> GetAllByEnum<TEnum>(TEnum value, string colName) where TEnum : Enum
        {
            if (value is EActiveStatus)
            {
                try
                {
                    string query = GetByIdQuery(colName);
                    using IDbConnection connection = connectionFactory.CreateConnection();
                    IEnumerable<BankModel> result = await connection.QueryAsync<BankModel>(query, new { Id = value });
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
