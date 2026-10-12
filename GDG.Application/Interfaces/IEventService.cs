using System.Collections.Generic;
using System.Threading.Tasks;
using GDG.Application.DTOs.Event;

namespace GDG.Application.Interfaces
{
    public interface IEventService
    {
        Task<List<EventDto>> GetAllAsync();
        Task<EventDto?> GetByIdAsync(int id);
        Task<EventDto> CreateAsync(CreateEventDto dto);
        Task<bool> UpdateAsync(UpdateEventDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
