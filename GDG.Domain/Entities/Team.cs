
namespace GDG.Domain.Entities
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        // Bir ekibin birden fazla üyesi olabilir (One-to-Many)
        public ICollection<Member> Members { get; set; }
    }
}