using Dapper.FluentMap;
using static Dapper.SimpleCRUD;

namespace Pobytne.Data
{
    public class MyFluentMapperNameResolver : IColumnNameResolver
    {

        public string ResolveColumnName(System.Reflection.PropertyInfo propertyInfo)
        {
            if (!FluentMapper.EntityMaps.TryGetValue(propertyInfo.ReflectedType!, out var map))
            {
                throw new NotImplementedException("Property ReflectedType not resolved!");
            }
            var property = map.PropertyMaps.FirstOrDefault(x => x.PropertyInfo == propertyInfo);
            if (property == null)
            {
                return propertyInfo.Name;
            }
            return property.ColumnName;
        }
    }
}
