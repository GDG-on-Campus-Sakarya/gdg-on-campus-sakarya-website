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
    /// <summary>
    /// IMemberService'in veritabanına bağlı gerçek uygulaması.
    ///
    /// Event'ten farkı: Member'ın ilişkileri var.
    ///   - Her üye bir ekibe bağlı (TeamId zorunlu, foreign key).
    ///   - Üye ile proje arasında çoka-çok ilişki var (MemberProject ara tablosu).
    /// </summary>
    public class MemberService : IMemberService
    {
        private readonly ApplicationDbContext _db;

        public MemberService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<MemberDto>> GetAllAsync()
        {
            var members = await _db.Members
                .AsNoTracking()
                .Include(m => m.Projects)   // projeleri de getir (JOIN). Yoksa m.Projects boş gelir.
                .OrderBy(m => m.FullName)
                .ToListAsync();

            return members.Select(ToDto).ToList();
        }

        public async Task<MemberDto?> GetByIdAsync(int id)
        {
            var member = await _db.Members
                .AsNoTracking()
                .Include(m => m.Projects)
                .FirstOrDefaultAsync(m => m.Id == id);

            return member == null ? null : ToDto(member);
        }

        public async Task<MemberDto> CreateAsync(CreateMemberDto dto)
        {
            // TeamId zorunlu bir foreign key. Olmayan bir ekibe üye eklemeye çalışırsak veritabanı
            // anlaşılmaz bir hata verir; biz önce kontrol edip anlaşılır bir mesaj veriyoruz.
            await EnsureTeamExistsAsync(dto.TeamId);

            var member = new Member
            {
                FullName = dto.FullName.Trim(),
                Role = dto.Role.Trim(),
                TeamId = dto.TeamId,
                Email = NullIfEmpty(dto.Email),
                GitHubUrl = NullIfEmpty(dto.GitHubUrl),
                LinkedInUrl = NullIfEmpty(dto.LinkedInUrl),
                Projects = await LoadProjectsAsync(dto.SelectedProjectIds)
            };

            _db.Members.Add(member);
            await _db.SaveChangesAsync();   // hem Members'a hem MemberProject ara tablosuna yazar

            return ToDto(member);
        }

        public async Task<bool> UpdateAsync(UpdateMemberDto dto)
        {
            // Projeleri de güncelleyeceğimiz için Include şart: EF mevcut bağlantıları bilmeli ki
            // hangilerinin silinip hangilerinin ekleneceğini hesaplayabilsin.
            var member = await _db.Members
                .Include(m => m.Projects)
                .FirstOrDefaultAsync(m => m.Id == dto.Id);
            if (member == null) return false;

            await EnsureTeamExistsAsync(dto.TeamId);

            member.FullName = dto.FullName.Trim();
            member.Role = dto.Role.Trim();
            member.TeamId = dto.TeamId;
            member.Email = NullIfEmpty(dto.Email);
            member.GitHubUrl = NullIfEmpty(dto.GitHubUrl);
            member.LinkedInUrl = NullIfEmpty(dto.LinkedInUrl);

            // Proje listesini baştan kur. EF ara tablodaki farkı kendisi hesaplar:
            // listeden çıkan projeler için DELETE, yeni eklenenler için INSERT üretir.
            member.Projects.Clear();
            member.Projects.AddRange(await LoadProjectsAsync(dto.SelectedProjectIds));

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var member = await _db.Members.FirstOrDefaultAsync(m => m.Id == id);
            if (member == null) return false;

            _db.Members.Remove(member);   // ara tablodaki proje bağlantıları da otomatik silinir
            await _db.SaveChangesAsync();
            return true;
        }

        // ───────── yardımcılar ─────────

        private async Task EnsureTeamExistsAsync(int teamId)
        {
            if (!await _db.Teams.AnyAsync(t => t.Id == teamId))
                throw new InvalidOperationException(
                    $"{teamId} numaralı ekip bulunamadı. Üye eklemeden önce ekibi oluşturun.");
        }

        // Formda seçilen proje Id'lerine karşılık gelen projeleri veritabanından getirir.
        // SQL: SELECT * FROM "Projects" WHERE "Id" IN (1, 3, 5)
        private async Task<List<Project>> LoadProjectsAsync(List<int> projectIds)
        {
            if (projectIds.Count == 0) return new List<Project>();
            return await _db.Projects.Where(p => projectIds.Contains(p.Id)).ToListAsync();
        }

        internal static MemberDto ToDto(Member m) => new MemberDto
        {
            Id = m.Id,
            FullName = m.FullName,
            Role = m.Role,
            TeamId = m.TeamId,
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
