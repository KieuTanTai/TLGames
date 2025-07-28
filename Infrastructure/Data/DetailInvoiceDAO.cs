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
    public record DetailInvoiceItemIds(string InvoiceId, string ProductId);

    public class DetailInvoiceDAO(IDbConnectionFactory connectionFactory, IColumnService colService, IStringConverter converter, IStringChecker checker)
        : BaseDAO<DetailInvoiceModel>(connectionFactory, colService, converter, checker, "detail_invoices", "invoice_id", "product_id"), IGetSingleByIdsAsync<DetailInvoiceModel>,
        IGetAllByIdAsync<DetailInvoiceModel>, IGetDataByEnumAsync<DetailInvoiceModel>
    {
        protected override string GetInsertQuery()
        {
            return $@"INSERT INTO {TableName} ({ColumnIdName}, {SecondColumnIdName}, quantity, price, status) 
                        VALUES(@{Converter.SnakeCaseToPascalCase(ColumnIdName)}, @{Converter.SnakeCaseToPascalCase(ColumnIdName)}, @Quantity, @Price, @Status); SELECT LAST_INSERT_ID();";
        }

        protected override string GetUpdateQuery()
        {
            return $@"UPDATE {TableName}
                        SET status = @Status
                        WHERE {ColumnIdName} = @{Converter.SnakeCaseToPascalCase(ColumnIdName)} 
                        AND {SecondColumnIdName} = @{Converter.SnakeCaseToPascalCase(ColumnIdName)}";
        }

        public string GetSingleDataString()
        {
            return $"SELECT * FROM {TableName} WHERE {ColumnIdName} = {Converter.SnakeCaseToPascalCase(ColumnIdName)} AND {SecondColumnIdName} = {Converter.SnakeCaseToPascalCase(SecondColumnIdName)}";
        }

        protected override string DeleteByIdQuery(string colIdName)
        {
            return "";
        }

        public async override Task<int> DeleteAsync(string id)
        {
            DetailInvoiceModel detailInvoice = await GetByIdAsync(id);
            if (detailInvoice == null)
                return -1;
            detailInvoice.SetStatus(EInvoiceStatus.CANCEL);
            return await UpdateAsync(detailInvoice);
        }

        public override async Task<int> DeleteManyAsync(IEnumerable<string> ids)
        {
            if (ids == null || !ids.Any())
                return -1;
            List<DetailInvoiceModel> detailInvoiceToUpdate = new List<DetailInvoiceModel>();

            foreach (string id in ids)
            {
                DetailInvoiceModel detailInvoice = await GetByIdAsync(id);
                if (detailInvoice == null)
                    return -1;
                detailInvoice.SetStatus(EInvoiceStatus.CANCEL);
                detailInvoiceToUpdate.Add(detailInvoice);
            }
            return await UpdateManyAsync(detailInvoiceToUpdate);
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
        public async Task<List<DetailInvoiceModel>> IGetAllByEnumAsync<TEnum>(TEnum value, string colName) where TEnum : Enum
        {
            if (value is EInvoiceStatus)
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
