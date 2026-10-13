using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GDG.Application.DTOs.Team;
using GDG.Application.Interfaces;
using GDG.Domain.Entities;
using GDG.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace GDG.Infrastructure.Services
{
    public class TeamService : ITeamService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public TeamService(
            IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<List<TeamDto>> GetAllAsync()
        {
            await using var db = await _contextFactory.CreateDbContextAsync();

            var teams = await db.Teams
                .AsNoTracking()
                .Include(t => t.Members)
                    .ThenInclude(m => m.Projects)
                .OrderBy(t => t.Name)
                .ToListAsync();

            return teams.Select(ToDto).ToList();
        }

        public async Task<TeamDto?> GetByIdAsync(int id)
        {
            await using var db = await _contextFactory.CreateDbContextAsync();

            var team = await db.Teams
                .AsNoTracking()
                .Include(t => t.Members)
                    .ThenInclude(m => m.Projects)
                .FirstOrDefaultAsync(t => t.Id == id);

            return team == null ? null : ToDto(team);
        }

        public async Task<TeamDto> CreateAsync(CreateTeamDto dto)
        {
            await using var db = await _contextFactory.CreateDbContextAsync();

            var team = new Team
            {
                Name = dto.Name.Trim(),
                Description = dto.Description.Trim()
            };

            db.Teams.Add(team);
            await db.SaveChangesAsync();

            if (dto.SelectedMemberIds.Count > 0)
            {
                var members = await db.Members
                    .Where(m => dto.SelectedMemberIds.Contains(m.Id))
                    .ToListAsync();

                foreach (var member in members)
                {
                    member.TeamId = team.Id;
                }

                await db.SaveChangesAsync();
            }

            return (await GetByIdAsync(team.Id))!;
        }

        public async Task<bool> UpdateAsync(UpdateTeamDto dto)
        {
            await using var db = await _contextFactory.CreateDbContextAsync();

            var team = await db.Teams
                .FirstOrDefaultAsync(t => t.Id == dto.Id);

            if (team == null)
                return false;

            var currentTeamMembers = await db.Members
                .Where(m => m.TeamId == dto.Id)
                .ToListAsync();

            var selectedIds = dto.SelectedMemberIds?
                .Distinct()
                .ToList() ?? new List<int>();

            // TeamId zorunlu olduğundan, mevcut bir üyeyi bu ekibin
            // listesinden çıkarmak için önce üyeler sayfasından atamak gerekir.
            if (currentTeamMembers.Any(
                member => !selectedIds.Contains(member.Id)))
            {
                return false;
            }

            var selectedMembers = await db.Members
                .Where(member => selectedIds.Contains(member.Id))
                .ToListAsync();

            if (selectedMembers.Count != selectedIds.Count)
                return false;

            team.Name = dto.Name.Trim();
            team.Description = dto.Description.Trim();

            foreach (var member in selectedMembers)
            {
                member.TeamId = dto.Id;
            }

            await db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await using var db = await _contextFactory.CreateDbContextAsync();

            var team = await db.Teams
                .FirstOrDefaultAsync(t => t.Id == id);

            if (team == null)
                return false;

            // Üyesi olan bir ekip silinemez.
            if (await db.Members.AnyAsync(m => m.TeamId == id))
                return false;

            db.Teams.Remove(team);
            await db.SaveChangesAsync();

            return true;
        }

        private static TeamDto ToDto(Team t) => new TeamDto
        {
            Id = t.Id,
            Name = t.Name,
            Description = t.Description,
            Members = t.Members
                .OrderBy(m => m.FullName)
                .Select(MemberService.ToDto)
                .ToList()
        };
    }
}