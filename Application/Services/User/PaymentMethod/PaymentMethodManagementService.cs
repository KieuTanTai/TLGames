using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TLGames.Core.Entities;
using TLGames.Core.Enums;
using TLGames.Core.Interfaces.IData;
using TLGames.Core.Interfaces.IServices.User.PaymentMethod;
using TLGames.Infrastructure.Data;

namespace TLGames.Application.Services.User.PaymentMethod
{
    public class PaymentMethodManagementService : ValidateService<UserPaymentMethodModel>, IPaymentMethodManagementService<UserPaymentMethodModel>
    {
        private readonly IDAO<UserPaymentMethodModel> _userPaymentMethodDAO;
        private readonly IGetAllByIdAsync<UserPaymentMethodModel> _getAllByIdService;
        private readonly IGetRelativeAsync<UserPaymentMethodModel> _getRelativeService;
        private readonly IGetDataByEnumAsync<UserPaymentMethodModel> _getDataByEnumService;
        private readonly IGetDataByDateTimeAsync<UserPaymentMethodModel> _getDataByDateTimeService;

        // Constructor để inject các dependency
        public PaymentMethodManagementService(IDAO<UserPaymentMethodModel> userPaymentMethodDAO, IGetAllByIdAsync<UserPaymentMethodModel> getAllByIdService,
            IGetRelativeAsync<UserPaymentMethodModel> getRelativeService, IGetDataByEnumAsync<UserPaymentMethodModel> getDataByEnumService, IGetDataByDateTimeAsync<UserPaymentMethodModel> getDataByDateTimeService)
            : base(userPaymentMethodDAO)
        {
            _userPaymentMethodDAO = userPaymentMethodDAO;
            _getAllByIdService = getAllByIdService;
            _getRelativeService = getRelativeService;
            _getDataByEnumService = getDataByEnumService;
            _getDataByDateTimeService = getDataByDateTimeService;
        }

        public async Task<List<UserPaymentMethodModel>> GetAllAsync()
        {
            return await _userPaymentMethodDAO.GetAllAsync();
        }

        public async Task<List<UserPaymentMethodModel>> GetAllByEnumAsync<TEnum>(TEnum value, string colName = "status") where TEnum : Enum
        {
            CheckNullOrEmpty([colName]); // if false it will throw an ArgumentException
            if (value is not EDataTimeType)
                throw new ArgumentException("Invalid enum type for this method.");
            if (!IsValidStringInputDB(colName))
                throw new ArgumentException("Invalid column name provided.");
            return await _getDataByEnumService.IGetAllByEnumAsync(value, colName);
        }

        public async Task<List<UserPaymentMethodModel>> GetAllByIdAsync(string id, string colName = "user_id")
        {
            CheckNullOrEmpty([id, colName]); // if false it will throw an ArgumentException
            if (!IsValidStringInputDB(colName))
                throw new ArgumentException("Invalid column name provided.");
            return await _getAllByIdService.GetAllByIdAsync(id, colName);
        }

        public async Task<List<UserPaymentMethodModel>> GetAllByTimeAsync<TEnum>(string time, TEnum timeType, string colName = "added_date") where TEnum : Enum
        {
            CheckNullOrEmpty([time, colName]); // if false it will throw an ArgumentException
            if (!IsValidStringInputDB(colName))
                throw new ArgumentException("Invalid column name provided.");
            // Validate timeType and time format
            if (!IsValidTimeFormat(time, timeType))
                throw new ArgumentException("Invalid time type or format provided.");
            return await _getDataByDateTimeService.GetAllByTimeAsync(time, colName, timeType);
        }

        public async Task<List<UserPaymentMethodModel>> GetAllByTimeRangeAsync<TEnum>(string firstInputTime, string secondInputTime, TEnum timeType, string colName = "added_date") where TEnum : Enum
        {
            CheckNullOrEmpty([firstInputTime, secondInputTime, colName]); // if false it will throw an ArgumentException
            if (!IsValidStringInputDB(colName))
                throw new ArgumentException("Invalid column name provided.");
            // Validate timeType and time format
            if (!IsValidTimeFormat(firstInputTime, timeType) && !IsValidTimeFormat(secondInputTime, timeType))
                throw new ArgumentException("Invalid time type or format provided.");
            return await _getDataByDateTimeService.GetAllByTimeRangeAsync(firstInputTime, secondInputTime, colName, timeType);
        }

        public async Task<UserPaymentMethodModel> GetByIdAsync(string id)
        {
            CheckNullOrEmpty([id]); // if false it will throw an ArgumentException
            UserPaymentMethodModel existingObject = await _userPaymentMethodDAO.GetByIdAsync(id);
            if (existingObject == null)
                throw new KeyNotFoundException($"No payment method found with ID: {id}");
            return existingObject;
        }

        public async Task<List<UserPaymentMethodModel>> GetRelativeAsync(string input, string colName = "display_name")
        {
            CheckNullOrEmpty([input, colName]); // if false it will throw an ArgumentException
            if (!IsValidStringInputDB(colName))
                throw new ArgumentException("Invalid column name provided.");
            return await _getRelativeService.GetRelativeAsync(input, colName);
        }

        public async Task<int> InsertAsync(UserPaymentMethodModel entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null.");
            if (await IsExistObject(entity.UserPaymentMethodId.ToString()))
                throw new InvalidOperationException($"Payment method with ID: {entity.UserPaymentMethodId} already exists.");
            int affectRow = await _userPaymentMethodDAO.InsertAsync(entity);
            if (affectRow <= 0)
                throw new InvalidOperationException("Failed to insert the payment method.");
            return affectRow;

        }

        public async Task<int> InsertManyAsync(IEnumerable<UserPaymentMethodModel> entities)
        {
            if (await IsValidList(entities, true))
            {
                int affectRow = _userPaymentMethodDAO.InsertManyAsync(entities).Result;
                if (affectRow <= 0)
                    throw new InvalidOperationException("Failed to insert payment methods.");
                return affectRow;
            }
            throw new ArgumentException("Invalid payment method list provided.");
        }

        //public async Task<int> SoftDeleteAsync<TEnum>(string id, TEnum status) where TEnum : Enum
        //{
        //    CheckNullOrEmpty([id]); // if false it will throw an ArgumentException
        //    if (status is EActiveStatus activeStatus)
        //    {
        //        UserPaymentMethodModel userPaymentMethod = await _userPaymentMethodDAO.GetByIdAsync(id);
        //        if (userPaymentMethod == null)
        //            throw new KeyNotFoundException($"user payment method with ID {id} not found.");
        //        userPaymentMethod.SetStatus(activeStatus);
        //        int affectRow = await _userPaymentMethodDAO.UpdateAsync(userPaymentMethod);
        //        if (affectRow <= 0)
        //            throw new InvalidOperationException($"Failed to soft delete user payment method with ID {id}.");
        //        return affectRow;
        //    }
        //    throw new ArgumentException("Invalid enum type for user payment method status.", nameof(status));
        //}

        //public async Task<int> SoftDeleteManyAsync<TEnum>(IEnumerable<string> ids, TEnum status) where TEnum : Enum
        //{
        //    CheckNullOrEmpty(ids); // if false it will throw an ArgumentException
        //    if (status is EActiveStatus activeStatus)
        //    {
        //        List<UserPaymentMethodModel> paymentMethodsForUpdate = new List<UserPaymentMethodModel>();
        //        foreach (string id in ids)
        //        {
        //            UserPaymentMethodModel userPaymentMethod = await _userPaymentMethodDAO.GetByIdAsync(id) ?? throw new KeyNotFoundException($"userPaymentMethod with ID {id} not found.");
        //            userPaymentMethod.SetStatus(activeStatus);
        //            paymentMethodsForUpdate.Add(userPaymentMethod);
        //        }
        //        int affectRow = await _userPaymentMethodDAO.UpdateManyAsync(paymentMethodsForUpdate);
        //        if (affectRow <= 0)
        //            throw new InvalidOperationException("Failed to soft delete one or more user payment method.");
        //        return affectRow;
        //    }
        //    throw new ArgumentException("Invalid enum type for user payment method status.", nameof(status));
        //}

        public async Task<int> UpdateAsync(UserPaymentMethodModel entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null.");
            if (!await IsExistObject(entity.UserPaymentMethodId.ToString()))
                throw new KeyNotFoundException($"No payment method found with ID: {entity.UserPaymentMethodId}");
            int affectRow = await _userPaymentMethodDAO.UpdateAsync(entity);
            if (affectRow <= 0)
                throw new InvalidOperationException("Failed to update the payment method.");
            return affectRow;
        }

        public async Task<int> UpdateManyAsync(IEnumerable<UserPaymentMethodModel> entities)
        {
            if (await IsValidList(entities, false))
            {
                int affectRow = _userPaymentMethodDAO.UpdateManyAsync(entities).Result;
                if (affectRow <= 0)
                    throw new InvalidOperationException("Failed to update payment methods.");
                return affectRow;
            }
            throw new ArgumentException("Invalid payment method list provided.");
        }
    }
}
