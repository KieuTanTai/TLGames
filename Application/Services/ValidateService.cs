using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using TLGames.Core.Interfaces.IData;
using TLGames.Core.Interfaces.IValidate;

namespace TLGames.Application.Services
{
    public abstract class ValidateService<T>(IDAO<T> objectDAO) where T : class //: IGetDataService<T>, IInsertDataService<T>, IUpdateSingleKeyDataService<T> where T : class
    {
        protected readonly IStringChecker _checker = GetProviderService.SystemServices.GetService<IStringChecker>();
        protected bool IsValidStringInputDB(string input)
        {
            if (_checker.ContainsProblematicDbChars(input) || !_checker.IsSafeDbString(input))
                return false;
            return true;
        }

#nullable enable
        protected async Task<bool> IsExistObject(string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;
            T? existingObject = await objectDAO.GetByIdAsync(input);
            return existingObject != null;
        }

        //public abstract Task<T> GetByIdAsync(string id);

        //public abstract Task<List<T>> GetAllAsync();
        //public abstract Task<List<T>> GetAllByNameAsync(string name);

        //public abstract Task<int> InsertAsync(T entity);

        //public abstract Task<int> InsertManyAsync(IEnumerable<T> entities);

        //public abstract Task<bool> UpdateAsync(T entity);
    }
}
