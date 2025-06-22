using System.Collections.Generic;
using System.Threading.Tasks;

namespace TLGames.Core.Interfaces
{
    internal interface IGetRelativeAsync<T> where T : class
    {
        public string GetQueryDataString(string colName);
#nullable enable
        Task<List<T>?> GetRelativeAsync(string input, string colName);
#nullable disable
    }
}
