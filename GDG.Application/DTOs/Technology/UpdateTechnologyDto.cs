using System.ComponentModel.DataAnnotations;

namespace GDG.Application.DTOs.Technology
{
    public class UpdateTechnologyDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Teknoloji adı zorunludur.")]
        [StringLength(50, ErrorMessage = "En fazla 50 karakter olabilir.")]
        public string Name { get; set; } = string.Empty;

        public string? IconClass { get; set; }

        public string? ColorHex { get; set; }
    }
}
