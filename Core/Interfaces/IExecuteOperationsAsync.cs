using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLGames.Core.Interfaces
{
    internal interface IExecuteOperationsAsync
    {
#nullable enable
        Task<bool> ExecuteAsync(string query, object? parameters = null, IDbTransaction? transaction = null);
#nullable disable
    }
}
