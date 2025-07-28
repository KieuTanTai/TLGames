using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TLGames.Core.Entities;
using TLGames.Core.Enums;
using TLGames.Core.Interfaces.IData;
using TLGames.Core.Interfaces.IServices.Transaction.Invoice;
using TLGames.Infrastructure.Data;

namespace TLGames.Application.Services.Transaction.Invoice
{
    internal class DetailInvoiceManagementService : ValidateService<DetailInvoiceModel>, IDetailInvoiceManagementService<DetailInvoiceModel>
    {
        private readonly IDAO<DetailInvoiceModel> _detailInvoiceDAO;
        private readonly IDAO<InvoiceModel> _invoiceDAO;
        private readonly IGetSingleByIdsAsync<DetailInvoiceModel> _getSingleByIdsService;
        private readonly IGetAllByIdAsync<DetailInvoiceModel> _getAllByIdService;
        private readonly IGetDataByEnumAsync<DetailInvoiceModel> _getDataByEnumService;

        public DetailInvoiceManagementService(IDAO<DetailInvoiceModel> detailInvoiceDAO, IDAO<InvoiceModel> invoiceDAO, IGetSingleByIdsAsync<DetailInvoiceModel> getSingleByIdsService,
            IGetAllByIdAsync<DetailInvoiceModel> getAllByIdService, IGetDataByEnumAsync<DetailInvoiceModel> getDataByEnumService) : base(detailInvoiceDAO)
        {
            _detailInvoiceDAO = detailInvoiceDAO;
            _invoiceDAO = invoiceDAO;
            _getSingleByIdsService = getSingleByIdsService;
            _getAllByIdService = getAllByIdService;
            _getDataByEnumService = getDataByEnumService;
        }

        public async Task<List<DetailInvoiceModel>> GetAllAsync()
        {
            return await _detailInvoiceDAO.GetAllAsync();
        }

        public async Task<List<DetailInvoiceModel>> GetAllByEnumAsync<TEnum>(TEnum value, string colName = "status") where TEnum : Enum
        {
            CheckNullOrEmpty([colName]); // if false it will throw an ArgumentException
            if (value is not EInvoiceStatus)
                throw new ArgumentException("Invalid enum type for status.", nameof(value));
            if (!IsValidStringInputDB(colName))
                throw new ArgumentException("Invalid column name for enum filtering.", nameof(colName));
            return await _getDataByEnumService.IGetAllByEnumAsync(value, colName);
        }

        public async Task<List<DetailInvoiceModel>> GetAllByIdAsync(string id, string colName = "invoice_id")
        {
            CheckNullOrEmpty([id, colName]); // if false it will throw an ArgumentException
            if (!IsValidStringInputDB(colName))
                throw new ArgumentException("Invalid column name for ID filtering.", nameof(colName));
            return await _getAllByIdService.GetAllByIdAsync(id, colName);
        }

        public async Task<DetailInvoiceModel> GetSingleByIdsAsync(object ids)
        {
            if (ids is DetailInvoiceItemIds itemIds)
            {
                if (string.IsNullOrEmpty(itemIds.InvoiceId) || string.IsNullOrEmpty(itemIds.ProductId))
                    throw new ArgumentException("DetailInvoiceId and ProductId cannot be null or empty.", nameof(ids));
                DetailInvoiceModel result = await _getSingleByIdsService.GetSingleByIdAsync(itemIds);
                if (result == null)
                    throw new InvalidOperationException("DetailInvoiceModel not found for the provided IDs.");
                return result;
            }
            throw new ArgumentException("Invalid type for ids. Expected DetailInvoiceItemIds.", nameof(ids));
        }

        public async Task<int> InsertAsync(DetailInvoiceModel productCategory)
        {
            if (await IsExistObject(productCategory))
                throw new InvalidOperationException("DetailInvoiceModel already exists with the same InvoiceId and ProductId.");
            int affectRow = await _detailInvoiceDAO.InsertAsync(productCategory);
            if (affectRow <= 0)
                throw new InvalidOperationException("Failed to insert DetailInvoiceModel.");
            return affectRow;
        }

        public async Task<int> InsertManyAsync(IEnumerable<DetailInvoiceModel> productCategories)
        {
            if (productCategories == null || !productCategories.Any())
                throw new ArgumentException("Product categories cannot be null or empty.", nameof(productCategories));
            if (await IsValidEnumerable(productCategories, true))
                throw new InvalidOperationException("DetailInvoiceModels already exist with the same InvoiceId and ProductId.");
            int affectRow = await _detailInvoiceDAO.InsertManyAsync(productCategories);
            if (affectRow <= 0)
                throw new InvalidOperationException("Failed to insert DetailInvoiceModels.");
            return affectRow;
        }

        public async Task<int> UpdateAsync(DetailInvoiceModel entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "DetailInvoiceModel cannot be null.");
            int affectRow = await _detailInvoiceDAO.UpdateAsync(entity);
            if (affectRow <= 0)
                throw new InvalidOperationException("Failed to update DetailInvoiceModel.");
            return affectRow;
        }

        public async Task<int> UpdateManyAsync(IEnumerable<DetailInvoiceModel> entities)
        {
            if (await IsValidEnumerable(entities, false))
            {
                int affectRow = await _detailInvoiceDAO.UpdateManyAsync(entities);
                if (affectRow <= 0)
                    throw new InvalidOperationException("Failed to update DetailInvoiceModels.");
                return affectRow;
            }
            throw new InvalidOperationException("DetailInvoiceModels already exist with the same InvoiceId and ProductId."); 
        }

        private async Task<bool> IsValidEnumerable(IEnumerable<DetailInvoiceModel> entities, bool checkExist)
        {
            if (entities == null || !entities.Any())
                throw new ArgumentNullException(nameof(entities), "Detail invoice entities cannot be null or empty.");
            foreach (DetailInvoiceModel entity in entities)
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity), "Detail invoice entity cannot be null.");

                if (checkExist)
                {
                    DetailInvoiceItemIds itemIds = new DetailInvoiceItemIds(entity.InvoiceId.ToString(), entity.ProductId.ToString());
                    DetailInvoiceModel existingObject = await _getSingleByIdsService.GetSingleByIdAsync(itemIds);

                    if (existingObject != null)
                        throw new InvalidOperationException($"Detail invoice with Product ID {entity.ProductId} and Invoice ID {entity.InvoiceId} already exists.");
                }
            }
            return true; // Tất cả các kiểm tra đều thành công
        }
    }
}
