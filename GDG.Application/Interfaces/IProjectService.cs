using System.Collections.Generic;
using System.Threading.Tasks;
using GDG.Application.DTOs.Project;

namespace GDG.Application.Interfaces
{
    public interface IProjectService
    {
        // interfaces 
        // bütün metodlar asenkron + ileride hangfire workers ( sistem tıkanmaması için )


        Task<List<ProjectDto>> GetAllAsync(int? technologyId = null, bool? featuredOnly = null);
        Task<ProjectDto?> GetByIdAsync(int id);
        Task<ProjectDto> CreateAsync(CreateProjectDto dto);
        Task<bool> UpdateAsync(UpdateProjectDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
