namespace Cube_Playground.Models
{
    internal class CustomEntity : CubeModel
    {
        public Guid GUID { get; set; }
        public string Name { get; set; }
        public Guid? ProfileID { get; set; }
        public DateTime? ProfilePublishedDate { get; set; }
        public bool? InProfile { get; set; }
    }
}
