using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GDG.Application.DTOs.Member;
using GDG.Application.Interfaces;


// üye servisi

namespace GDG.Web.Services
{
    public class MemberService : IMemberService
    {
        private static readonly List<MemberDto> _members = new();

        public Task<List<MemberDto>> GetAllAsync()
        {
            return Task.FromResult(_members.OrderBy(m => m.FullName).ToList());
        }

        public Task<MemberDto?> GetByIdAsync(int id)
        {
            var member = _members.FirstOrDefault(m => m.Id == id);
            return Task.FromResult(member);
        }

        public Task<MemberDto> CreateAsync(CreateMemberDto dto)
        {
            var nextId = _members.Any() ? _members.Max(m => m.Id) + 1 : 1;
            var member = new MemberDto
            {
                Id = nextId,
                FullName = dto.FullName,
                Role = dto.Role,
                TeamId = dto.TeamId,
                Email = dto.Email,
                GitHubUrl = dto.GitHubUrl,
                LinkedInUrl = dto.LinkedInUrl,
                ProjectIds = new List<int>(dto.SelectedProjectIds)
            };
            _members.Add(member);
            return Task.FromResult(member);
        }

        public Task<bool> UpdateAsync(UpdateMemberDto dto)
        {
            var existing = _members.FirstOrDefault(m => m.Id == dto.Id);
            if (existing == null) return Task.FromResult(false);

            existing.FullName = dto.FullName;
            existing.Role = dto.Role;
            existing.TeamId = dto.TeamId;
            existing.Email = dto.Email;
            existing.GitHubUrl = dto.GitHubUrl;
            existing.LinkedInUrl = dto.LinkedInUrl;
            existing.ProjectIds = new List<int>(dto.SelectedProjectIds);

            return Task.FromResult(true);
        }

        public Task<bool> DeleteAsync(int id)
        {
            var existing = _members.FirstOrDefault(m => m.Id == id);
            if (existing != null)
            {
                _members.Remove(existing);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }
    }
}
