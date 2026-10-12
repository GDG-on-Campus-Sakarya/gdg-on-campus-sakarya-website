using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GDG.Application.DTOs.Member;
using GDG.Application.Interfaces;
using GDG.Domain.Entities;
using GDG.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace GDG.Infrastructure.Services
{
    public class MemberService : IMemberService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public MemberService(
            IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<List<MemberDto>> GetAllAsync()
        {
            await using var db = await _contextFactory.CreateDbContextAsync();

            var members = await db.Members
                .AsNoTracking()
                .Include(m => m.Projects)
                .OrderBy(m => m.FullName)
                .ToListAsync();

            return members.Select(ToDto).ToList();
        }

        public async Task<MemberDto?> GetByIdAsync(int id)
        {
            await using var db = await _contextFactory.CreateDbContextAsync();

            var member = await db.Members
                .AsNoTracking()
                .Include(m => m.Projects)
                .FirstOrDefaultAsync(m => m.Id == id);

            return member == null ? null : ToDto(member);
        }

        public async Task<MemberDto> CreateAsync(CreateMemberDto dto)
        {
            await using var db = await _contextFactory.CreateDbContextAsync();

            await EnsureTeamExistsAsync(db, dto.TeamId);

            var member = new Member
            {
                FullName = dto.FullName.Trim(),
                Role = dto.Role.Trim(),
                TeamId = dto.TeamId,
                AvatarUrl = NullIfEmpty(dto.AvatarUrl),
                Email = NullIfEmpty(dto.Email),
                GitHubUrl = NullIfEmpty(dto.GitHubUrl),
                LinkedInUrl = NullIfEmpty(dto.LinkedInUrl),
                Projects = await LoadProjectsAsync(
                    db, dto.SelectedProjectIds)
            };

            db.Members.Add(member);
            await db.SaveChangesAsync();

            return ToDto(member);
        }

        public async Task<bool> UpdateAsync(UpdateMemberDto dto)
        {
            await using var db = await _contextFactory.CreateDbContextAsync();

            var member = await db.Members
                .Include(m => m.Projects)
                .FirstOrDefaultAsync(m => m.Id == dto.Id);

            if (member == null)
                return false;

            await EnsureTeamExistsAsync(db, dto.TeamId);

            member.FullName = dto.FullName.Trim();
            member.Role = dto.Role.Trim();
            member.TeamId = dto.TeamId;
            member.AvatarUrl = NullIfEmpty(dto.AvatarUrl);
            member.Email = NullIfEmpty(dto.Email);
            member.GitHubUrl = NullIfEmpty(dto.GitHubUrl);
            member.LinkedInUrl = NullIfEmpty(dto.LinkedInUrl);

            member.Projects.Clear();
            member.Projects.AddRange(
                await LoadProjectsAsync(db, dto.SelectedProjectIds));

            await db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await using var db = await _contextFactory.CreateDbContextAsync();

            var member = await db.Members
                .FirstOrDefaultAsync(m => m.Id == id);

            if (member == null)
                return false;

            db.Members.Remove(member);
            await db.SaveChangesAsync();

            return true;
        }

        private async Task EnsureTeamExistsAsync(
            ApplicationDbContext db,
            int teamId)
        {
            if (!await db.Teams.AnyAsync(t => t.Id == teamId))
            {
                throw new InvalidOperationException(
                    $"{teamId} numaralı ekip bulunamadı. Üye eklemeden önce ekibi oluşturun.");
            }
        }

        private async Task<List<Project>> LoadProjectsAsync(
            ApplicationDbContext db,
            List<int> projectIds)
        {
            if (projectIds.Count == 0)
                return new List<Project>();

            return await db.Projects
                .Where(p => projectIds.Contains(p.Id))
                .ToListAsync();
        }

        internal static MemberDto ToDto(Member m) => new MemberDto
        {
            Id = m.Id,
            FullName = m.FullName,
            Role = m.Role,
            TeamId = m.TeamId,
            AvatarUrl = m.AvatarUrl,
            Email = m.Email,
            GitHubUrl = m.GitHubUrl,
            LinkedInUrl = m.LinkedInUrl,
            ProjectIds = m.Projects.Select(p => p.Id).ToList(),
            ProjectNames = m.Projects.Select(p => p.Name).ToList()
        };

        private static string? NullIfEmpty(string? value) =>
            string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}