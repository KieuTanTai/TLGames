using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TLGames.Core.Entities;
using TLGames.Core.Enums;
using TLGames.Core.Interfaces.IData;
using TLGames.Core.Interfaces.IValidate;
using TLGames.Infrastructure.Persistence;

namespace TLGames.Infrastructure.Data
{
    public class CategoryDAO(IDbConnectionFactory connectionFactory, IColumnService colService, IStringConverter converter, IStringChecker checker)
        : BaseDAO<CategoryModel>(connectionFactory, colService, converter, checker, "categories", "category_id", null),
        IGetRelativeAsync<CategoryModel>, ISoftDeleteAsync<CategoryModel>, IGetDataByEnum<CategoryModel>
    {
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
            if (!ColService.IsValidColumn(TableName, colName))
                return "";
            return $"SELECT * FROM {(IsValidStringInputDB(TableName) ? TableName : throw new ArgumentException("error Input"))} WHERE {colName} LIKE @Input";
        }

        protected override string GetUpdateQuery()
        {
            return $@"UPDATE FROM {(IsValidStringInputDB(TableName) ? TableName : throw new ArgumentException("error Input"))} 
                        SET category_name=@CategoryName, status = @Status
                        WHERE {(IsValidStringInputDB(ColumnIdName) ? ColumnIdName : throw new ArgumentException("error Input"))}=@{Converter.SnakeCaseToPascalCase(ColumnIdName)}";
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
