using System;

namespace GDG.Domain.Entities
{

    // event
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.UtcNow.AddDays(7);
        public string Location { get; set; } = string.Empty;
        public string? ImageUrl { get; set; } // ileride db içerisinde image tutarız 
        public string? RegistrationUrl { get; set; }   // kayıt url'si veya formu
    }
}