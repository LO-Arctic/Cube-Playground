namespace Cube_Playground.Models
{
    internal class RegBase : CubeResponse
    {
        public int VersionOrdinal { get; set; }
        public string Title { get; set; }
        public string NativeTitle { get; set; }
        public string BookName { get; set; }
        public string NativeBookName { get; set; }
        public string IssuanceTitle { get; set; }
        public string NativeIssuanceTitle { get; set; }
        public string Citation { get; set; }
        public DateTime? SubscriptionDate { get; set; }
        public DateTime? ComplianceDate { get; set; }
        public DateTime? IssuanceDate { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? LoadDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string IssuanceTypeName { get; set; }
        public Guid? IssuanceTypeSourceId { get; set; }
        public string JurisdictionName { get; set; }
        public string IssuingBodyName { get; set; }
        public Guid? IssuingBodySourceId { get; set; }
        public string LinkURL { get; set; }
        public Guid? ProfileId { get; set; }
        public string CubeIssuingDepartment { get; set; }
        public string CubeApplicableJurisdiction { get; set; }
        public List<CustomEntity> Attributes { get; set; }
        public string ChangeType { get; set; }
        public DateTime? TranslationFixDate { get; set; }
        public DateTime? TranslationFixed { get; set; }
        public List<CustomEntity> CustomEntities { get; set; }
    }
}