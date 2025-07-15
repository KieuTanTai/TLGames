using System.Threading.Tasks;

namespace TLGames.Core.Interfaces.IServices
{
    internal interface IDeleteDoubleKeyDataService
    {
        Task<bool> DeleteByKeysAsync(object keys);
    }
}
