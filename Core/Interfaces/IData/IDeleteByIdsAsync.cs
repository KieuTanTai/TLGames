using System.Threading.Tasks;

namespace TLGames.Core.Interfaces.IData
{
    internal interface IDeleteByIdsAsync
    {
        public string GetDeleteQuery();
        Task<bool> DeleteByIdsAsync(object keys);
    }
}
