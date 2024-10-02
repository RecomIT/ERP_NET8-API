using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Services
{
    public static class DynamicToStatic
    {
        public static T ToStatic<T>(object expando)
        {
            var entity = Activator.CreateInstance<T>();
            var properties = expando as IDictionary<string, object>;

            if (properties == null)
                return entity;

            foreach (var entry in properties) {
                var propertyInfo = entity.GetType().GetProperty(entry.Key);
                if (propertyInfo != null)
                    propertyInfo.SetValue(entity, entry.Value, null);
            }
            return entity;
        }

        public static T ToStaticList<T>(object expando)
        {
            var entity = Activator.CreateInstance<T>();
            var properties = expando as IDictionary<string, object>;

            if (properties == null)
                return entity;

            foreach (var entry in properties) {
                var propertyInfo = entity.GetType().GetProperty(entry.Key);
                if (propertyInfo != null)
                    propertyInfo.SetValue(entity, entry.Value, null);
            }
            return entity;
        }
    }
}
