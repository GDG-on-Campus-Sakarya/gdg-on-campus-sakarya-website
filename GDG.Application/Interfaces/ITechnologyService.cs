using System.Collections.Generic;
using System.Threading.Tasks;
using GDG.Application.DTOs.Technology;

namespace GDG.Application.Interfaces
{
    public interface ITechnologyService
    {
        // interfaces 
        // bütün metodlar asenkron + ileride hangfire workers ( sistem tıkanmaması için + zaman alan işlemler )

        Task<List<TechnologyDto>> GetAllAsync();
        Task<TechnologyDto?> GetByIdAsync(int id);
        Task<TechnologyDto> CreateAsync(CreateTechnologyDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
