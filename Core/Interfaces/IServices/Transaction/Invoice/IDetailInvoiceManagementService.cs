using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TLGames.Core.Interfaces.IServices.Transaction.Invoice
{
    internal interface IDetailInvoiceManagementService<T> : IBaseLinkingDataService<T> where T : class
    {
        Task<List<T>> GetAllByEnumAsync<TEnum>(TEnum value, string colName) where TEnum : Enum;
        Task<int> UpdateAsync(T entity);
        Task<int> UpdateManyAsync(IEnumerable<T> entities);
    }
}
