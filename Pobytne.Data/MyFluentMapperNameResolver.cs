using Dapper.FluentMap;
using static Dapper.SimpleCRUD;

namespace Pobytne.Data
{
    public class MyFluentMapperNameResolver : IColumnNameResolver
    {

        public string ResolveColumnName(System.Reflection.PropertyInfo propertyInfo)
        {
            if (!FluentMapper.EntityMaps.TryGetValue(propertyInfo.DeclaringType, out var map))
            {
                throw new NotImplementedException("Property DeclaringType not resolved!");
            }
            var property = map.PropertyMaps.FirstOrDefault(x => x.PropertyInfo == propertyInfo);
            if (property == null)
            {
                throw new NotImplementedException("Property not resolved!");
            }
            return property.ColumnName;
        }
    }
}
