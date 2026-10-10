using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GDG.Application.DTOs.Event;
using GDG.Application.Interfaces;
using GDG.Domain.Entities;
using GDG.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace GDG.Infrastructure.Services
{
    /// <summary>
    /// IEventService'in veritabanına bağlı gerçek uygulaması.
    /// GDG.Web/Services/EventService.cs'teki mock'un yerini alır: veriler artık
    /// uygulama yeniden başlayınca kaybolmaz.
    /// </summary>
    public class EventService : IEventService
    {
        private readonly ApplicationDbContext _db;

        // Dependency Injection: DbContext'i ASP.NET verir (Program.cs'teki AddDbContext sayesinde).
        public EventService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<EventDto>> GetAllAsync()
        {
            // SQL: SELECT * FROM "Events" ORDER BY "Date" DESC
            var events = await _db.Events
                .AsNoTracking()                      // sadece okuyoruz, EF takip etmesin
                .OrderByDescending(e => e.Date)
                .ToListAsync();                      // sorgu BURADA çalışır

            return events.Select(ToDto).ToList();
        }

        public async Task<EventDto?> GetByIdAsync(int id)
        {
            var entity = await _db.Events
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);

            return entity == null ? null : ToDto(entity);
        }

        public async Task<EventDto> CreateAsync(CreateEventDto dto)
        {
            var entity = new Event
            {
                Title = dto.Title.Trim(),
                Description = dto.Description.Trim(),
                Date = TurkeyTime.ToUtc(dto.Date),   // ← UTC'ye çevirmezsek Npgsql hata verir
                Location = dto.Location.Trim(),
                ImageUrl = NullIfEmpty(dto.ImageUrl),
                RegistrationUrl = NullIfEmpty(dto.RegistrationUrl)
            };

            _db.Events.Add(entity);        // EF not alır, henüz SQL yok
            await _db.SaveChangesAsync();  // INSERT burada çalışır, entity.Id dolar

            return ToDto(entity);
        }

        public async Task<bool> UpdateAsync(UpdateEventDto dto)
        {
            // AsNoTracking YOK: değiştireceğiz, EF takip etmeli.
            var entity = await _db.Events.FirstOrDefaultAsync(e => e.Id == dto.Id);
            if (entity == null) return false;

            entity.Title = dto.Title.Trim();
            entity.Description = dto.Description.Trim();
            entity.Date = TurkeyTime.ToUtc(dto.Date);
            entity.Location = dto.Location.Trim();
            entity.ImageUrl = NullIfEmpty(dto.ImageUrl);
            entity.RegistrationUrl = NullIfEmpty(dto.RegistrationUrl);

            // _db.Update(entity) gerekmez: EF hangi alanların değiştiğini kendisi fark eder (change tracking).
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _db.Events.FirstOrDefaultAsync(e => e.Id == id);
            if (entity == null) return false;

            _db.Events.Remove(entity);
            await _db.SaveChangesAsync();  // DELETE
            return true;
        }

        // Entity (UTC) → DTO (Türkiye saati). Dönüşüm tek bir yerde.
        private static EventDto ToDto(Event e) => new EventDto
        {
            Id = e.Id,
            Title = e.Title,
            Description = e.Description,
            Date = TurkeyTime.FromUtc(e.Date),
            Location = e.Location,
            ImageUrl = e.ImageUrl,
            RegistrationUrl = e.RegistrationUrl
        };

        // Boş kutu "" olarak gelir; veritabanında "" ile null karışmasın diye hep null saklıyoruz.
        private static string? NullIfEmpty(string? value) =>
            string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}
