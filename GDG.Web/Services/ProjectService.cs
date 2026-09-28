using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GDG.Application.DTOs.Project;
using GDG.Application.DTOs.Technology;
using GDG.Application.Interfaces;

namespace GDG.Web.Services
{
    public class ProjectService : IProjectService
    {
        private readonly ITechnologyService _technologyService;

        private static readonly List<ProjectDto> _projects = new()
        {
            new ProjectDto
            {
                Id = 1,
                Name = "GDG on Campus Sakarya Web Sitesi",
                Description = "Topluluk üyelerimizi, etkinliklerimizi, ekiplerimizi ve projelerimizi sergilediğimiz modern, açık kaynaklı web platformumuz.",
                ImageUrl = "https://images.unsplash.com/photo-1522071820081-009f0129c71c?auto=format&fit=crop&w=800&q=80",
                GitHubUrl = "https://github.com/GDG-on-Campus-Sakarya/gdg-on-campus-sakarya-website",
                LiveDemoUrl = "https://gdgsakarya.com",
                IsFeatured = true,
                CreatedAt = DateTime.UtcNow.AddDays(-14),
                Technologies = new List<TechnologyDto>
                {
                    new TechnologyDto { Id = 1, Name = "C# / .NET 8", ColorHex = "#512BD4", IconClass = "bi-filetype-cs" },
                    new TechnologyDto { Id = 2, Name = "Blazor", ColorHex = "#5C2D91", IconClass = "bi-window-stack" },
                    new TechnologyDto { Id = 3, Name = "PostgreSQL", ColorHex = "#4169E1", IconClass = "bi-database" }
                }
            },
            new ProjectDto
            {
                Id = 2,
                Name = "Sakarya Kampüs Asistanı",
                Description = "Öğrencilerin kampüs içindeki yemekhane menüleri, derslikler ve kulüp etkinliklerine anlık ulaşmasını sağlayan mobil uygulama.",
                ImageUrl = "https://images.unsplash.com/photo-1512941937669-90a1b58e7e9c?auto=format&fit=crop&w=800&q=80",
                GitHubUrl = "https://github.com/GDG-on-Campus-Sakarya/campus-assistant-app",
                LiveDemoUrl = null,
                IsFeatured = true,
                CreatedAt = DateTime.UtcNow.AddDays(-30),
                Technologies = new List<TechnologyDto>
                {
                    new TechnologyDto { Id = 4, Name = "Flutter", ColorHex = "#02569B", IconClass = "bi-phone" },
                    new TechnologyDto { Id = 6, Name = "Google Cloud", ColorHex = "#4285F4", IconClass = "bi-cloud" }
                }
            },
            new ProjectDto
            {
                Id = 3,
                Name = "GDG AI Soru-Cevap Botu",
                Description = "Gemini API ve Python kullanılarak geliştirilen, teknik atölyelerimizde öğrencilerin sorularını yanıtlayan akıllı asistan.",
                ImageUrl = "https://images.unsplash.com/photo-1677442136019-21780efad99a?auto=format&fit=crop&w=800&q=80",
                GitHubUrl = "https://github.com/GDG-on-Campus-Sakarya/gemini-study-bot",
                LiveDemoUrl = "https://t.me/gdg_sakarya_bot",
                IsFeatured = false,
                CreatedAt = DateTime.UtcNow.AddDays(-45),
                Technologies = new List<TechnologyDto>
                {
                    new TechnologyDto { Id = 5, Name = "Python", ColorHex = "#3776AB", IconClass = "bi-filetype-py" },
                    new TechnologyDto { Id = 7, Name = "TensorFlow / AI", ColorHex = "#FF6F00", IconClass = "bi-cpu" }
                }
            }
        };

        public ProjectService(ITechnologyService technologyService)
        {
            _technologyService = technologyService;
        }

        public Task<List<ProjectDto>> GetAllAsync(int? technologyId = null, bool? featuredOnly = null)
        {
            var query = _projects.AsQueryable();

            if (technologyId.HasValue && technologyId.Value > 0)
            {
                query = query.Where(p => p.Technologies.Any(t => t.Id == technologyId.Value));
            }

            if (featuredOnly.HasValue && featuredOnly.Value)
            {
                query = query.Where(p => p.IsFeatured);
            }

            return Task.FromResult(query.OrderByDescending(p => p.IsFeatured).ThenByDescending(p => p.CreatedAt).ToList());
        }

        public Task<ProjectDto?> GetByIdAsync(int id)
        {
            var project = _projects.FirstOrDefault(p => p.Id == id);
            return Task.FromResult(project);
        }

        public async Task<ProjectDto> CreateAsync(CreateProjectDto dto)
        {
            var nextId = _projects.Any() ? _projects.Max(p => p.Id) + 1 : 1;
            var allTechs = await _technologyService.GetAllAsync();
            var selectedTechs = allTechs.Where(t => dto.SelectedTechnologyIds.Contains(t.Id)).ToList();

            var project = new ProjectDto
            {
                Id = nextId,
                Name = dto.Name,
                Description = dto.Description,
                ImageUrl = string.IsNullOrWhiteSpace(dto.ImageUrl)
                    ? "https://images.unsplash.com/photo-1555066931-4365d14bab8c?auto=format&fit=crop&w=800&q=80"
                    : dto.ImageUrl,
                GitHubUrl = dto.GitHubUrl,
                LiveDemoUrl = dto.LiveDemoUrl,
                IsFeatured = dto.IsFeatured,
                CreatedAt = DateTime.UtcNow,
                Technologies = selectedTechs
            };

            _projects.Add(project);
            return project;
        }

        public async Task<bool> UpdateAsync(UpdateProjectDto dto)
        {
            var existing = _projects.FirstOrDefault(p => p.Id == dto.Id);
            if (existing == null) return false;

            var allTechs = await _technologyService.GetAllAsync();
            var selectedTechs = allTechs.Where(t => dto.SelectedTechnologyIds.Contains(t.Id)).ToList();

            existing.Name = dto.Name;
            existing.Description = dto.Description;
            if (!string.IsNullOrWhiteSpace(dto.ImageUrl)) existing.ImageUrl = dto.ImageUrl;
            existing.GitHubUrl = dto.GitHubUrl;
            existing.LiveDemoUrl = dto.LiveDemoUrl;
            existing.IsFeatured = dto.IsFeatured;
            existing.Technologies = selectedTechs;

            return true;
        }

        public Task<bool> DeleteAsync(int id)
        {
            var existing = _projects.FirstOrDefault(p => p.Id == id);
            if (existing != null)
            {
                _projects.Remove(existing);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }
    }
}
