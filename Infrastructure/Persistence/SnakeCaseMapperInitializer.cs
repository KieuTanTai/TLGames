using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TLGames.Infrastructure.Configuration;

namespace TLGames.Infrastructure.Persistence;
public static class SnakeCaseMapperInitializer
{
    public static void RegisterAllEntities()
    {
        IEnumerable<Type> entityTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type => type.IsClass && type.Namespace == "TLGames.Core.Entities");

        foreach (Type type in entityTypes)
        {
            Type mapperType = typeof(SnakeCaseTypeMapperConfiguration<>).MakeGenericType(type);
            SqlMapper.ITypeMap mapperInstance = (SqlMapper.ITypeMap)Activator.CreateInstance(mapperType);
            SqlMapper.SetTypeMap(type, mapperInstance);
        }
    }
}
