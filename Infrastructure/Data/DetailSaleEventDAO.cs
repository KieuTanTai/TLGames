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
using static TLGames.Infrastructure.Data.DetailInvoiceDAO;

namespace TLGames.Infrastructure.Data
{
    internal class DetailSaleEventDAO(IDbConnectionFactory connectionFactory) : BaseDAO<DetailSaleEventModel>(connectionFactory), IGetAllByIdAsync<DetailSaleEventModel>, 
                                        IGetSingleByIdsAsync<DetailSaleEventModel>, IGetDataByEnum<DetailSaleEventModel>
    {
        protected override string TableName => "detail_sale_events";

        protected override string ColumnIdName => "sale_event_id";
        private static string SecondColumnIdName => "product_id";
        private readonly IStringConverter _converter = App.SystemServices.GetService<IStringConverter>();
        public record DetailSaleEventItemIds(string SaleEventId, string ProductId);

        protected override string GetInsertQuery()
        {
            return $@"INSERT INTO {TableName} ({ColumnIdName}, {SecondColumnIdName}, discount_percent, discount_amount, max_discount_price, min_price_to_use) 
                        VALUES(@{_converter.SnakeCaseToPascalCase(ColumnIdName)}, @{_converter.SnakeCaseToPascalCase(ColumnIdName)}, @DiscountPercent, @DiscountAmount, @MaxDiscountPrice, @MinPriceToUse); SELECT LAST_INSERT_ID();";
        }

        protected override string GetUpdateQuery()
        {
            return $@"UPDATE {TableName}
                        SET discount_percent = @DiscountPercent, 
                        discount_amount = @DiscountAmount, max_discount_price = @MaxDiscountPrice, min_price_to_use = @MinPriceToUse
                        WHERE {ColumnIdName} = @{_converter.SnakeCaseToPascalCase(ColumnIdName)} 
                        AND {SecondColumnIdName} = @{_converter.SnakeCaseToPascalCase(SecondColumnIdName )}";
        }
        public string GetSingleDataString()
        {
            return $"SELECT * FROM {TableName} WHERE {ColumnIdName} = {_converter.SnakeCaseToPascalCase(ColumnIdName)} AND {SecondColumnIdName} = {_converter.SnakeCaseToPascalCase(SecondColumnIdName)}";
        }

        protected override string DeleteByIdQuery(string colIdName)
        {
            return "";
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
        public async Task<List<DetailSaleEventModel>> GetAllByEnum<TEnum>(TEnum value, string colName) where TEnum : Enum
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
