using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GDG.Application.DTOs.Project;
using GDG.Application.Interfaces;
using GDG.Domain.Entities;
using GDG.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace GDG.Infrastructure.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public ProjectService(
            IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<List<ProjectDto>> GetAllAsync(
            int? technologyId = null,
            bool? featuredOnly = null)
        {
            await using var db = await _contextFactory.CreateDbContextAsync();

            var query = db.Projects
                .AsNoTracking()
                .Include(p => p.Technologies)
                .AsQueryable();

            if (technologyId.HasValue && technologyId.Value > 0)
            {
                query = query.Where(
                    p => p.Technologies.Any(
                        t => t.Id == technologyId.Value));
            }

            if (featuredOnly.HasValue && featuredOnly.Value)
            {
                query = query.Where(p => p.IsFeatured);
            }

            var projects = await query
                .OrderByDescending(p => p.IsFeatured)
                .ThenByDescending(p => p.CreatedAt)
                .ToListAsync();

            return projects.Select(ToDto).ToList();
        }

        public async Task<ProjectDto?> GetByIdAsync(int id)
        {
            await using var db = await _contextFactory.CreateDbContextAsync();

            var project = await db.Projects
                .AsNoTracking()
                .Include(p => p.Technologies)
                .FirstOrDefaultAsync(p => p.Id == id);

            return project == null ? null : ToDto(project);
        }

        public async Task<ProjectDto> CreateAsync(CreateProjectDto dto)
        {
            await using var db = await _contextFactory.CreateDbContextAsync();

            var project = new Project
            {
                Name = dto.Name.Trim(),
                Description = dto.Description.Trim(),
                ImageUrl = string.IsNullOrWhiteSpace(dto.ImageUrl)
                    ? "https://images.unsplash.com/photo-1555066931-4365d14bab8c?auto=format&fit=crop&w=800&q=80"
                    : dto.ImageUrl.Trim(),
                GitHubUrl = NullIfEmpty(dto.GitHubUrl),
                LiveDemoUrl = NullIfEmpty(dto.LiveDemoUrl),
                IsFeatured = dto.IsFeatured,
                CreatedAt = DateTime.UtcNow,
                Technologies = await LoadTechnologiesAsync(
                    db, dto.SelectedTechnologyIds)
            };

            db.Projects.Add(project);
            await db.SaveChangesAsync();

            return ToDto(project);
        }

        public async Task<bool> UpdateAsync(UpdateProjectDto dto)
        {
            await using var db = await _contextFactory.CreateDbContextAsync();

            var project = await db.Projects
                .Include(p => p.Technologies)
                .FirstOrDefaultAsync(p => p.Id == dto.Id);

            if (project == null)
                return false;

            project.Name = dto.Name.Trim();
            project.Description = dto.Description.Trim();
            project.ImageUrl = string.IsNullOrWhiteSpace(dto.ImageUrl)
                ? (string.IsNullOrWhiteSpace(project.ImageUrl)
                    ? "https://images.unsplash.com/photo-1555066931-4365d14bab8c?auto=format&fit=crop&w=800&q=80"
                    : project.ImageUrl)
                : dto.ImageUrl.Trim();

            project.GitHubUrl = NullIfEmpty(dto.GitHubUrl);
            project.LiveDemoUrl = NullIfEmpty(dto.LiveDemoUrl);
            project.IsFeatured = dto.IsFeatured;

            var selectedIds = dto.SelectedTechnologyIds?
                .Distinct()
                .ToList() ?? new List<int>();

            var currentIds = project.Technologies
                .Select(t => t.Id)
                .ToList();

            var toRemove = project.Technologies
                .Where(t => !selectedIds.Contains(t.Id))
                .ToList();

            foreach (var technology in toRemove)
            {
                project.Technologies.Remove(technology);
            }

            var toAddIds = selectedIds
                .Where(id => !currentIds.Contains(id))
                .ToList();

            if (toAddIds.Count > 0)
            {
                var newTechnologies = await db.Technologies
                    .Where(t => toAddIds.Contains(t.Id))
                    .ToListAsync();

                project.Technologies.AddRange(newTechnologies);
            }

            await db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await using var db = await _contextFactory.CreateDbContextAsync();

            var project = await db.Projects
                .Include(p => p.Technologies)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (project == null)
                return false;

            db.Projects.Remove(project);
            await db.SaveChangesAsync();

            return true;
        }

        private async Task<List<Technology>> LoadTechnologiesAsync(
            ApplicationDbContext db,
            List<int>? technologyIds)
        {
            if (technologyIds == null || technologyIds.Count == 0)
                return new List<Technology>();

            var distinctIds = technologyIds.Distinct().ToList();

            return await db.Technologies
                .Where(t => distinctIds.Contains(t.Id))
                .ToListAsync();
        }

        internal static ProjectDto ToDto(Project p) => new ProjectDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            ImageUrl = p.ImageUrl,
            GitHubUrl = p.GitHubUrl,
            LiveDemoUrl = p.LiveDemoUrl,
            IsFeatured = p.IsFeatured,
            CreatedAt = p.CreatedAt,
            Technologies = p.Technologies
                .Select(TechnologyService.ToDto)
                .ToList()
        };

        private static string? NullIfEmpty(string? value) =>
            string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}