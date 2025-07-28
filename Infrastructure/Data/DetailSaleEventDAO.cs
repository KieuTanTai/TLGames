using Dapper;
using MySqlX.XDevAPI.Common;
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
    public record DetailSaleEventItemIds(string SaleEventId, string ProductId);
    public class DetailSaleEventDAO(IDbConnectionFactory connectionFactory, IColumnService colService, IStringConverter converter, IStringChecker checker)
        : BaseDAO<DetailSaleEventModel>(connectionFactory, colService, converter, checker, "detail_sale_events", "sale_event_id", "product_id"), 
        IGetAllByIdAsync<DetailSaleEventModel>, IGetSingleByIdsAsync<DetailSaleEventModel>, IGetDataByEnumAsync<DetailSaleEventModel>
    {
        protected override string GetInsertQuery()
        {
            return $@"INSERT INTO {TableName} ({ColumnIdName}, {SecondColumnIdName}, discount_percent, discount_amount, max_discount_price, min_price_to_use) 
                        VALUES(@{Converter.SnakeCaseToPascalCase(ColumnIdName)}, @{Converter.SnakeCaseToPascalCase(ColumnIdName)}, @DiscountPercent, @DiscountAmount, @MaxDiscountPrice, @MinPriceToUse); SELECT LAST_INSERT_ID();";
        }

        protected override string GetUpdateQuery()
        {
            return $@"UPDATE {TableName}
                        SET discount_percent = @DiscountPercent, 
                        discount_amount = @DiscountAmount, max_discount_price = @MaxDiscountPrice, min_price_to_use = @MinPriceToUse
                        WHERE {ColumnIdName} = @{Converter.SnakeCaseToPascalCase(ColumnIdName)} 
                        AND {SecondColumnIdName} = @{Converter.SnakeCaseToPascalCase(SecondColumnIdName)}";
        }
        public string GetSingleDataString()
        {
            return $"SELECT * FROM {TableName} WHERE {ColumnIdName} = {Converter.SnakeCaseToPascalCase(ColumnIdName)} AND {SecondColumnIdName} = {Converter.SnakeCaseToPascalCase(SecondColumnIdName)}";
        }

        protected override string DeleteByIdQuery(string colIdName)
        {
            return ""; // Soft delete is handled in DeleteAsync
        }

        public override Task<int> DeleteAsync(string id)
        {
            return Task.FromResult(-1);
        }

        public override Task<int> DeleteManyAsync(IEnumerable<string> ids)
        {
            return Task.FromResult(-1);
        }

        public async Task<List<DetailSaleEventModel>> GetAllByIdAsync(string id, string colIdName)
        {
            try
            {
                string query = GetByIdQuery(colIdName);
                using IDbConnection connection = connectionFactory.CreateConnection();
                IEnumerable<DetailSaleEventModel> result = await connection.QueryAsync<DetailSaleEventModel>(query, new { Id = id });
                return result.AsList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return new();
            }
        }

        public async Task<DetailSaleEventModel> GetSingleByIdAsync(object keys)
        {
            if (keys is DetailSaleEventItemIds)
            {
                try
                {
                    string query = GetSingleDataString();
                    using IDbConnection connection = connectionFactory.CreateConnection();
                    DetailSaleEventModel result = await connection.QueryFirstOrDefaultAsync<DetailSaleEventModel>(query, keys);
                    return result;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                    return null;
                }
            }
            return null;
        }

        // search by enum
        public async Task<List<DetailSaleEventModel>> IGetAllByEnumAsync<TEnum>(TEnum value, string colName) where TEnum : Enum
        {
            if (value is EDiscountType)
            {
                try
                {
                    string query = GetByIdQuery(colName);
                    using IDbConnection connection = connectionFactory.CreateConnection();
                    IEnumerable<DetailSaleEventModel> result = await connection.QueryAsync<DetailSaleEventModel>(query, new { Id = value });
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
