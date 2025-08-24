namespace Cube_Playground.Models
{
    internal class Attribute : CubeModel
    {
        public string Id { get; set; }
        public Guid? SourceIdIdentifier { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public bool IsRestrictive { get; set; }
        public bool IsCustom { get; set; }
        public DateTime? createdDate { get; set; }
        public string TagComments { get; set; }
        public string TagCommentsNarrative { get; set; }
        public DateTime? tagCommentsCreatedDate { get; set; }
        public string TagCommentsCreatedBy { get; set; }
        public Guid? TagValueSourceId { get; set; }
        public bool IsContainer { get; set; }
        public Guid? ContainerTagSourceId { get; set; }
        public string CustomerReference { get; set; }
        public string Description { get; set; }
        public DateTime? ArchiveDate { get; set; }
        public string ArchivedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public List<string> Metadata { get; set; }
        public bool IsBookLevel { get; set; }
    }
}
