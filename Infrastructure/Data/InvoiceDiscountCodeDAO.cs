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
    public record InvoiceDiscountCodeItemIds(string InvoiceId, string DiscountCodeId);
    public class InvoiceDiscountCodeDAO(IDbConnectionFactory connectionFactory, IColumnService colService, IStringConverter converter, IStringChecker checker)
        : BaseDAO<InvoiceDiscountCodeModel>(connectionFactory, colService, converter, checker, "discount_code_of_invoices", "invoice_id", "discount_code"),
        IGetAllByIdAsync<InvoiceDiscountCodeModel>, IGetSingleByIdsAsync<InvoiceDiscountCodeModel>
    {
        protected override string GetInsertQuery()
        {
            return $@"INSERT INTO {TableName} ({ColumnIdName}, {SecondColumnIdName}) 
                        VALUES(@{Converter.SnakeCaseToPascalCase(ColumnIdName)}, @{Converter.SnakeCaseToPascalCase(ColumnIdName)}); SELECT LAST_INSERT_ID();";
        }

        protected override string GetUpdateQuery()
        {
            return $@""; // No update query needed for this DAO, as the discount code is typically set at creation
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

        public override Task<int> UpdateAsync(InvoiceDiscountCodeModel entity)
        {
            return Task.FromResult(-1); // Soft delete is handled in SoftDeleteAsync
        }

        public override Task<int> UpdateManyAsync(IEnumerable<InvoiceDiscountCodeModel> entities)
        {
            return Task.FromResult(-1); // Soft delete is handled in SoftDeleteAsync
        }

        public string GetSingleDataString()
        {
            return $"SELECT * FROM {TableName} WHERE {ColumnIdName} = {Converter.SnakeCaseToPascalCase(ColumnIdName)} AND {SecondColumnIdName} = {Converter.SnakeCaseToPascalCase(SecondColumnIdName)}";
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
