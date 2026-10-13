using System.Collections.Generic;

namespace GDG.Domain.Entities
{
    public class Member
    {
        public int Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public string? AvatarUrl { get; set; }

        public string? Email { get; set; }

        public string? GitHubUrl { get; set; }

        public string? LinkedInUrl { get; set; }

        // Üyenin ana ekibi.
        public int TeamId { get; set; }

        // Ana ekip ilişkisi.
        public Team? Team { get; set; }

        // Üyenin ayrıca dahil olduğu ekipler.
        public List<Team> AdditionalTeams { get; set; } = new();

        // İlişkili projeler.
        public List<Project> Projects { get; set; } = new();
    }
}