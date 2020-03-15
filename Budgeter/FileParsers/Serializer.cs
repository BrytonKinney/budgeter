using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Budgeter.FileParsers
{
    public static class Serializer
    {
        private static ConcurrentDictionary<Type, IEnumerable<PropertyInfo>> _propertyTypeCache = new ConcurrentDictionary<Type, IEnumerable<PropertyInfo>>();

        public static IEnumerable<PropertyInfo> GetProperties(Type type)
        {
            IEnumerable<PropertyInfo> properties;
            if (!_propertyTypeCache.TryGetValue(type, out properties))
                properties = AddTypeProperties(type);
            return properties;
        }
        private static IEnumerable<PropertyInfo> AddTypeProperties(Type type)
        {
            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            _propertyTypeCache.TryAdd(type, properties);
            return properties;
        }
    }
}
