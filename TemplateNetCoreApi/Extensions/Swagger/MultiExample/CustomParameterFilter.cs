using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace TemplateNetCoreApi.Extension.MultiExample
{
    public class CustomParameterFilter : IParameterFilter
    {
        public void Apply(OpenApiParameter parameter, ParameterFilterContext context)
        {
            IEnumerable<SwaggerParameterExampleAttribute>? parameterAttributes = null;
            if (context.PropertyInfo != null)
            {
                parameterAttributes = context.PropertyInfo.GetCustomAttributes<SwaggerParameterExampleAttribute>();
            }
            else if (context.ParameterInfo != null)
            {
                parameterAttributes = context.ParameterInfo.GetCustomAttributes<SwaggerParameterExampleAttribute>();
            }
            if (parameterAttributes != null && parameterAttributes.Any())
            {
                AddExample(parameter, parameterAttributes);
            }
        }

        private void AddExample(OpenApiParameter parameter, IEnumerable<SwaggerParameterExampleAttribute> parameterAttributes)
        {
            foreach (var item in parameterAttributes)
            {
                var example = new OpenApiExample
                {
                    Value = new Microsoft.OpenApi.Any.OpenApiString(item.Value),
                };
                parameter.Examples.Add(item.Name, example);
            }
        }
    }
}
