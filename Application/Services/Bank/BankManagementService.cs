using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TLGames.Core.Entities;
using TLGames.Core.Enums;
using TLGames.Core.Interfaces.IData;
using TLGames.Core.Interfaces.IServices.Bank;

namespace TLGames.Application.Services.Bank
{
    public class BankManagementService : ValidateService<BankModel>, IBankManagementService<BankModel>
    {
        private readonly IDAO<BankModel> _bankDAO;
        private readonly IGetRelativeAsync<BankModel> _getRelativeService;
        private readonly IGetDataByEnumAsync<BankModel> _getDataByEnumService;

        public BankManagementService(IDAO<BankModel> bankDAO, IGetRelativeAsync<BankModel> getRelativeService, IGetDataByEnumAsync<BankModel> getDataByEnumService)
            : base(bankDAO)
        {
            _bankDAO = bankDAO;
            _getRelativeService = getRelativeService;
            _getDataByEnumService = getDataByEnumService;
        }

        public async Task<List<BankModel>> GetAllAsync()
        {
            return await _bankDAO.GetAllAsync();
        }

        public async Task<BankModel> GetByIdAsync(string id)
        {
            CheckNullOrEmpty([id]); // if false it will throw an ArgumentException
            BankModel bank = await _bankDAO.GetByIdAsync(id);
            if (bank == null)
                throw new KeyNotFoundException($"Bank with ID {id} not found.");
            return bank;
        }

        public async Task<List<BankModel>> GetAllByEnumAsync<TEnum>(TEnum status, string colName = "status") where TEnum : Enum
        {
            CheckNullOrEmpty([colName]); // if false it will throw an ArgumentException
            if (status is not EActiveStatus)
                throw new ArgumentException("Invalid enum type for bank status.", nameof(status));
            if (!IsValidStringInputDB(colName))
                throw new ArgumentException("Invalid column name for enum filtering.", nameof(colName));
            return await _getDataByEnumService.IGetAllByEnumAsync(status, colName);
        }

        public async Task<List<BankModel>> GetRelativeAsync(string input, string colName = "bank_name")
        {
            CheckNullOrEmpty([input, colName]); // if false it will throw an ArgumentException
            if (!IsValidStringInputDB(colName))
                throw new ArgumentException("Invalid column name for relative bank retrieval.", nameof(colName));
            return await _getRelativeService.GetRelativeAsync(input, colName);
        }

        public async Task<int> InsertAsync(BankModel entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Bank entity cannot be null.");
            if (await IsExistObject(entity.BankId.ToString()))
                throw new InvalidOperationException($"Bank with ID {entity.BankId} already exists.");
            int affectRow = await _bankDAO.InsertAsync(entity);
            if (affectRow <= 0)
                throw new InvalidOperationException("Failed to insert bank entity.");
            return affectRow;
        }

        public async Task<int> InsertManyAsync(IEnumerable<BankModel> entities)
        {
            if (await IsValidList(entities, true))
            {
                int affectRow = await _bankDAO.InsertManyAsync(entities);
                if (affectRow <= 0)
                    throw new InvalidOperationException("Failed to insert bank entities.");
                return affectRow;
            }
            return -1;
        }

        public async Task<int> UpdateAsync(BankModel entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Bank entity cannot be null.");
            int affectRow = await _bankDAO.UpdateAsync(entity);
            if (affectRow <= 0)
                throw new InvalidOperationException($"Failed to update bank with ID {entity.BankId}.");
            return affectRow;
        }

        public async Task<int> UpdateManyAsync(IEnumerable<BankModel> entities)
        {
            if (await IsValidList(entities, false))
            {
                int affectRow = await _bankDAO.UpdateManyAsync(entities);
                if (affectRow <= 0)
                    throw new InvalidOperationException("Failed to update bank entities.");
                return affectRow;
            }
            return -1;
        }

        //public async Task<int> SoftDeleteAsync<TEnum>(string id, TEnum status) where TEnum : Enum
        //{
        //    CheckNullOrEmpty([id]); // if false it will throw an ArgumentException
        //    if (status is EActiveStatus activeStatus)
        //    {
        //        BankModel bank = await _bankDAO.GetByIdAsync(id);
        //        if (bank == null)
        //            throw new KeyNotFoundException($"Bank with ID {id} not found.");
        //        bank.SetStatus(activeStatus);
        //        int affectRow = await _bankDAO.UpdateAsync(bank);
        //        if (affectRow <= 0)
        //            throw new InvalidOperationException($"Failed to soft delete bank with ID {id}.");
        //        return affectRow;
        //    }
        //    throw new ArgumentException("Invalid enum type for bank status.", nameof(status));
        //}

        //public async Task<int> SoftDeleteManyAsync<TEnum>(IEnumerable<string> ids, TEnum status) where TEnum : Enum
        //{
        //    CheckNullOrEmpty(ids); // if false it will throw an ArgumentException
        //    if (status is EActiveStatus activeStatus)
        //    {
        //        List<BankModel> banksToUpdate = new List<BankModel>();
        //        foreach (string id in ids)
        //        {
        //            BankModel bank = await _bankDAO.GetByIdAsync(id) ?? throw new KeyNotFoundException($"Bank with ID {id} not found.");
        //            bank.SetStatus(activeStatus);
        //            banksToUpdate.Add(bank);
        //        }
        //        int affectRow = await _bankDAO.UpdateManyAsync(banksToUpdate);
        //        if (affectRow <= 0)
        //            throw new InvalidOperationException("Failed to soft delete one or more banks.");
        //        return affectRow;
        //    }
        //    throw new ArgumentException("Invalid enum type for bank status.", nameof(status));
        //}
    }
}
