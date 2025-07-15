using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TLGames.Core.Interfaces.IData
{
    internal interface IGetDataByDateTime<T> where T : class
    {
        string GetByMonth(string colName);
        string GetByYear(string colName);
        string GetByDateTime(string colName);
        string GetByDateTimeRange(string colName);
        string GetByMonthAndYear(string colName);
#nullable enable
        Task<List<T>?> GetAllByTime<TEnum>(string time, string colName, TEnum timeType) where TEnum : Enum;
        Task<List<T>?> GetAllByTimeRange<TEnum>(string timeStart, string timeEnd, string colName, TEnum timeType) where TEnum : Enum;
    }
}
