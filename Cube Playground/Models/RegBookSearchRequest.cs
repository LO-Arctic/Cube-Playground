namespace Cube_Playground.Models
{
    internal class RegBookSearchRequest : CubeRequest
    {
        public List<string>? SourceId { get; set; }

        public List<string>? IssuanceType { get; set; }

        public List<string>? IssuanceCategory { get; set; }

        public List<string>? Concept { get; set; }

        public List<string>? IssuingBody { get; set; }

        public List<string>? Jurisdiction { get; set; }

        public List<string>? Tag { get; set; }

        public List<string>? TileAndCitation { get; set; }

        public List<string>? RegSection { get; set; }

        public List<DateRange>? ComplianceDate { get; set; }

        public List<DateRange>? IssuanceDate { get; set; }

        public List<DateRange>? EffectiveDate { get; set; }

        public List<DateRange>? LoadDate { get; set; }

        public List<DateRange>? UpdateDate { get; set; }

        public string? BookPublishStatus { get; set; }

        public bool? IsLatestPublishedVersion { get; set; }

        public string? OrderBy { get; set; }
    }
}
