using System.Threading.Tasks;

namespace TLGames.Core.Interfaces.IData
{
    public interface IDeleteByIdsAsync
    {
        public string GetDeleteQuery();
        Task<int> DeleteByIdsAsync(object keys);
    }
}
