namespace Cube_Playground.Enums
{
    public enum FormElementType
    {
        None = -1,
        Header = 0,
        Text = 6, // SubHeader
        TextField = 1,
        TextArea = 2,
        SingleSelect = 3,
        MultiSelect = 4,
        DateField = 5,
        Table = 7,
        NumberField = 9,
        Separator = 10,
        Paragraph = 11,
        Boolean = 12,
        Attachment = 13,
        Signature = 14,
        AutoNumber = 15,
        CheckboxList = 16,
        Spacer = 17,
        DateTimeField = 99, // Only used for formatting
        Custom = 100, // RAP Fields
    }
}
