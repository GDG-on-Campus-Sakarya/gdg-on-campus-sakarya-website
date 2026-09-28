using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GDG.Application.DTOs.Member
{
    public class MemberDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public int TeamId { get; set; }
        public string? Email { get; set; }
        public string? GitHubUrl { get; set; }
        public string? LinkedInUrl { get; set; }
        public List<int> ProjectIds { get; set; } = new();
        public List<string> ProjectNames { get; set; } = new();
    }

    public class CreateMemberDto
    {
        [Required(ErrorMessage = "Üye adı ve soyadı zorunludur.")]
        [StringLength(100, ErrorMessage = "En fazla 100 karakter olabilir.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Üyenin rolü veya unvanı zorunludur.")]
        public string Role { get; set; } = string.Empty;

        public int TeamId { get; set; } = 1;

        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        public string? Email { get; set; }

        public string? GitHubUrl { get; set; }
        public string? LinkedInUrl { get; set; }

        public List<int> SelectedProjectIds { get; set; } = new();
    }

    public class UpdateMemberDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Üye adı ve soyadı zorunludur.")]
        [StringLength(100, ErrorMessage = "En fazla 100 karakter olabilir.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Üyenin rolü veya unvanı zorunludur.")]
        public string Role { get; set; } = string.Empty;

        public int TeamId { get; set; }

        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        public string? Email { get; set; }

        public string? GitHubUrl { get; set; }
        public string? LinkedInUrl { get; set; }

        public List<int> SelectedProjectIds { get; set; } = new();
    }
}
