using System.Threading.Tasks;

namespace TLGames.Core.Interfaces.IServices
{
    internal interface IUpdateDoubleKeyDataService<T> where T : class
    {
        Task<bool> UpdateAsync(T entity, string oldKey1);
    }
}
