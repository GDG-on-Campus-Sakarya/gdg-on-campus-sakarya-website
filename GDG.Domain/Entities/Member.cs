namespace GDG.Domain.Entities
{
    public class Member
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }

        // Foreign Key
        public int TeamId { get; set; }

        // Navigation Property: EF Core bu sayede hangi Team'e ait olduğunu anlar
        public Team Team { get; set; }
    }
}