using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TLGames.Core.Enums;
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

        protected bool IsValidTimeFormat<TEnum>(string time, TEnum timeType) where TEnum : Enum
        {
            if (timeType is EDataTimeType eTimeType)
                if (eTimeType == EDataTimeType.MONTH || eTimeType == EDataTimeType.YEAR)
                    return int.TryParse(time, out _);
                else if (eTimeType == EDataTimeType.DATETIME)
                    return DateTime.TryParse(time, out _);
            return false;
        }

#nullable enable
        protected async Task<bool> IsExistObject(string input)
        {
            T? existingObject = await objectDAO.GetByIdAsync(input);
            return existingObject != null;
        }

        protected async Task<bool> IsExistObject(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null.");
            return await objectDAO.IsExistObjectAsync(entity);
        }

        protected void CheckNullOrEmpty(IEnumerable<string> input)
        {
            if (input == null || !input.Any())
                throw new ArgumentException(nameof(input), "Input list cannot be null or empty.");
            foreach (string item in input)
                if (string.IsNullOrEmpty(item))
                    throw new ArgumentException(nameof(input), $"{nameof(item)} is null or empty.");
        }

        protected async Task<bool> IsValidList(IEnumerable<T> entities, bool checkExist)
        {
            if (entities == null || !entities.Any())
                throw new ArgumentNullException(nameof(entities), "Entities cannot be null or empty.");
            foreach (T entity in entities)
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity), "Entity cannot be null.");
                if (checkExist)
                    if (await IsExistObject(entity))
                        throw new InvalidOperationException($"Entity already exists.");
            }
            return true;
        }
    }
}
