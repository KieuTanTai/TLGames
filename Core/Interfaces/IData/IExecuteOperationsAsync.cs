using System.Data;
using System.Threading.Tasks;

namespace TLGames.Core.Interfaces.IData
{
    internal interface IExecuteOperationsAsync
    {
#nullable enable
        Task<bool> ExecuteAsync(string query, object? parameters = null, IDbTransaction? transaction = null);
#nullable disable
    }
}
