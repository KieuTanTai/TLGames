using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TLGames.Core.Entities;

namespace TLGames.Core.Interfaces.IServices.User.PaymentMethod
{
    public interface IPaymentMethodManagementService<T> : IBaseService<T>, IBaseRelativeService<T>, IBaseEnumTimeService<T>  where T : class
    {

        Task<List<T>> GetAllByIdAsync(string id, string colName);
    }
}
