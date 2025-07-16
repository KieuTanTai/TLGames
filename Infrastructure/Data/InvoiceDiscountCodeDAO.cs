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
    public record InvoiceDiscountCodeItemIds(string InvoiceId, string DiscountCodeId);
    internal class InvoiceDiscountCodeDAO(IDbConnectionFactory connectionFactory, IColumnService colService, IStringConverter converter, IStringChecker checker)
        : BaseDAO<InvoiceDiscountCodeModel>(connectionFactory, colService, converter, checker, "discount_code_of_invoices", "invoice_id", "discount_code"),
                                        IGetAllByIdAsync<InvoiceDiscountCodeModel>, IGetSingleByIdsAsync<InvoiceDiscountCodeModel>
    {
        protected override string GetInsertQuery()
        {
            return $@"INSERT INTO {(IsValidStringInputDB(TableName) ? TableName : throw new ArgumentException("error Input"))} ({(IsValidStringInputDB(ColumnIdName) ? ColumnIdName : throw new ArgumentException("error Input"))}, {(IsValidStringInputDB(SecondColumnIdName) ? SecondColumnIdName : throw new ArgumentException("error Input"))}) 
                        VALUES(@{Converter.SnakeCaseToPascalCase(ColumnIdName)}, @{Converter.SnakeCaseToPascalCase(ColumnIdName)}); SELECT LAST_INSERT_ID();";
        }

        protected override string GetUpdateQuery()
        {
            return $@"";
        }

        protected override string DeleteByIdQuery(string colIdName)
        {
            return "";
        }

        public string GetSingleDataString()
        {
            return $"SELECT * FROM {(IsValidStringInputDB(TableName) ? TableName : throw new ArgumentException("error Input"))} WHERE {(IsValidStringInputDB(ColumnIdName) ? ColumnIdName : throw new ArgumentException("error Input"))} = {Converter.SnakeCaseToPascalCase(ColumnIdName)} AND {(IsValidStringInputDB(SecondColumnIdName) ? SecondColumnIdName : throw new ArgumentException("error Input"))} = {Converter.SnakeCaseToPascalCase(SecondColumnIdName)}";
        }

        public async Task<List<InvoiceDiscountCodeModel>> GetAllByIdAsync(string id, string colIdName)
        {
            try
            {
                string query = GetByIdQuery(colIdName);
                using IDbConnection connection = connectionFactory.CreateConnection();
                IEnumerable<InvoiceDiscountCodeModel> result = await connection.QueryAsync<InvoiceDiscountCodeModel>(query, new { Id = id });
                return result.AsList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return new();
            }
        }

        public async Task<InvoiceDiscountCodeModel> GetSingleByIdAsync(object keys)
        {
            if (keys is InvoiceDiscountCodeItemIds)
            {
                try
                {
                    string query = GetSingleDataString();
                    using IDbConnection connection = connectionFactory.CreateConnection();
                    InvoiceDiscountCodeModel result = await connection.QueryFirstOrDefaultAsync<InvoiceDiscountCodeModel>(query, keys);
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
    }
}
