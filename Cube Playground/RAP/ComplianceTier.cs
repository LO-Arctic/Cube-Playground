namespace Cube_Playground.RAP
{
    internal class ComplianceTier
    {
        public string Name { get; set; }
        public int Order { get; set; }
        public string Description { get; set; }
        public string Rationale { get; set; }
        public List<ComplianceTier> ChildTiers { get; set; }
        public List<ComplianceObligation> Obligations { get; set; }
    }
}
