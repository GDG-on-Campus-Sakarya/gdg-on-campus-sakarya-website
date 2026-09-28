using System.Collections.Generic;

namespace GDG.Domain.Entities
{

    // üyeler
    public class Member
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        // komüniteler arası id'ler proje ekibi id 1 :D
        public int TeamId { get; set; }

        // Navigation Property: EF Core bu sayede hangi Team'e ait olduğunu anlar
        public Team Team { get; set; }
        public List<Project> Projects { get; set; } = new List<Project>();
    }
}