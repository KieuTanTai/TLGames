using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TLGames.Core.Entities;
using TLGames.Core.Interfaces.IData;
using TLGames.Core.Interfaces.IValidate;

namespace TLGames.Infrastructure.Data
{
    internal class ProductSystemRequirementDAO(IDbConnectionFactory connectionFactory, IColumnService colService, IStringConverter converter, IStringChecker checker)
        : BaseDAO<ProductSystemRequirementModel>(connectionFactory, colService, converter, checker, "system_requirements", "system_requirement_id", null), IGetRelativeAsync<ProductSystemRequirementModel>
    {
        protected override string GetInsertQuery()
        {
            return $@"INSERT INTO {(IsValidStringInputDB(TableName) ? TableName : throw new ArgumentException("error Input"))} (product_id, minimum_os, recommend_os, minimum_processor, recommend_processor, 
                    minimum_memory, recommend_memory, minimum_graphics, recommend_graphics, minimum_directX, recommend_directX, minimum_storage, recommend_storage)
             VALUES(@ProductId, @MinimumOs, @RecommendOs, @MinimumProcessor, @RecommendProcessor, @MinimumMemory, 
                    @RecommendMemory, @MinimumGraphics, @RecommendGraphics, @MinimumDirectX, @RecommendDirectX, @MinimumStorage, @RecommendStorage); SELECT LAST_INSERT_ID();";
        }

        protected override string GetUpdateQuery()
        {
            return $@"UPDATE {(IsValidStringInputDB(TableName) ? TableName : throw new ArgumentException("error Input"))}
             SET minimum_os = @MinimumOs,
                 recommend_os = @RecommendOs,
                 minimum_processor = @MinimumProcessor,
                 recommend_processor = @RecommendProcessor,
                 minimum_memory = @MinimumMemory,
                 recommend_memory = @RecommendMemory,
                 minimum_graphics = @MinimumGraphics,
                 recommend_graphics = @RecommendGraphics,
                 minimum_directX = @MinimumDirectX,
                 recommend_directX = @RecommendDirectX,
                 minimum_storage = @MinimumStorage,
                 recommend_storage = @RecommendStorage
             WHERE {(IsValidStringInputDB(ColumnIdName) ? ColumnIdName : throw new ArgumentException("error Input"))} = @{Converter.SnakeCaseToPascalCase(ColumnIdName)}";
        }

        public string GetQueryDataString(string colName)
        {
            if (!ColService.IsValidColumn(TableName, colName))
                return "";
            return $"SELECT * FROM {(IsValidStringInputDB(TableName) ? TableName : throw new ArgumentException("error Input"))} WHERE {colName} LIKE @Input";
        }

        public async Task<List<ProductSystemRequirementModel>> GetRelativeAsync(string input, string colName)
        {
            try
            {
                string query = GetQueryDataString(colName);
                using IDbConnection connection = connectionFactory.CreateConnection();
                if (!input.Contains('%'))
                    input = $"%{input}%";
                IEnumerable<ProductSystemRequirementModel> result = await connection.QueryAsync<ProductSystemRequirementModel>(query, new { Input = input });
                return result.AsList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return new();
            }
        }
    }
}
