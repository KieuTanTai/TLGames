using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TLGames.Core.Interfaces.IData
{
    public interface IGetDataByEnumAsync<T> where T : class
    {
#nullable enable
        Task<List<T>?> IGetAllByEnumAsync<TEnum>(TEnum value, string colName) where TEnum : Enum;
    }
}
