using Cube_Playground.Enums;
using Newtonsoft.Json.Linq;

namespace Cube_Playground.RAP
{
    internal class FormElement
    {
        public FormElementType Type { get; set; }
        public string Display { get; set; }
        public Guid Ref { get; set; } = Guid.NewGuid();
        public FormElementSize Size { get; set; }
        public string Label { get; set; }
        public bool SubHeading { get; set; }
        public bool HideReporting { get; set; }
        public string Tooltip { get; set; }
        public bool? Mandatory { get; set; }
        public JObject Additional { get; set; }
        public JToken Value { get; set; }
        public string DisplayValue { get; set; }
        public long? ProfileId { get; set; }


        public class FormElementDate
        {
            public bool CurrentDate { get; set; }
            public bool DateAndTime { get; set; }
        }

        public class FormElementText
        {
            public int? MaxLength { get; set; }
        }

        public class FormElementNumber
        {
            public object Max { get; set; }
            public object Min { get; set; }
            public string Prefix { get; set; }
            public string Suffix { get; set; }
        }

        public class FormElementAutoNumber
        {
            public string Format { get; set; }
        }

        public class FormElementTable
        {
            public bool AlterColumns { get; set; }
            public bool AlterRows { get; set; }
        }

        public class FormElementList
        {
            public string ListSize { get; set; }
        }
    }
}
