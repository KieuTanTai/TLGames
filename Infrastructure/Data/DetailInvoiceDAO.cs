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
    public record DetailInvoiceItemIds(string InvoiceId, string ProductId);

    internal class DetailInvoiceDAO(IDbConnectionFactory connectionFactory, IColumnService colService, IStringConverter converter, IStringChecker checker)
        : BaseDAO<DetailInvoiceModel>(connectionFactory, colService, converter, checker, "detail_invoices", "invoice_id", "product_id"), 
        ISoftDeleteAsync<DetailInvoiceModel>, IGetSingleByIdsAsync<DetailInvoiceModel>, IGetAllByIdAsync<DetailInvoiceModel>, IGetDataByEnum<DetailInvoiceModel>
    {
        protected override string GetInsertQuery()
        {
            return $@"INSERT INTO {(IsValidStringInputDB(TableName) ? TableName : throw new ArgumentException("error Input"))} ({(IsValidStringInputDB(ColumnIdName) ? ColumnIdName : throw new ArgumentException("error Input"))}, {(IsValidStringInputDB(SecondColumnIdName) ? SecondColumnIdName : throw new ArgumentException("error Input"))}, quantity, price, status) 
                        VALUES(@{Converter.SnakeCaseToPascalCase(ColumnIdName)}, @{Converter.SnakeCaseToPascalCase(ColumnIdName)}, @Quantity, @Price, @Status); SELECT LAST_INSERT_ID();";
        }

        protected override string GetUpdateQuery()
        {
            return $@"UPDATE {(IsValidStringInputDB(TableName) ? TableName : throw new ArgumentException("error Input"))}
                        SET status = @Status
                        WHERE {(IsValidStringInputDB(ColumnIdName) ? ColumnIdName : throw new ArgumentException("error Input"))} = @{Converter.SnakeCaseToPascalCase(ColumnIdName)} 
                        AND {(IsValidStringInputDB(SecondColumnIdName) ? SecondColumnIdName : throw new ArgumentException("error Input"))} = @{Converter.SnakeCaseToPascalCase(ColumnIdName)}";
        }

        public string GetSingleDataString()
        {
            return $"SELECT * FROM {(IsValidStringInputDB(TableName) ? TableName : throw new ArgumentException("error Input"))} WHERE {(IsValidStringInputDB(ColumnIdName) ? ColumnIdName : throw new ArgumentException("error Input"))} = {Converter.SnakeCaseToPascalCase(ColumnIdName)} AND {(IsValidStringInputDB(SecondColumnIdName) ? SecondColumnIdName : throw new ArgumentException("error Input"))} = {Converter.SnakeCaseToPascalCase(SecondColumnIdName)}";
        }

        protected override string DeleteByIdQuery(string colIdName)
        {
            return "";
        }

        public async Task<bool> SoftDeleteAsync(DetailInvoiceModel entity)
        {
            return await UpdateAsync(entity);
        }

        public async Task<List<DetailInvoiceModel>> GetAllByIdAsync(string id, string colIdName)
        {
            try
            {
                string query = GetByIdQuery(colIdName);
                using IDbConnection connection = connectionFactory.CreateConnection();
                IEnumerable<DetailInvoiceModel> result = await connection.QueryAsync<DetailInvoiceModel>(query, new { Id = id });
                return result.AsList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return new();
            }
        }

        public async Task<DetailInvoiceModel> GetSingleByIdAsync(object keys)
        {
            if (keys is DetailInvoiceItemIds)
            {
                try
                {
                    string query = GetSingleDataString();
                    using IDbConnection connection = connectionFactory.CreateConnection();
                    DetailInvoiceModel result = await connection.QueryFirstOrDefaultAsync<DetailInvoiceModel>(query, keys);
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
        public async Task<List<DetailInvoiceModel>> GetAllByEnum<TEnum>(TEnum value, string colName) where TEnum : Enum
        {
            if (value is EDetailStatusInvoice)
            {
                try
                {
                    string query = GetByIdQuery(colName);
                    using IDbConnection connection = connectionFactory.CreateConnection();
                    IEnumerable<DetailInvoiceModel> result = await connection.QueryAsync<DetailInvoiceModel>(query, new { Id = value });
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
