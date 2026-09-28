using System.Collections.Generic;
using System.Threading.Tasks;
using GDG.Application.DTOs.Member;

namespace GDG.Application.Interfaces
{
    public interface IMemberService
    {
        Task<List<MemberDto>> GetAllAsync();
        Task<MemberDto?> GetByIdAsync(int id);
        Task<MemberDto> CreateAsync(CreateMemberDto dto);
        Task<bool> UpdateAsync(UpdateMemberDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
