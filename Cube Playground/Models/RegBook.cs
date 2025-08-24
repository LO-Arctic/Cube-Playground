namespace Cube_Playground.Models
{
    internal class RegBook : RegBase
    {
        public Guid SourceId { get; set; }
        public int BookId { get; set; }
        public int BookVersionId { get; set; }
        public string IssuanceTypeCategoryName { get; set; }
        public Guid? IssuanceTypeCategorySourceId { get; set; }
        public Guid? CubeJurisdictionUuid { get; set; }
        public Guid? CubeIssuingBodyUuid { get; set; }
        public string CubeLink { get; set; }

        public List<CustomEntity> RegLinks = new();
        public string RegSummary { get; set; }
        public DateTime? DqUpdateDate { get; set; }
        public bool IsLatestPublishedVersion { get; set; }
    }
}
