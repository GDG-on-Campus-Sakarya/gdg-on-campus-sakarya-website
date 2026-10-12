namespace GDG.Web.Models;

/// <summary>
/// View model used for presenting team member information in MemberCard components.
/// </summary>
public class MemberViewModel
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string? TeamName { get; set; }
    public string? AvatarUrl { get; set; }
    public string? GitHubUrl { get; set; }
    public string? LinkedInUrl { get; set; }
    public string CardColor { get; set; } = "blue"; // "blue", "orange", "yellow", "green"
}
