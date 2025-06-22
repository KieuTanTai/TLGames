using System.Threading.Tasks;

namespace TLGames.Core.Interfaces
{
    internal interface IDeleteByIdsAsync
    {
        public string GetDeleteQuery();
        Task<bool> DeleteByIdsAsync(object keys);
    }
}
