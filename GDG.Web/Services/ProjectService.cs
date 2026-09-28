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

        private static readonly List<ProjectDto> _projects = new();

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
