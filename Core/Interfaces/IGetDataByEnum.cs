using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLGames.Core.Interfaces
{
    internal interface IGetDataByEnum<T> where T : class
    {
#nullable enable
        Task<List<T>?> GetAllByEnum<TEnum>(TEnum value, string colName) where TEnum : Enum;
    }
}
