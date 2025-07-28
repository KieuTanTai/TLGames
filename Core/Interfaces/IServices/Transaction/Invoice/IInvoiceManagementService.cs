using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TLGames.Core.Interfaces.IServices.Transaction.Invoice
{
    public interface IInvoiceManagementService<T> : IBaseEnumTimeService<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(string id);
        Task<int> InsertAsync(T entity);
        Task<int> InsertManyAsync(IEnumerable<T> entities);
        Task<List<T>> GetAllByEnumAsync<TEnum>(TEnum value, string colName) where TEnum : Enum;
        Task<List<T>> GetAllByIdAsync(string id, string colName);
    }
}
