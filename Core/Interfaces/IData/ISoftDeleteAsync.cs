using System.Threading.Tasks;

namespace TLGames.Core.Interfaces.IData
{
    public interface ISoftDeleteAsync<T> where T : class
    {
        //public string GetSoftDeleteQuery();
        Task<bool> SoftDeleteAsync(T entity);
    }
}
