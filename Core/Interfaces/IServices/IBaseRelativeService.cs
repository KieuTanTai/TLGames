using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TLGames.Core.Interfaces.IServices
{
    public interface IBaseRelativeService<T> where T : class
    {
        Task<List<T>> GetRelativeAsync(string input, string colName);
        Task<List<T>> GetAllByEnumAsync<TEnum>(TEnum value, string colName) where TEnum : Enum;
    }
}
