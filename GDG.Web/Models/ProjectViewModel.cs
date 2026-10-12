namespace GDG.Web.Models;

/// <summary>
/// View model used for presenting project information in ProjectCard components.
/// </summary>
public class ProjectViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = "Kulüp Projesi";
    public string ProjectNumber { get; set; } = "01";
    public string? GitHubUrl { get; set; }
    public string? LiveUrl { get; set; }
    public List<string> Technologies { get; set; } = new();
    public string ThemeColor { get; set; } = "blue"; // "blue", "green", "yellow", "red"
}
