
namespace GDG.Domain.Entities
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string GitHubUrl { get; set; }
        public List<Technology> Technologies { get; set; } = new List<Technology>();
        public List<Member> Members { get; set; } = new List<Member>();
    }
}