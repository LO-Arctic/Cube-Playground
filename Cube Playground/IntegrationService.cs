using Cube_Playground.Enums;
using Cube_Playground.RAP;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net;
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
                string displayName = Regex.Replace(property.Name, @"((?<=\p{Ll})\p{Lu})|((?!\A)\p{Lu}(?>\p{Ll}))", " $0").Trim();

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

        public static string GenerateFormData(object source, string formContent)
        {
            List<FormElement> form = JsonConvert.DeserializeObject<List<FormElement>>(formContent) ?? new();
            List<FormElementData> formData = new List<FormElementData>();

            foreach (FormElement element in form)
            {
                PropertyInfo? property = source.GetType().GetProperty(Regex.Replace(element.Display, " ", string.Empty));
                object formValue = null;
                if (property != null)
                {
                    object? sourceValue = property.GetValue(source);
                    if (sourceValue != null)
                    {
                        switch (element.Type)
                        {
                            case FormElementType.TextField:
                                formValue = sourceValue.ToString();
                                break;
                            case FormElementType.DateField:
                                if (sourceValue is DateTime dateValue)
                                {
                                    formValue = dateValue.ToString("yyyy-MM-dd");
                                }
                                break;
                            case FormElementType.Boolean:
                                if (sourceValue is bool boolValue)
                                {
                                    formValue = boolValue;
                                }
                                break;
                        }
                    }
                }
                formData.Add(new FormElementData
                {
                    Ref = element.Ref,
                    Type = element.Type,
                    Label = element.Label,
                    Value = formValue,
                    DisplayValue = formValue?.ToString(),
                    Visible = true
                });
            }

            return JsonConvert.SerializeObject(formData, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, ContractResolver = new CamelCasePropertyNamesContractResolver() });
        }

        public static string StripHtml(string input)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(input);
            return WebUtility.HtmlDecode(htmlDoc.DocumentNode.InnerText);
        }
    }
}
