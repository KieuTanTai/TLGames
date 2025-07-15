using System.Collections.Generic;
using System.Threading.Tasks;
using TLGames.Core.Enums;

namespace TLGames.Core.Interfaces.IServices
{
    internal interface IDeleteIntegrityService<TParent> where TParent : class
    {
        Task<bool> CanDeleteParentAsync(string parent);
        Task<bool> DeleteParentWithChildrenAsync(string parentId, EDeleteStrategy deleteStrategy = EDeleteStrategy.Restrict);
        Task<bool> DeleteParentsWithChildrenAsync(IEnumerable<string> parentIds, EDeleteStrategy EDeleteStrategy = EDeleteStrategy.Restrict);
    }
}
