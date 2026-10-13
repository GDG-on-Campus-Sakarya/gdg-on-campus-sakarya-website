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
    public class TechnologyService : ITechnologyService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public TechnologyService(
            IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<List<TechnologyDto>> GetAllAsync()
        {
            await using var db = await _contextFactory.CreateDbContextAsync();

            var technologies = await db.Technologies
                .AsNoTracking()
                .OrderBy(t => t.Name)
                .ToListAsync();

            return technologies.Select(ToDto).ToList();
        }

        public async Task<TechnologyDto?> GetByIdAsync(int id)
        {
            await using var db = await _contextFactory.CreateDbContextAsync();

            var technology = await db.Technologies
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);

            return technology == null ? null : ToDto(technology);
        }

        public async Task<TechnologyDto> CreateAsync(CreateTechnologyDto dto)
        {
            await using var db = await _contextFactory.CreateDbContextAsync();

            var technology = new Technology
            {
                Name = dto.Name.Trim(),
                IconClass = string.IsNullOrWhiteSpace(dto.IconClass)
                    ? "bi-code-slash"
                    : dto.IconClass.Trim(),
                ColorHex = string.IsNullOrWhiteSpace(dto.ColorHex)
                    ? "#4285F4"
                    : dto.ColorHex.Trim()
            };

            db.Technologies.Add(technology);
            await db.SaveChangesAsync();

            return ToDto(technology);
        }

        public async Task<bool> UpdateAsync(UpdateTechnologyDto dto)
        {
            await using var db = await _contextFactory.CreateDbContextAsync();

            var technology = await db.Technologies
                .FirstOrDefaultAsync(t => t.Id == dto.Id);

            if (technology == null)
                return false;

            technology.Name = dto.Name.Trim();
            technology.IconClass = string.IsNullOrWhiteSpace(dto.IconClass)
                ? "bi-code-slash"
                : dto.IconClass.Trim();
            technology.ColorHex = string.IsNullOrWhiteSpace(dto.ColorHex)
                ? "#4285F4"
                : dto.ColorHex.Trim();

            await db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await using var db = await _contextFactory.CreateDbContextAsync();

            var technology = await db.Technologies
                .FirstOrDefaultAsync(t => t.Id == id);

            if (technology == null)
                return false;

            db.Technologies.Remove(technology);
            await db.SaveChangesAsync();

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