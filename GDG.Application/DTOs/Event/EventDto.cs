using System;
using System.ComponentModel.DataAnnotations;

namespace GDG.Application.DTOs.Event
{
    public class EventDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Location { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public string? RegistrationUrl { get; set; }
        public bool IsPast => Date < DateTime.UtcNow;
    }

    public class CreateEventDto
    {
        [Required(ErrorMessage = "Etkinlik başlığı zorunludur.")]
        [StringLength(150, ErrorMessage = "Başlık en fazla 150 karakter olabilir.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Etkinlik açıklaması zorunludur.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Etkinlik tarihi zorunludur.")]
        public DateTime Date { get; set; } = DateTime.UtcNow.AddDays(7);

        [Required(ErrorMessage = "Etkinlik konumu veya platformu zorunludur.")]
        public string Location { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }

        [Url(ErrorMessage = "Geçerli bir kayıt bağlantısı giriniz.")]
        public string? RegistrationUrl { get; set; }
    }

    public class UpdateEventDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Etkinlik başlığı zorunludur.")]
        [StringLength(150, ErrorMessage = "Başlık en fazla 150 karakter olabilir.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Etkinlik açıklaması zorunludur.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Etkinlik tarihi zorunludur.")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Etkinlik konumu veya platformu zorunludur.")]
        public string Location { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }

        [Url(ErrorMessage = "Geçerli bir kayıt bağlantısı giriniz.")]
        public string? RegistrationUrl { get; set; }
    }
}
