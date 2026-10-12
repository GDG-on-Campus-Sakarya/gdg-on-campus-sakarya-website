using System.Collections.Generic;
using System.Threading.Tasks;
using GDG.Application.DTOs.Team;

namespace GDG.Application.Interfaces
{
    public interface ITeamService
    {
        Task<List<TeamDto>> GetAllAsync();
        Task<TeamDto?> GetByIdAsync(int id);
        Task<TeamDto> CreateAsync(CreateTeamDto dto);
        Task<bool> UpdateAsync(UpdateTeamDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
