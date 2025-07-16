using Microsoft.Extensions.DependencyInjection;
using TLGames.Core.Interfaces.IValidate;

namespace TLGames.Application.Services
{
    internal abstract class ValidateService //: IGetDataService<T>, IInsertDataService<T>, IUpdateSingleKeyDataService<T> where T : class
    {
        protected readonly IStringChecker _checker = GetProviderService.SystemServices.GetService<IStringChecker>();
        protected bool IsValidStringInputDB(string input)
        {
            if (_checker.ContainsProblematicDbChars(input) || !_checker.IsSafeDbString(input))
                return false;
            return true;
        }

        //public abstract Task<T> GetByIdAsync(string id);

        //public abstract Task<List<T>> GetAllAsync();
        //public abstract Task<List<T>> GetAllByNameAsync(string name);

        //public abstract Task<int> InsertAsync(T entity);

        //public abstract Task<int> InsertManyAsync(IEnumerable<T> entities);

        //public abstract Task<bool> UpdateAsync(T entity);
    }
}
