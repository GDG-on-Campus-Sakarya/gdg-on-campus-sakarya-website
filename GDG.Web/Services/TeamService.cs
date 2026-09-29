using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GDG.Application.DTOs.Member;
using GDG.Application.DTOs.Team;
using GDG.Application.Interfaces;

namespace GDG.Web.Services
{
    public class TeamService : ITeamService
    {
        private static readonly List<TeamDto> _teams = new();
        private readonly IMemberService _memberService;

        public TeamService(IMemberService memberService)
        {
            _memberService = memberService;
        }

        public async Task<List<TeamDto>> GetAllAsync()
        {
            var allMembers = await _memberService.GetAllAsync();
            foreach (var team in _teams)
            {
                team.Members = allMembers.Where(m => m.TeamId == team.Id).ToList();
            }
            return _teams.OrderBy(t => t.Name).ToList();
        }

        public async Task<TeamDto?> GetByIdAsync(int id)
        {
            var team = _teams.FirstOrDefault(t => t.Id == id);
            if (team != null)
            {
                var allMembers = await _memberService.GetAllAsync();
                team.Members = allMembers.Where(m => m.TeamId == team.Id).ToList();
            }
            return team;
        }

        public async Task<TeamDto> CreateAsync(CreateTeamDto dto)
        {
            var nextId = _teams.Any() ? _teams.Max(t => t.Id) + 1 : 1;
            var team = new TeamDto
            {
                Id = nextId,
                Name = dto.Name,
                Description = dto.Description
            };
            _teams.Add(team);

            // Assign selected members to this team
            if (dto.SelectedMemberIds.Any())
            {
                var allMembers = await _memberService.GetAllAsync();
                foreach (var memberId in dto.SelectedMemberIds)
                {
                    var member = allMembers.FirstOrDefault(m => m.Id == memberId);
                    if (member != null)
                    {
                        await _memberService.UpdateAsync(new UpdateMemberDto
                        {
                            Id = member.Id,
                            FullName = member.FullName,
                            Role = member.Role,
                            TeamId = nextId,
                            Email = member.Email,
                            GitHubUrl = member.GitHubUrl,
                            LinkedInUrl = member.LinkedInUrl,
                            SelectedProjectIds = member.ProjectIds
                        });
                    }
                }
            }

            return team;
        }

        public async Task<bool> UpdateAsync(UpdateTeamDto dto)
        {
            var existing = _teams.FirstOrDefault(t => t.Id == dto.Id);
            if (existing == null) return false;

            existing.Name = dto.Name;
            existing.Description = dto.Description;

            // Re-assign members
            var allMembers = await _memberService.GetAllAsync();
            foreach (var m in allMembers)
            {
                bool shouldBeInTeam = dto.SelectedMemberIds.Contains(m.Id);
                if (shouldBeInTeam && m.TeamId != dto.Id)
                {
                    await _memberService.UpdateAsync(new UpdateMemberDto
                    {
                        Id = m.Id,
                        FullName = m.FullName,
                        Role = m.Role,
                        TeamId = dto.Id,
                        Email = m.Email,
                        GitHubUrl = m.GitHubUrl,
                        LinkedInUrl = m.LinkedInUrl,
                        SelectedProjectIds = m.ProjectIds
                    });
                }
                else if (!shouldBeInTeam && m.TeamId == dto.Id)
                {
                    await _memberService.UpdateAsync(new UpdateMemberDto
                    {
                        Id = m.Id,
                        FullName = m.FullName,
                        Role = m.Role,
                        TeamId = 0,
                        Email = m.Email,
                        GitHubUrl = m.GitHubUrl,
                        LinkedInUrl = m.LinkedInUrl,
                        SelectedProjectIds = m.ProjectIds
                    });
                }
            }

            return true;
        }

        public Task<bool> DeleteAsync(int id)
        {
            var existing = _teams.FirstOrDefault(t => t.Id == id);
            if (existing != null)
            {
                _teams.Remove(existing);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }
    }
}
