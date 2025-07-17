using System.Threading.Tasks;

namespace TLGames.Core.Interfaces.IData
{
    public interface IGetSingleByIdsAsync<T> where T : class
    {
        public string GetSingleDataString();
        Task<T> GetSingleByIdAsync(object keys);
    }
}
