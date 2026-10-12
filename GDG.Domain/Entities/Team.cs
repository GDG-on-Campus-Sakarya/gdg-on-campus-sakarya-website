
using System.Collections.Generic;

namespace GDG.Domain.Entities
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public List<Member> Members { get; set; } = new List<Member>();
    }
}