
namespace GDG.Domain.Entities
{
    public class Technology
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? IconClass { get; set; } 
        public string? ColorHex { get; set; } 

        public List<Project> Projects { get; set; } = new List<Project>();
    }
}