using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GDG.Application.DTOs.Technology;
using GDG.Application.Interfaces;
using GDG.Domain.Entities;
using GDG.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace GDG.Infrastructure.Services
{
    /// <summary>
    /// ITechnologyService'in veritabanına (PostgreSQL / EF Core) bağlı uygulaması.
    /// </summary>
    public class TechnologyService : ITechnologyService
    {
        private readonly ApplicationDbContext _db;

        public TechnologyService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<TechnologyDto>> GetAllAsync()
        {
            var technologies = await _db.Technologies
                .AsNoTracking()
                .OrderBy(t => t.Name)
                .ToListAsync();

            return technologies.Select(ToDto).ToList();
        }

        public async Task<TechnologyDto?> GetByIdAsync(int id)
        {
            var tech = await _db.Technologies
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);

            return tech == null ? null : ToDto(tech);
        }

        public async Task<TechnologyDto> CreateAsync(CreateTechnologyDto dto)
        {
            var tech = new Technology
            {
                Name = dto.Name.Trim(),
                IconClass = string.IsNullOrWhiteSpace(dto.IconClass) ? "bi-code-slash" : dto.IconClass.Trim(),
                ColorHex = string.IsNullOrWhiteSpace(dto.ColorHex) ? "#4285F4" : dto.ColorHex.Trim()
            };

            _db.Technologies.Add(tech);
            await _db.SaveChangesAsync();

            return ToDto(tech);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var tech = await _db.Technologies.FirstOrDefaultAsync(t => t.Id == id);
            if (tech == null) return false;

            _db.Technologies.Remove(tech);
            await _db.SaveChangesAsync();
            return true;
        }

        internal static TechnologyDto ToDto(Technology t) => new TechnologyDto
        {
            Id = t.Id,
            Name = t.Name,
            IconClass = t.IconClass,
            ColorHex = t.ColorHex
        };
    }
}
