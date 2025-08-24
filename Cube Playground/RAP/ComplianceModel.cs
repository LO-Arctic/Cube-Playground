namespace Cube_Playground.RAP
{
    internal class ComplianceModel
    {
        public int FormId { get; set; }
        public string FormName { get; set; }
        public string DomainName { get; set; }
        public List<ComplianceTier> ComplianceTiers { get; set; }
        public bool DisplayRationaleContent { get; set; }
        public string Rationale { get; set; }
    }
}
