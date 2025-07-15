using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TLGames.Applications.Services;
using TLGames.Core.Entities;
using TLGames.Core.Enums;
using TLGames.Core.Interfaces.IData;

namespace TLGames.Infrastructure.Data
{
    internal class CategoryDAO(IDbConnectionFactory connectionFactory,
                                        string tableName,
                                        string columnIdName) : BaseDAO<CategoryModel>(connectionFactory, tableName, columnIdName), IGetRelativeAsync<CategoryModel>,
                                ISoftDeleteAsync<CategoryModel>, IGetDataByEnum<CategoryModel>
    {
        //protected override string TableName => "categories";
        //protected override string ColumnIdName => "category_id";

        public async Task<List<CategoryModel>> GetRelativeAsync(string input, string colName)
        {
            try
            {
                string query = GetQueryDataString(colName);
                using IDbConnection connection = connectionFactory.CreateConnection();
                if (!input.Contains('%'))
                    input = $"%{input}%";
                IEnumerable<CategoryModel> result = await connection.QueryAsync<CategoryModel>(query, new { Input = input });
                return result.AsList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return new();
            }
        }

        protected override string DeleteByIdQuery(string colIdName)
        {
            return "";
        }

        public async Task<bool> SoftDeleteAsync(CategoryModel entity)
        {
            return await UpdateAsync(entity);
        }

        protected override string GetInsertQuery()
        {
            return $"INSERT INTO {(IsValidStringInputDB(TableName) ? TableName : throw new ArgumentException("error Input"))}(category_name, status) VALUES (@CategoryName, @Status)";
        }

        public string GetQueryDataString(string colName)
        {
            if (!_colService.IsValidColumn(TableName, colName))
                return "";
            return $"SELECT * FROM {(IsValidStringInputDB(TableName) ? TableName : throw new ArgumentException("error Input"))} WHERE {colName} LIKE @Input";
        }

        protected override string GetUpdateQuery()
        {
            return $@"UPDATE FROM {(IsValidStringInputDB(TableName) ? TableName : throw new ArgumentException("error Input"))} 
                        SET category_name=@CategoryName, status = @Status
                        WHERE {(IsValidStringInputDB(ColumnIdName) ? ColumnIdName : throw new ArgumentException("error Input"))}=@{_converter.SnakeCaseToPascalCase(ColumnIdName)}";
        }

        // search by enum
        public async Task<List<CategoryModel>> GetAllByEnum<TEnum>(TEnum value, string colName) where TEnum : Enum
        {
            if (value is EActiveStatus)
            {
                try
                {
                    string query = GetByIdQuery(colName);
                    using IDbConnection connection = connectionFactory.CreateConnection();
                    IEnumerable<CategoryModel> result = await connection.QueryAsync<CategoryModel>(query, new { Id = value });
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
