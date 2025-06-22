using System.Threading.Tasks;

namespace TLGames.Core.Interfaces
{
    internal interface IGetSingleByIdsAsync<T> where T : class
    {
        public string GetSingleDataString();
        Task<T> GetSingleByIdAsync(object keys);
    }
}
