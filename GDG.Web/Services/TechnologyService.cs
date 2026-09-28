using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GDG.Application.DTOs.Technology;
using GDG.Application.Interfaces;

namespace GDG.Web.Services
{
    public class TechnologyService : ITechnologyService
    {
        private static readonly List<TechnologyDto> _technologies = new();

        public Task<List<TechnologyDto>> GetAllAsync()
        {
            return Task.FromResult(_technologies.OrderBy(t => t.Name).ToList());
        }

        public Task<TechnologyDto?> GetByIdAsync(int id)
        {
            var tech = _technologies.FirstOrDefault(t => t.Id == id);
            return Task.FromResult(tech);
        }

        public Task<TechnologyDto> CreateAsync(CreateTechnologyDto dto)
        {
            var nextId = _technologies.Any() ? _technologies.Max(t => t.Id) + 1 : 1;
            var tech = new TechnologyDto
            {
                Id = nextId,
                Name = dto.Name,
                IconClass = dto.IconClass ?? "bi-code-slash",
                ColorHex = string.IsNullOrWhiteSpace(dto.ColorHex) ? "#4285F4" : dto.ColorHex
            };
            _technologies.Add(tech);
            return Task.FromResult(tech);
        }

        public Task<bool> DeleteAsync(int id)
        {
            var item = _technologies.FirstOrDefault(t => t.Id == id);
            if (item != null)
            {
                _technologies.Remove(item);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }
    }
}
