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
        private readonly IDbContextFactory<ApplicationDbContext>
            _contextFactory;

        public TeamService(
            IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<List<TeamDto>> GetAllAsync()
        {
            await using var db =
                await _contextFactory.CreateDbContextAsync();

            var teams = await db.Teams
                .AsNoTracking()
                .Include(t => t.Members)
                    .ThenInclude(m => m.Projects)
                .Include(t => t.AdditionalMembers)
                    .ThenInclude(m => m.Projects)
                .OrderBy(t => t.Name)
                .ToListAsync();

            return teams.Select(ToDto).ToList();
        }

        public async Task<TeamDto?> GetByIdAsync(int id)
        {
            await using var db =
                await _contextFactory.CreateDbContextAsync();

            var team = await db.Teams
                .AsNoTracking()
                .Include(t => t.Members)
                    .ThenInclude(m => m.Projects)
                .Include(t => t.AdditionalMembers)
                    .ThenInclude(m => m.Projects)
                .FirstOrDefaultAsync(t => t.Id == id);

            return team == null ? null : ToDto(team);
        }

        public async Task<TeamDto> CreateAsync(CreateTeamDto dto)
        {
            await using var db =
                await _contextFactory.CreateDbContextAsync();

            if (dto.LeaderMemberId.HasValue &&
                !await db.Members.AnyAsync(m => m.Id == dto.LeaderMemberId.Value))
            {
                throw new InvalidOperationException("Seçilen ekip lideri bulunamadı.");
            }

            var team = new Team
            {
                Name = dto.Name.Trim(),
                Description = dto.Description.Trim(),
                LeaderMemberId = dto.LeaderMemberId
            };

            db.Teams.Add(team);

            await db.SaveChangesAsync();

            var selectedIds = dto.SelectedMemberIds
                .Distinct()
                .ToList();

            // Lider ayrıca işaretlenmemiş olsa bile o ekibin üyesi olmalıdır.
            if (dto.LeaderMemberId.HasValue && !selectedIds.Contains(dto.LeaderMemberId.Value))
            {
                selectedIds.Add(dto.LeaderMemberId.Value);
            }

            if (selectedIds.Count > 0)
            {
                var selectedMembers = await db.Members
                    .Where(m => selectedIds.Contains(m.Id))
                    .ToListAsync();

                if (selectedMembers.Count != selectedIds.Count)
                {
                    throw new InvalidOperationException("Seçilen ekip üyelerinden biri bulunamadı.");
                }

                foreach (var member in selectedMembers)
                {
                    // Üyenin ana ekibini değiştirmiyoruz.
                    if (member.TeamId != team.Id &&
                        !team.AdditionalMembers.Any(existing => existing.Id == member.Id))
                    {
                        team.AdditionalMembers.Add(member);
                    }
                }

                await db.SaveChangesAsync();
            }

            return (await GetByIdAsync(team.Id))!;
        }

        public async Task<bool> UpdateAsync(UpdateTeamDto dto)
        {
            await using var db =
                await _contextFactory.CreateDbContextAsync();

            var team = await db.Teams
                .Include(t => t.AdditionalMembers)
                .FirstOrDefaultAsync(t => t.Id == dto.Id);

            if (team == null)
                return false;

            var selectedIds = dto.SelectedMemberIds?
                .Distinct()
                .ToList() ?? new List<int>();

            var selectedMembers = await db.Members
                .Where(m => selectedIds.Contains(m.Id))
                .ToListAsync();

            // Gönderilen ID'ler gerçekten mevcut üyelere ait olmalı.
            if (selectedMembers.Count != selectedIds.Count)
                return false;

            if (dto.LeaderMemberId.HasValue &&
                !await db.Members.AnyAsync(m => m.Id == dto.LeaderMemberId.Value))
            {
                return false;
            }

            team.Name = dto.Name.Trim();
            team.Description = dto.Description.Trim();
            team.LeaderMemberId = dto.LeaderMemberId;

            // Lider seçildiyse, bu kişinin ilgili ekibin de üyesi olmasını sağla.
            var leaderMember = dto.LeaderMemberId.HasValue
                ? await db.Members.FirstOrDefaultAsync(m => m.Id == dto.LeaderMemberId.Value)
                : null;

            // Formdan kaldırılmış ek üyelikleri temizle.
            foreach (var member in team.AdditionalMembers.ToList())
            {
                if (!selectedIds.Contains(member.Id)
                    || member.TeamId == team.Id)
                {
                    team.AdditionalMembers.Remove(member);
                }
            }

            // Yeni seçilmiş üyeleri ekle.
            foreach (var member in selectedMembers)
            {
                // Üye bu ekibin zaten ana üyesiyse
                // ikinci kez ilişki oluşturma.
                if (member.TeamId == team.Id)
                    continue;

                var alreadyAdded = team.AdditionalMembers
                    .Any(m => m.Id == member.Id);

                if (!alreadyAdded)
                {
                    team.AdditionalMembers.Add(member);
                }
            }

            if (leaderMember != null && leaderMember.TeamId != team.Id &&
                !team.AdditionalMembers.Any(m => m.Id == leaderMember.Id))
            {
                team.AdditionalMembers.Add(leaderMember);
            }

            await db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await using var db =
                await _contextFactory.CreateDbContextAsync();

            var team = await db.Teams
                .Include(t => t.AdditionalMembers)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (team == null)
                return false;

            // Ana üyesi veya ek üyeliği olan ekip silinemez.
            var hasPrimaryMembers = await db.Members
                .AnyAsync(m => m.TeamId == id);

            if (hasPrimaryMembers || team.AdditionalMembers.Any())
                return false;

            db.Teams.Remove(team);

            await db.SaveChangesAsync();

            return true;
        }

        private static TeamDto ToDto(Team team)
        {
            // Ana üyeler ve ek üyeler birleştirilir.
            // Aynı kişi iki ilişkide de varsa bir kez sayılır.
            var members = team.Members
                .Concat(team.AdditionalMembers)
                .GroupBy(m => m.Id)
                .Select(group => group.First())
                .OrderByDescending(m => team.LeaderMemberId.HasValue && m.Id == team.LeaderMemberId.Value)
                .ThenBy(m => m.FullName, StringComparer.CurrentCultureIgnoreCase)
                .Select(MemberService.ToDto)
                .ToList();

            return new TeamDto
            {
                Id = team.Id,
                Name = team.Name,
                Description = team.Description,
                LeaderMemberId = team.LeaderMemberId,
                Members = members
            };
        }
    }
}