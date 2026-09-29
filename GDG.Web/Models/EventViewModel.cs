namespace GDG.Web.Models;

/// <summary>
/// View model used for presenting event information in EventCard components.
/// </summary>
public class EventViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string FormattedDate { get; set; } = string.Empty;
    public string Time { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string CategoryTag { get; set; } = "Etkinlik";
    public string CategoryColor { get; set; } = "blue"; // "blue", "red", "yellow", "green"
    public string? ImageUrl { get; set; }
    public string? CalendarUrl { get; set; }
}
