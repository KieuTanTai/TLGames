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
    public record DetailSaleEventItemIds(string SaleEventId, string ProductId);
    internal class DetailSaleEventDAO(IDbConnectionFactory connectionFactory, IColumnService colService, IStringConverter converter, IStringChecker checker)
        : BaseDAO<ConversationParticipantModel>(connectionFactory, colService, converter, checker, "detail_sale_events", "sale_event_id", "product_id"), 
        IGetAllByIdAsync<DetailSaleEventModel>, IGetSingleByIdsAsync<DetailSaleEventModel>, IGetDataByEnum<DetailSaleEventModel>
    {
        protected override string GetInsertQuery()
        {
            return $@"INSERT INTO {(IsValidStringInputDB(TableName) ? TableName : throw new ArgumentException("error Input"))} ({(IsValidStringInputDB(ColumnIdName) ? ColumnIdName : throw new ArgumentException("error Input"))}, {(IsValidStringInputDB(SecondColumnIdName) ? SecondColumnIdName : throw new ArgumentException("error Input"))}, discount_percent, discount_amount, max_discount_price, min_price_to_use) 
                        VALUES(@{Converter.SnakeCaseToPascalCase(ColumnIdName)}, @{Converter.SnakeCaseToPascalCase(ColumnIdName)}, @DiscountPercent, @DiscountAmount, @MaxDiscountPrice, @MinPriceToUse); SELECT LAST_INSERT_ID();";
        }

        protected override string GetUpdateQuery()
        {
            return $@"UPDATE {(IsValidStringInputDB(TableName) ? TableName : throw new ArgumentException("error Input"))}
                        SET discount_percent = @DiscountPercent, 
                        discount_amount = @DiscountAmount, max_discount_price = @MaxDiscountPrice, min_price_to_use = @MinPriceToUse
                        WHERE {(IsValidStringInputDB(ColumnIdName) ? ColumnIdName : throw new ArgumentException("error Input"))} = @{Converter.SnakeCaseToPascalCase(ColumnIdName)} 
                        AND {(IsValidStringInputDB(SecondColumnIdName) ? SecondColumnIdName : throw new ArgumentException("error Input"))} = @{Converter.SnakeCaseToPascalCase(SecondColumnIdName)}";
        }
        public string GetSingleDataString()
        {
            return $"SELECT * FROM {(IsValidStringInputDB(TableName) ? TableName : throw new ArgumentException("error Input"))} WHERE {(IsValidStringInputDB(ColumnIdName) ? ColumnIdName : throw new ArgumentException("error Input"))} = {Converter.SnakeCaseToPascalCase(ColumnIdName)} AND {(IsValidStringInputDB(SecondColumnIdName) ? SecondColumnIdName : throw new ArgumentException("error Input"))} = {Converter.SnakeCaseToPascalCase(SecondColumnIdName)}";
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
