using System.Threading.Tasks;

namespace TLGames.Core.Interfaces
{
    internal interface IUpdateByOldKeyAsync<T> where T : class
    {
        string GetUpdateWithOldKeyString();
        Task<bool> UpdateAsync(T entity, string oldKey);
    }
}
