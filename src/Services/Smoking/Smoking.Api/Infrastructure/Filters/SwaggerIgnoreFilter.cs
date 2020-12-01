using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Adams.Services.Smoking.Api.Infrastructure.Filters
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SwaggerIgnoreAttribute : Attribute { }

    public class SwaggerIgnoreFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            var type = context.Type;

            if (schema?.Properties == null || type == null)
            {
                return;
            }

            var ignoreProperties =
                type.GetProperties().Where(t => t.GetCustomAttribute<SwaggerIgnoreAttribute>() != null);

            foreach (var ignoreProperty in ignoreProperties)
            {
                if (schema.Properties.ContainsKey(ignoreProperty.Name))
                {
                    schema.Properties.Remove(ignoreProperty.Name);
                }
            }
        }
    }
}
