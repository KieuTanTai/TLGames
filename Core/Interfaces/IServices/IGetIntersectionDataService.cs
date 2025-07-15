using System.Collections.Generic;
using System.Threading.Tasks;

namespace TLGames.Core.Interfaces.IServices
{
    internal interface IGetIntersectionDataService<T> where T : class
    {
#nullable enable
        Task<List<T>?> GetAllByIdAsync(string id, string colIdName);
        Task<T?> GetSingleByIdAsync(object keys);
        //Task<T?> GetSingleByIdAsync(string firstId, string secondId, string colFirstIdName, string colSecondIdName);
    }
}
