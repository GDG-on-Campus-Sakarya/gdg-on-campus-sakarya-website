using System;
using System.ComponentModel.DataAnnotations;

namespace GDG.Domain.Entities
{
    public class SiteSetting
    {
        [Key]
        public string Key { get; set; } = string.Empty;

        public string Value { get; set; } = string.Empty;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
