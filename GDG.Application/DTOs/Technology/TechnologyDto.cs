namespace GDG.Application.DTOs.Technology
{
    // technology dto's 

    public class TechnologyDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? IconClass { get; set; }
        public string? ColorHex { get; set; }
    }

    public class CreateTechnologyDto
    {
        public string Name { get; set; } = string.Empty;
        public string? IconClass { get; set; } // icons
        public string? ColorHex { get; set; } // colour palette
    }
}
