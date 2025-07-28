using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TLGames.Core.Entities;
using TLGames.Core.Enums;

namespace TLGames.Core.Interfaces.IServices.ICategory
{
    public interface ICategoryManagementService<T> : IBaseService<T>, IBaseRelativeService<T> where T : class
    {

    }
}
