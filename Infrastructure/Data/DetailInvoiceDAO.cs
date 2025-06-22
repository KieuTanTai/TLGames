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
using static TLGames.Infrastructure.Data.ConversationPaticipantDAO;

namespace TLGames.Infrastructure.Data
{
    internal class DetailInvoiceDAO(IDbConnectionFactory connectionFactory) : BaseDAO<DetailInvoiceModel>(connectionFactory), ISoftDeleteAsync<DetailInvoiceModel>, 
                                    IGetSingleByIdsAsync<DetailInvoiceModel>, IGetAllByIdAsync<DetailInvoiceModel>, IGetDataByEnum<DetailInvoiceModel>
    {
        protected override string TableName => "detail_invoices";

        protected override string ColumnIdName => "invoice_id";
        private static string SecondColumnIdName => "product_id";
        private readonly IStringConverter _converter = App.SystemServices.GetService<IStringConverter>();
        public record DetailInvoiceItemIds(string InvoiceId, string ProductId);

        protected override string GetInsertQuery()
        {
            return $@"INSERT INTO {TableName} ({ColumnIdName}, {SecondColumnIdName}, quantity, price, status) 
                        VALUES(@{_converter.SnakeCaseToPascalCase(ColumnIdName)}, @{_converter.SnakeCaseToPascalCase(ColumnIdName)}, @Quantity, @Price, @Status); SELECT LAST_INSERT_ID();";
        }

        protected override string GetUpdateQuery()
        {
            return $@"UPDATE {TableName}
                        SET status = @Status
                        WHERE {ColumnIdName} = @{_converter.SnakeCaseToPascalCase(ColumnIdName)} 
                        AND {SecondColumnIdName} = @{_converter.SnakeCaseToPascalCase(ColumnIdName)}";
        }

        public string GetSingleDataString()
        {
            return $"SELECT * FROM {TableName} WHERE {ColumnIdName} = {_converter.SnakeCaseToPascalCase(ColumnIdName)} AND {SecondColumnIdName} = {_converter.SnakeCaseToPascalCase(SecondColumnIdName)}";
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
