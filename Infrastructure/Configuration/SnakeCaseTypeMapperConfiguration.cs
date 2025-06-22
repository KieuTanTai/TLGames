using Dapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using TLGames.Core.Interfaces;

namespace TLGames.Infrastructure.Configuration;
public class SnakeCaseTypeMapperConfiguration<T> : FallbackTypeMapper
{
    public SnakeCaseTypeMapperConfiguration() : base(
    [
        new CustomPropertyTypeMap(typeof(T), SelectProperty),
        new DefaultTypeMap(typeof(T))
    ])
    { }

    private static PropertyInfo SelectProperty(Type type, string columnName)
    {
        // Convert snake_case column to PascalCase property
        IStringConverter converter = App.SystemServices.GetService<IStringConverter>();
        string pascal = converter.SnakeCaseToPascalCase(columnName);
        return type.GetProperties().FirstOrDefault(p =>
            string.Equals(p.Name, pascal, StringComparison.OrdinalIgnoreCase));
    }
}

public class FallbackTypeMapper(SqlMapper.ITypeMap[] sqlMappers) : SqlMapper.ITypeMap
{
    public ConstructorInfo FindConstructor(string[] names, Type[] types) => sqlMappers.Select(mapper => mapper.FindConstructor(names, types)).FirstOrDefault(mapper => mapper != null);

    public ConstructorInfo FindExplicitConstructor() => sqlMappers.Select(mapper => mapper.FindExplicitConstructor()).FirstOrDefault(mapper => mapper != null);

    public SqlMapper.IMemberMap GetConstructorParameter(ConstructorInfo constructor, string columnName) => sqlMappers.Select(mapper => mapper.GetConstructorParameter(constructor, columnName)).FirstOrDefault(mapper => mapper != null);

    public SqlMapper.IMemberMap GetMember(string columnName) => sqlMappers.Select(mapper => mapper.GetMember(columnName)).FirstOrDefault(mapper => mapper != null);
}
