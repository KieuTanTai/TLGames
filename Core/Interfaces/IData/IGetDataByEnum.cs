using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TLGames.Core.Interfaces.IData
{
    public interface IGetDataByEnum<T> where T : class
    {
#nullable enable
        Task<List<T>?> GetAllByEnum<TEnum>(TEnum value, string colName) where TEnum : Enum;
    }
}
