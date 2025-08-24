using Cube_Playground.Enums;
using Cube_Playground.RAP;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Cube_Playground
{
    internal static class IntegrationService
    {
        public static List<FormElement> GenerateFormContent(Type modelType)
        {
            Dictionary<Type, FormElementType> elementTypeMap = new()
            {
                { typeof(string), FormElementType.TextField },
                { typeof(DateTime), FormElementType.DateField },
                { typeof(int), FormElementType.TextField },
                { typeof(int?), FormElementType.TextField },
                { typeof(double), FormElementType.TextField },
                { typeof(double?), FormElementType.TextField },
                { typeof(decimal), FormElementType.TextField },
                { typeof(decimal?), FormElementType.TextField },
                { typeof(bool), FormElementType.Boolean },
                { typeof(bool?), FormElementType.Boolean },
                { typeof(Guid), FormElementType.TextField },
                { typeof(Guid?), FormElementType.TextField }

            };

            List<FormElement> formElements = new List<FormElement>();
            foreach (PropertyInfo property in modelType.GetProperties().Where(x => elementTypeMap.ContainsKey(x.PropertyType)))
            {
                string displayName = Regex.Replace(property.Name, "[A-Z]", " $0").Trim();

                formElements.Add(new FormElement
                {
                    Type = elementTypeMap[property.PropertyType],
                    Display = displayName,
                    Label = displayName,
                    Ref = Guid.NewGuid(),
                    Size = FormElementSize.Small
                });
            }

            return formElements;
        }
    }
}
