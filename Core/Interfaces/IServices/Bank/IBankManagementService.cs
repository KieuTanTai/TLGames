using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLGames.Core.Entities;
using TLGames.Core.Enums;

namespace TLGames.Core.Interfaces.IServices.Bank
{
    public interface IBankManagementService<T> : IBaseService<T>, IBaseRelativeService<T> where T : class
    {
    }
}
