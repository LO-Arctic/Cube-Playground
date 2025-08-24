namespace Cube_Playground.Models
{
    internal class RegSection : RegBase
    {
        public int? ParentId { get; set; }

        public int SectionId { get; set; }

        public int Level { get; set; }

        public Guid? SourceId { get; set; }

        public Guid? BookSourceId { get; set; }

        public bool IsPdf { get; set; }

        public string Content { get; set; }

        public string SuggestedTitle { get; set; }
        public string Version { get; set; }
        public List<Ontology> Ontology { get; set; }
        public List<RegInsight> Insight { get; set; }
        public string Note { get; set; }
        public List<CustomEntity> RegLinks = new();
    }
}
