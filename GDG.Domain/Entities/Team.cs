
using System.Collections.Generic;

namespace GDG.Domain.Entities
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // Bir ekibin birden fazla üyesi olabilir (One-to-Many)
        public ICollection<Member> Members { get; set; } = new List<Member>();
    }
}