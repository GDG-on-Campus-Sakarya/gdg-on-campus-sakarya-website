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
    /// <summary>
    /// ITeamService'in veritabanına bağlı gerçek uygulaması.
    ///
    /// ⚠ EKİPLE KARARLAŞTIRILMASI GEREKEN İKİ KONU (aşağıda TODO olarak işaretli):
    ///   1) Ekipten çıkarılan üye ne olacak? (TeamId zorunlu, 0 yapılamaz)
    ///   2) Üyesi olan ekip silinebilir mi? (Veritabanı ayarı: ekip silinirse ÜYELERİ DE SİLİNİR)
    /// </summary>
    public class TeamService : ITeamService
    {
        private readonly ApplicationDbContext _db;

        public TeamService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<TeamDto>> GetAllAsync()
        {
            var teams = await _db.Teams
                .AsNoTracking()
                .Include(t => t.Members)              // ekibin üyeleri
                    .ThenInclude(m => m.Projects)     // ve o üyelerin projeleri (MemberDto'da lazım)
                .OrderBy(t => t.Name)
                .ToListAsync();

            return teams.Select(ToDto).ToList();
        }

        public async Task<TeamDto?> GetByIdAsync(int id)
        {
            var team = await _db.Teams
                .AsNoTracking()
                .Include(t => t.Members)
                    .ThenInclude(m => m.Projects)
                .FirstOrDefaultAsync(t => t.Id == id);

            return team == null ? null : ToDto(team);
        }

        public async Task<TeamDto> CreateAsync(CreateTeamDto dto)
        {
            var team = new Team
            {
                Name = dto.Name.Trim(),
                Description = dto.Description.Trim()
            };

            _db.Teams.Add(team);
            await _db.SaveChangesAsync();   // önce kaydet ki team.Id oluşsun

            // Formda seçilen üyeleri bu ekibe taşı.
            if (dto.SelectedMemberIds.Count > 0)
            {
                var members = await _db.Members
                    .Where(m => dto.SelectedMemberIds.Contains(m.Id))
                    .ToListAsync();

                foreach (var member in members)
                    member.TeamId = team.Id;

                await _db.SaveChangesAsync();
            }

            return (await GetByIdAsync(team.Id))!;
        }

        public async Task<bool> UpdateAsync(UpdateTeamDto dto)
        {
            var team = await _db.Teams.FirstOrDefaultAsync(t => t.Id == dto.Id);
            if (team == null) return false;

            team.Name = dto.Name.Trim();
            team.Description = dto.Description.Trim();

            // Seçilen ama henüz bu ekipte olmayan üyeleri bu ekibe taşı.
            var membersToAdd = await _db.Members
                .Where(m => dto.SelectedMemberIds.Contains(m.Id) && m.TeamId != dto.Id)
                .ToListAsync();
            foreach (var member in membersToAdd)
                member.TeamId = dto.Id;

            // TODO(ekip kararı): Seçimden ÇIKARILAN üyeler.
            // Mock servis bunların TeamId'sini 0 yapıyordu. Gerçek veritabanında bu yapılamaz:
            // TeamId zorunlu bir foreign key ve 0 numaralı ekip yok, kayıt hata verir.
            // Seçenekler: (a) TeamId'yi nullable yapmak (Canan, migration), (b) "Atanmamış" diye bir
            // ekip oluşturup oraya taşımak, (c) çıkarmaya izin vermemek. Karar verilene kadar
            // çıkarılan üyelere dokunmuyoruz: ekipte kalmaya devam ediyorlar.

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var team = await _db.Teams.FirstOrDefaultAsync(t => t.Id == id);
            if (team == null) return false;

            // TODO(ekip kararı): Veritabanında Team → Member ilişkisi "Cascade" ayarlı.
            // Yani bir ekip silinirse İÇİNDEKİ BÜTÜN ÜYELER DE SİLİNİR. Bu büyük ihtimalle istenen
            // davranış değil. Karar verilene kadar güvenli tarafta kalıyoruz: üyesi olan ekip silinmez.
            if (await _db.Members.AnyAsync(m => m.TeamId == id))
                return false;

            _db.Teams.Remove(team);
            await _db.SaveChangesAsync();
            return true;
        }

        private static TeamDto ToDto(Team t) => new TeamDto
        {
            Id = t.Id,
            Name = t.Name,
            Description = t.Description,
            Members = t.Members
                .OrderBy(m => m.FullName)
                .Select(MemberService.ToDto)   // aynı dönüşümü tekrar yazmamak için
                .ToList()
        };
    }
}
