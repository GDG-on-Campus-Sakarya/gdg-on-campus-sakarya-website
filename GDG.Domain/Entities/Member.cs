using System.Collections.Generic;

namespace GDG.Domain.Entities
{
    // üyeler
    public class Member
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        public string? AvatarUrl { get; set; }
        public string? Email { get; set; }
        public string? GitHubUrl { get; set; }
        public string? LinkedInUrl { get; set; }

        // komüniteler arası id'ler proje ekibi id 1 :D
        public int TeamId { get; set; }

        // Navigation Properties
        public Team? Team { get; set; }
        public List<Project> Projects { get; set; } = new List<Project>();
    }
}