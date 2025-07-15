using System.Threading.Tasks;

namespace TLGames.Core.Interfaces.IServices
{
    internal interface IUpdateSingleKeyDataService<T> where T : class
    {
        Task<bool> UpdateAsync(T entity);
    }
}
