using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLGames.Core.Interfaces.IServices.ICategory
{
    public interface ICategoryCoordinateService
    {
        Task<bool> DeleteCategoryAndProductCategory(string id);
        Task<bool> IsHaveConstraintProductCategory(string id);

    }
}
