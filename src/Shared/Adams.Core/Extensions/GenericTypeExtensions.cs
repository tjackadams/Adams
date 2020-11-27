using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adams.Core.Extensions
{
    public static class GenericTypeExtensions
    {
        public static string GetGenericTypeName(this Type type)
        {
            string typeName;

            if (type.IsGenericType)
            {
                var genericTypes = string.Join(",", type.GetGenericArguments().Select(t => t.Name).ToArray());
                typeName = $"{type.Name.Remove(type.Name.IndexOf('`'))}<{genericTypes}>";
            }
            else
            {
                if (type.IsNested)
                {
                    typeName = type.FullName?.Substring(type.FullName.LastIndexOf(".", StringComparison.Ordinal) + 1).Replace("+", string.Empty);
                }
                else
                {
                    typeName = type.Name;
                }
            }

            return typeName;
        }

        public static string GetGenericTypeName(this object @object)
        {
            return @object.GetType().GetGenericTypeName();
        }
    }
    
}
