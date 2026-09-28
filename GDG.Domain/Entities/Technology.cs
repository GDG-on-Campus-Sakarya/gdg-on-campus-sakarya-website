
namespace GDG.Domain.Entities
{
    public class Technology
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? IconClass { get; set; } // e.g. devicon-csharp-plain, bi-code-slash
        public string? ColorHex { get; set; }  // e.g. #4285F4 (Google Blue), #0F9D58 (Green)

        // Many-to-Many relationship
        public ICollection<Project> Projects { get; set; } = new List<Project>();
    }
}