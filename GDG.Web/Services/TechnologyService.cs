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
        private static readonly List<TechnologyDto> _technologies = new()
        {
            new TechnologyDto { Id = 1, Name = "C# / .NET 8", ColorHex = "#512BD4", IconClass = "bi-filetype-cs" },
            new TechnologyDto { Id = 2, Name = "Blazor", ColorHex = "#5C2D91", IconClass = "bi-window-stack" },
            new TechnologyDto { Id = 3, Name = "PostgreSQL", ColorHex = "#4169E1", IconClass = "bi-database" },
            new TechnologyDto { Id = 4, Name = "Flutter", ColorHex = "#02569B", IconClass = "bi-phone" },
            new TechnologyDto { Id = 5, Name = "Python", ColorHex = "#3776AB", IconClass = "bi-filetype-py" },
            new TechnologyDto { Id = 6, Name = "Google Cloud", ColorHex = "#4285F4", IconClass = "bi-cloud" },
            new TechnologyDto { Id = 7, Name = "TensorFlow / AI", ColorHex = "#FF6F00", IconClass = "bi-cpu" }
        };

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
