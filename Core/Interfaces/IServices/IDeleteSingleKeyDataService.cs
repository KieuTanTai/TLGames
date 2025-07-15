using System.Collections.Generic;
using System.Threading.Tasks;

namespace TLGames.Core.Interfaces.IServices
{
    internal interface IDeleteSingleKeyDataService<T> where T : class
    {
        Task<bool> DeleteByIdAsync(string id);
        Task<bool> DeleteByIdsAsync(IEnumerable<string> ids);
    }
}
