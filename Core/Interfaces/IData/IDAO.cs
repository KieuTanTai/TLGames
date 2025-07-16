using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLGames.Core.Interfaces.IData
{
    internal interface IDAO<TModel> : ICrudOperationsAsync<TModel>, IQueryOperationsAsync, IExecuteOperationsAsync
        where TModel : class
    {
    }
}
