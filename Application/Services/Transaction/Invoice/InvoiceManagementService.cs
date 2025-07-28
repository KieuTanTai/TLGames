using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TLGames.Core.Entities;
using TLGames.Core.Enums;
using TLGames.Core.Interfaces.IData;
using TLGames.Core.Interfaces.IServices.Transaction.Invoice;

namespace TLGames.Application.Services.Transaction.Invoice
{
    internal class InvoiceManagementService : ValidateService<InvoiceModel>, IInvoiceManagementService<InvoiceModel>
    {
        private readonly IDAO<InvoiceModel> _invoiceDAO;
        private readonly IGetAllByIdAsync<InvoiceModel> _getAllByIdAsync;
        private readonly IGetDataByEnumAsync<InvoiceModel> _getDataByEnum;
        private readonly IGetDataByDateTimeAsync<InvoiceModel> _getDataByDateTime;

        public InvoiceManagementService(IDAO<InvoiceModel> invoiceDAO, IGetAllByIdAsync<InvoiceModel> getAllByIdAsync, IGetDataByEnumAsync<InvoiceModel> getDataByEnum, IGetDataByDateTimeAsync<InvoiceModel> getDataByDateTime)
            : base(invoiceDAO)
        {
            _invoiceDAO = invoiceDAO;
            _getAllByIdAsync = getAllByIdAsync;
            _getDataByEnum = getDataByEnum;
            _getDataByDateTime = getDataByDateTime;
        }

        public async Task<List<InvoiceModel>> GetAllAsync()
        {
            return await _invoiceDAO.GetAllAsync();
        }

        public async Task<List<InvoiceModel>> GetAllByIdAsync(string id, string colName = "customer_id")
        {
            CheckNullOrEmpty([id, colName]); // if false it will throw an ArgumentException
            List<InvoiceModel> invoices = await _getAllByIdAsync.GetAllByIdAsync(id, colName);
            if (invoices == null || invoices.Count == 0)
                throw new KeyNotFoundException("No invoices found for the provided IDs.");
            return invoices;
        }

        public async Task<List<InvoiceModel>> IGetAllByEnumAsync<TEnum>(TEnum value, string colName = "status") where TEnum : Enum
        {
            
            CheckNullOrEmpty([colName]); // if false it will throw an ArgumentException
            if (value is not EDataTimeType)
                throw new ArgumentException("Invalid enum type for this method.");
            if (!IsValidStringInputDB(colName))
                throw new ArgumentException("Invalid column name provided.");
            return await _getDataByEnum.IGetAllByEnumAsync(value, colName);
        }

        public async Task<List<InvoiceModel>> GetAllByTimeAsync<TEnum>(string time, TEnum timeType, string colName = "invoice_date") where TEnum : Enum
        {
            CheckNullOrEmpty([colName, time]); // if false it will throw an ArgumentException
            if (timeType is not EDataTimeType)
                throw new ArgumentException("Invalid enum type for this method.");
            if (!IsValidStringInputDB(colName))
                throw new ArgumentException("Invalid column name provided.");
            if (!IsValidTimeFormat(time, timeType))
                throw new ArgumentException("Invalid time type or format provided.");
            return await _getDataByDateTime.GetAllByTimeAsync(time, colName, timeType);
        }

        public async Task<List<InvoiceModel>> GetAllByTimeRangeAsync<TEnum>(string firstInputTime, string secondInputTime, TEnum timeType, string colName = "invoice_date") 
            where TEnum : Enum
        {
            CheckNullOrEmpty([firstInputTime, secondInputTime, colName]); // if false it will throw an ArgumentException
            if (timeType is not EDataTimeType)
                throw new ArgumentException("Invalid enum type for this method.");
            if (!IsValidTimeFormat(firstInputTime, timeType) || !IsValidTimeFormat(secondInputTime, timeType))
                throw new ArgumentException("Invalid time type or format provided.");
            if (!IsValidStringInputDB(colName))
                throw new ArgumentException("Invalid column name provided.");
            return await _getDataByDateTime.GetAllByTimeRangeAsync(firstInputTime, secondInputTime, colName, timeType);
        }

        public async Task<InvoiceModel> GetByIdAsync(string id)
        {
            CheckNullOrEmpty([id]); // if false it will throw an ArgumentException
            InvoiceModel invoice = await _invoiceDAO.GetByIdAsync(id);
            if (invoice == null)
                throw new KeyNotFoundException($"Invoice with ID {id} not found.");
            return invoice;
        }

        public async Task<int> InsertAsync(InvoiceModel entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Invoice entity cannot be null.");
            if (await IsExistObject(entity))
                throw new InvalidOperationException("Invoice with the same ID already exists.");
            int affectRow = await _invoiceDAO.InsertAsync(entity);
            if (affectRow <= 0)
                throw new InvalidOperationException("Failed to insert the invoice.");
            return affectRow;
        }

        public async Task<int> InsertManyAsync(IEnumerable<InvoiceModel> entities)
        {
            if (await IsValidList(entities, true))
            {
                int affectRow = await _invoiceDAO.InsertManyAsync(entities);
                if (affectRow <= 0)
                    throw new InvalidOperationException("Failed to insert one or more invoices.");
                return affectRow;
            }
            throw new ArgumentException("One or more entities are invalid or already exist.");
        }

        public Task<List<InvoiceModel>> GetAllByEnumAsync<TEnum>(TEnum value, string colName = "status") where TEnum : Enum
        {
            if (value is nameof(EInvoiceStatus))
            {
                CheckNullOrEmpty([colName]); // if false it will throw an ArgumentException
                if (!IsValidStringInputDB(colName))
                    throw new ArgumentException("Invalid column name provided.");
                return _getDataByEnum.IGetAllByEnumAsync(value, colName);
            }
            throw new ArgumentException("Invalid enum type for invoice status.", nameof(value));
        }

        //public async Task<int> SoftDeleteAsync<TEnum>(string id, TEnum status) where TEnum : Enum
        //{
        //    CheckNullOrEmpty([id]); // if false it will throw an ArgumentException
        //    if (status is EInvoiceStatus activeStatus)
        //    {
        //        InvoiceModel invoice = await _invoiceDAO.GetByIdAsync(id);
        //        if (invoice == null)
        //            throw new KeyNotFoundException($"invoice with ID {id} not found.");
        //        invoice.SetStatus(activeStatus);
        //        int affectRow = await _invoiceDAO.UpdateAsync(invoice);
        //        if (affectRow <= 0)
        //            throw new InvalidOperationException($"Failed to soft delete invoice with ID {id}.");
        //        return affectRow;
        //    }
        //    throw new ArgumentException("Invalid enum type for invoice status.", nameof(status));
        //}

        //public async Task<int> SoftDeleteManyAsync<TEnum>(IEnumerable<string> ids, TEnum status) where TEnum : Enum
        //{
        //    CheckNullOrEmpty(ids); // if false it will throw an ArgumentException
        //    if (status is EInvoiceStatus activeStatus)
        //    {
        //        List<InvoiceModel> invoicesForUpdate = new List<InvoiceModel>();
        //        foreach (string id in ids)
        //        {
        //            InvoiceModel invoice = await _invoiceDAO.GetByIdAsync(id) ?? throw new KeyNotFoundException($"invoice with ID {id} not found.");
        //            invoice.SetStatus(activeStatus);
        //            invoicesForUpdate.Add(invoice);
        //        }
        //        int affectRow = await _invoiceDAO.UpdateManyAsync(invoicesForUpdate);
        //        if (affectRow <= 0)
        //            throw new InvalidOperationException("Failed to soft delete one or more invoices.");
        //        return affectRow;
        //    }
        //    throw new ArgumentException("Invalid enum type for invoice status.", nameof(status));
        //}

        //public async Task<int> UpdateAsync(InvoiceModel entity)
        //{
        //    if (entity == null)
        //        throw new ArgumentNullException(nameof(entity), "Invoice entity cannot be null.");
        //    int affectRow = await _invoiceDAO.UpdateAsync(entity);
        //    if (affectRow <= 0)
        //        throw new InvalidOperationException("Failed to update the invoice.");
        //    return affectRow;
        //}

        //public async Task<int> UpdateManyAsync(IEnumerable<InvoiceModel> entities)
        //{
        //    if (await IsValidList(entities, false))
        //    {
        //        int affectRow = await _invoiceDAO.UpdateManyAsync(entities);
        //        if (affectRow <= 0)
        //            throw new InvalidOperationException("Failed to update one or more invoices.");
        //        return affectRow;
        //    }
        //    throw new ArgumentException("One or more entities are invalid or do not exist.");
        //}
    }
}
