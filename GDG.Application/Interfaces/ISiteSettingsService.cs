using System.Threading.Tasks;

namespace GDG.Application.Interfaces
{
    public interface ISiteSettingsService
    {
        Task<string> GetSettingAsync(string key, string defaultValue = "");
        Task SetSettingAsync(string key, string value);
        Task<SiteSettingsDto> GetAllSettingsAsync();
        Task SaveAllSettingsAsync(SiteSettingsDto dto);
    }

    public class SiteSettingsDto
    {
        public string CommunityName { get; set; } = "GDG on Campus Sakarya";
        public string UniversityName { get; set; } = "Sakarya Üniversitesi";
        public string CampusLocation { get; set; } = "Esentepe Kampüsü, Serdivan / Sakarya";
        public string ContactEmail { get; set; } = string.Empty; // Only set after the official address is confirmed.
        public string GitHubUrl { get; set; } = "https://github.com/GDG-on-Campus-Sakarya";
        public string CommunityDevUrl { get; set; } = "https://gdg.community.dev/gdg-on-campus-sakarya-university-sakarya-turkey/";
        public string LinkedInUrl { get; set; } = "https://www.linkedin.com/company/gdgsau/";
        public string InstagramUrl { get; set; } = "https://www.instagram.com/gdgocsau";
    }
}
