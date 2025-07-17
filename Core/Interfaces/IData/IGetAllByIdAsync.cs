using System.Collections.Generic;
using System.Threading.Tasks;

namespace TLGames.Core.Interfaces.IData
{
    public interface IGetAllByIdAsync<T> where T : class
    {
#nullable enable
        Task<List<T>?> GetAllByIdAsync(string id, string colIdName);
    }
}
