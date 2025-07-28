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
    public class NewsCategoryDAO(IDbConnectionFactory connectionFactory, IColumnService colService, IStringConverter converter, IStringChecker checker)
        : BaseDAO<NewsCategoryModel>(connectionFactory, colService, converter, checker, "news_categories", "news_category_id", null),
        IGetRelativeAsync<NewsCategoryModel>, IGetDataByEnumAsync<NewsCategoryModel>
    {
        protected override string GetInsertQuery()
        {
            return $@"INSERT INTO {TableName} (category_name, status) 
                        VALUES(@CategoryName, @Status); SELECT LAST_INSERT_ID();";
        }

        protected override string GetUpdateQuery()
        {
            return $@"UPDATE {TableName}
                        SET category_name = @CategoryName, status = @Status
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
            NewsCategoryModel newCategory = await GetByIdAsync(id);
            if (newCategory == null)
                return -1;
            newCategory.SetStatus(EActiveStatus.INACTIVE);
            return await UpdateAsync(newCategory);
        }

        public override async Task<int> DeleteManyAsync(IEnumerable<string> ids)
        {
            if (ids == null || !ids.Any())
                return -1;

            List<NewsCategoryModel> newCategorysToUpdate = new List<NewsCategoryModel>();

            foreach (string id in ids)
            {
                NewsCategoryModel newCategory = await GetByIdAsync(id);
                if (newCategory == null)
                    return -1;
                newCategory.SetStatus(EActiveStatus.INACTIVE);
                newCategorysToUpdate.Add(newCategory);
            }
            return await UpdateManyAsync(newCategorysToUpdate);
        }

        public async Task<List<NewsCategoryModel>> GetRelativeAsync(string input, string colName)
        {
            try
            {
                string query = GetQueryDataString(colName);
                using IDbConnection connection = connectionFactory.CreateConnection();
                if (!input.Contains('%'))
                    input = $"%{input}%";
                IEnumerable<NewsCategoryModel> result = await connection.QueryAsync<NewsCategoryModel>(query, new { Input = input });
                return result.AsList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return new();
            }
        }

        // search by enum
        public async Task<List<NewsCategoryModel>> IGetAllByEnumAsync<TEnum>(TEnum value, string colName) where TEnum : Enum
        {
            if (value is EActiveStatus)
            {
                try
                {
                    string query = GetByIdQuery(colName);
                    using IDbConnection connection = connectionFactory.CreateConnection();
                    IEnumerable<NewsCategoryModel> result = await connection.QueryAsync<NewsCategoryModel>(query, new { Id = value });
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
