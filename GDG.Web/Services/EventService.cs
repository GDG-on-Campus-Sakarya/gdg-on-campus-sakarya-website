using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GDG.Application.DTOs.Event;
using GDG.Application.Interfaces;

namespace GDG.Web.Services
{
    public class EventService : IEventService
    {
        private static readonly List<EventDto> _events = new();

        public Task<List<EventDto>> GetAllAsync()
        {
            return Task.FromResult(_events.OrderByDescending(e => e.Date).ToList());
        }

        public Task<EventDto?> GetByIdAsync(int id)
        {
            var item = _events.FirstOrDefault(e => e.Id == id);
            return Task.FromResult(item);
        }

        public Task<EventDto> CreateAsync(CreateEventDto dto)
        {
            var nextId = _events.Any() ? _events.Max(e => e.Id) + 1 : 1;
            var item = new EventDto
            {
                Id = nextId,
                Title = dto.Title,
                Description = dto.Description,
                Date = dto.Date,
                Location = dto.Location,
                ImageUrl = dto.ImageUrl,
                RegistrationUrl = dto.RegistrationUrl
            };
            _events.Add(item);
            return Task.FromResult(item);
        }

        public Task<bool> UpdateAsync(UpdateEventDto dto)
        {
            var existing = _events.FirstOrDefault(e => e.Id == dto.Id);
            if (existing == null) return Task.FromResult(false);

            existing.Title = dto.Title;
            existing.Description = dto.Description;
            existing.Date = dto.Date;
            existing.Location = dto.Location;
            existing.ImageUrl = dto.ImageUrl;
            existing.RegistrationUrl = dto.RegistrationUrl;

            return Task.FromResult(true);
        }

        public Task<bool> DeleteAsync(int id)
        {
            var existing = _events.FirstOrDefault(e => e.Id == id);
            if (existing != null)
            {
                _events.Remove(existing);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }
    }
}
