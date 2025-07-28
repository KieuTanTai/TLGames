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
    public class CategoryDAO(IDbConnectionFactory connectionFactory, IColumnService colService, IStringConverter converter, IStringChecker checker)
        : BaseDAO<CategoryModel>(connectionFactory, colService, converter, checker, "categories", "category_id", null),
        IGetRelativeAsync<CategoryModel>, IGetDataByEnumAsync<CategoryModel>
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

        protected override string DeleteByIdQuery(string colIdName)
        {
            return ""; // Soft delete is handled in DeleteAsync 
        }

        public async override Task<int> DeleteAsync(string id)
        {
            CategoryModel category = await GetByIdAsync(id);
            if (category == null)
                return -1;
            category.SetStatus(EActiveStatus.INACTIVE);
            return await UpdateAsync(category);
        }

        public override async Task<int> DeleteManyAsync(IEnumerable<string> ids)
        {
            if (ids == null || !ids.Any())
                return -1;

            List<CategoryModel> categoriesToUpdate = new List<CategoryModel>();

            foreach (string id in ids)
            {
                CategoryModel category = await GetByIdAsync(id);
                if (category == null)
                    return -1;
                category.SetStatus(EActiveStatus.INACTIVE);
                categoriesToUpdate.Add(category);
            }
            return await UpdateManyAsync(categoriesToUpdate);
        }

        protected override string GetInsertQuery()
        {
            return $"INSERT INTO {TableName}(category_name, status) VALUES (@CategoryName, @Status)";
        }

        public string GetQueryDataString(string colName)
        {
            if (!ColService.IsValidColumn(TableName, colName))
                return "";
            return $"SELECT * FROM {TableName} WHERE {colName} LIKE @Input";
        }

        protected override string GetUpdateQuery()
        {
            return $@"UPDATE FROM {TableName} 
                        SET category_name=@CategoryName, status = @Status
                        WHERE {ColumnIdName}=@{Converter.SnakeCaseToPascalCase(ColumnIdName)}";
        }

        // search by enum
        public async Task<List<CategoryModel>> IGetAllByEnumAsync<TEnum>(TEnum value, string colName) where TEnum : Enum
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
