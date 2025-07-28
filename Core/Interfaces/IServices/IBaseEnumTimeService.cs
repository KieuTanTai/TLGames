using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLGames.Core.Interfaces.IServices
{
    public interface IBaseEnumTimeService<T> where T : class
    {
        Task<List<T>> GetAllByTimeRangeAsync<TEnum>(string firstInputTime, string secondInputTime, TEnum timeType, string colName) where TEnum : Enum;
        Task<List<T>> GetAllByTimeAsync<TEnum>(string time, TEnum timeType, string colName) where TEnum : Enum;
    }
}
