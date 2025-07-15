using System.Collections.Generic;
using System.Threading.Tasks;

namespace TLGames.Core.Interfaces.IServices
{
    internal interface ISoftDeleteService
    {
        Task<bool> SoftDeleteByIdAsync(string id);
        Task<bool> SoftDeleteByIdsAsync(IEnumerable<string> ids);
    }
}
