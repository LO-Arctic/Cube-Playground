using Cube_Playground.Enums;

namespace Cube_Playground.RAP
{
    internal class FormElementData
    {
        public Guid Ref { get; set; } = Guid.Empty;
        public FormElementType Type { get; set; }
        public string Label { get; set; }
        public object Value { get; set; }
        public string DisplayValue { get; set; }
        public bool? Visible { get; set; }
    }
}
