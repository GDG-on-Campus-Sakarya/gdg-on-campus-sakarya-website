using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GDG.Application.DTOs.Technology;

namespace GDG.Application.DTOs.Project
{

    // project

    public class ProjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? ImageUrl { get; set; } // optional
        public string? GitHubUrl { get; set; }
        public string? LiveDemoUrl { get; set; }
        public bool IsFeatured { get; set; }
        public DateTime CreatedAt { get; set; }

        public List<TechnologyDto> Technologies { get; set; } = new();
    }

     // val. + create
    public class CreateProjectDto
    {
        [Required(ErrorMessage = "Proje adı zorunludur.")]
        [StringLength(100, ErrorMessage = "Proje adı en fazla 100 karakter olabilir.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Proje açıklaması zorunludur.")]
        public string Description { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }

        [Url(ErrorMessage = "Geçerli bir GitHub bağlantısı giriniz.")]
        public string? GitHubUrl { get; set; }

        [Url(ErrorMessage = "Geçerli bir canlı demo bağlantısı giriniz.")]
        public string? LiveDemoUrl { get; set; }

        public bool IsFeatured { get; set; } = false;

        public List<int> SelectedTechnologyIds { get; set; } = new();
    }

    // val. + update

    public class UpdateProjectDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Proje adı zorunludur.")]
        [StringLength(100, ErrorMessage = "Proje adı en fazla 100 karakter olabilir.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Proje açıklaması zorunludur.")]
        public string Description { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }

        [Url(ErrorMessage = "Geçerli bir GitHub bağlantısı giriniz.")]
        public string? GitHubUrl { get; set; }

        [Url(ErrorMessage = "Geçerli bir canlı demo bağlantısı giriniz.")]
        public string? LiveDemoUrl { get; set; }

        public bool IsFeatured { get; set; }

        public List<int> SelectedTechnologyIds { get; set; } = new();
    }
}
