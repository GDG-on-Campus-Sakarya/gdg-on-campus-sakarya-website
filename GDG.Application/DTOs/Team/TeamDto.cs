using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GDG.Application.DTOs.Member;

namespace GDG.Application.DTOs.Team
{
    public class TeamDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<MemberDto> Members { get; set; } = new();
    }

    public class CreateTeamDto
    {
        [Required(ErrorMessage = "Ekip adı zorunludur.")]
        [StringLength(100, ErrorMessage = "Ekip adı en fazla 100 karakter olabilir.")]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public List<int> SelectedMemberIds { get; set; } = new();
    }

    public class UpdateTeamDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ekip adı zorunludur.")]
        [StringLength(100, ErrorMessage = "Ekip adı en fazla 100 karakter olabilir.")]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public List<int> SelectedMemberIds { get; set; } = new();
    }
}
