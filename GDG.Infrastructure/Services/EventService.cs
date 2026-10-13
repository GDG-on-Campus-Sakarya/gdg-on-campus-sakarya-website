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
    public class EventService : IEventService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public EventService(
            IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<List<EventDto>> GetAllAsync()
        {
            await using var db = await _contextFactory.CreateDbContextAsync();

            var events = await db.Events
                .AsNoTracking()
                .OrderByDescending(e => e.Date)
                .ToListAsync();

            return events.Select(ToDto).ToList();
        }

        public async Task<EventDto?> GetByIdAsync(int id)
        {
            await using var db = await _contextFactory.CreateDbContextAsync();

            var entity = await db.Events
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);

            return entity == null ? null : ToDto(entity);
        }

        public async Task<EventDto> CreateAsync(CreateEventDto dto)
        {
            await using var db = await _contextFactory.CreateDbContextAsync();

            var entity = new Event
            {
                Title = dto.Title.Trim(),
                Description = dto.Description.Trim(),
                Date = TurkeyTime.ToUtc(dto.Date),
                Location = dto.Location.Trim(),
                ImageUrl = NullIfEmpty(dto.ImageUrl),
                RegistrationUrl = NullIfEmpty(dto.RegistrationUrl)
            };

            db.Events.Add(entity);
            await db.SaveChangesAsync();

            return ToDto(entity);
        }

        public async Task<bool> UpdateAsync(UpdateEventDto dto)
        {
            await using var db = await _contextFactory.CreateDbContextAsync();

            var entity = await db.Events
                .FirstOrDefaultAsync(e => e.Id == dto.Id);

            if (entity == null)
                return false;

            entity.Title = dto.Title.Trim();
            entity.Description = dto.Description.Trim();
            entity.Date = TurkeyTime.ToUtc(dto.Date);
            entity.Location = dto.Location.Trim();
            entity.ImageUrl = NullIfEmpty(dto.ImageUrl);
            entity.RegistrationUrl = NullIfEmpty(dto.RegistrationUrl);

            await db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await using var db = await _contextFactory.CreateDbContextAsync();

            var entity = await db.Events
                .FirstOrDefaultAsync(e => e.Id == id);

            if (entity == null)
                return false;

            db.Events.Remove(entity);
            await db.SaveChangesAsync();

            return true;
        }

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

        private static string? NullIfEmpty(string? value) =>
            string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}