using GDG.Web.Models;

namespace GDG.Web.Services;

// TODO(backend): Remove MockDataService when real backend APIs/repositories are connected.
public static class MockDataService
{
    public static EventViewModel GetUpcomingEvent()
    {
        return new EventViewModel
        {
            Id = 1,
            Title = "Google I/O Extended Sakarya",
            Description = "Google I/O yeniliklerini, AI ve Web geliştirmedeki en son duyuruları Sakarya Üniversitesi'nde birlikte inceliyoruz!",
            Date = DateTime.Now.AddDays(14).AddHours(3),
            FormattedDate = "09 MAYIS 2026",
            Time = "13:00",
            Location = "SAÜ Kültür ve Kongre Merkezi",
            CategoryTag = "YAKLAŞAN ETKİNLİK",
            CategoryColor = "blue",
            ImageUrl = null
        };
    }

    public static List<EventViewModel> GetEvents()
    {
        return new List<EventViewModel>
        {
            new EventViewModel
            {
                Id = 1,
                Title = "Google I/O Extended Sakarya",
                Description = "Google I/O yeniliklerini ve yapay zeka duyurularını birlikte inceliyoruz.",
                Date = DateTime.Now.AddDays(14),
                FormattedDate = "09 MAYIS 2026",
                Time = "13:00",
                Location = "SAÜ Kültür ve Kongre Merkezi",
                CategoryTag = "DEVELOPER CONFERENCE",
                CategoryColor = "blue"
            },
            new EventViewModel
            {
                Id = 2,
                Title = "Build with AI Workshop",
                Description = "Gemini API ve Google Cloud araçları ile eller serbest uygulama geliştirme atölyesi.",
                Date = DateTime.Now.AddDays(28),
                FormattedDate = "23 MAYIS 2026",
                Time = "15:30",
                Location = "Teknoloji Fakültesi",
                CategoryTag = "AI & MACHINE LEARNING",
                CategoryColor = "red"
            },
            new EventViewModel
            {
                Id = 3,
                Title = "Kariyer Sohbetleri: Google",
                Description = "Sektörden uzmanlarla yazılım kariyeri ve açık kaynak dünyası söyleşisi.",
                Date = DateTime.Now.AddDays(42),
                FormattedDate = "06 HAZİRAN 2026",
                Time = "14:00",
                Location = "Turgut Özal Kültür Merkezi",
                CategoryTag = "CAREER & NETWORKING",
                CategoryColor = "yellow"
            }
        };
    }

    public static List<ProjectViewModel> GetProjects()
    {
        return new List<ProjectViewModel>
        {
            new ProjectViewModel
            {
                Id = 1,
                Name = "Campus Connect",
                Description = "Kampüs içi öğrenci etkinliklerini ve kulüp duyurularını tek platformda toplayan mobil uyumlu web uygulaması.",
                Category = "KAMPÜS TEKNOLOJİSİ",
                ProjectNumber = "01",
                ThemeColor = "blue",
                GitHubUrl = "https://github.com",
                Technologies = new List<string> { "C#", "ASP.NET Core", "Blazor", "PostgreSQL" }
            },
            new ProjectViewModel
            {
                Id = 2,
                Name = "Sakarya Açık Veri",
                Description = "Sakarya ulaşım, etkinlik ve akıllı şehir verilerini görselleştiren açık kaynak veri platformu.",
                Category = "AKILLI ŞEHİR",
                ProjectNumber = "02",
                ThemeColor = "green",
                GitHubUrl = "https://github.com",
                Technologies = new List<string> { "Python", "FastAPI", "React", "PostGIS" }
            }
        };
    }

    public static List<MemberViewModel> GetMembers()
    {
        return new List<MemberViewModel>
        {
            new MemberViewModel
            {
                Id = 1,
                FullName = "Arda Timuçin",
                Role = "Kulüp Başkanı",
                TeamName = "Yönetim",
                CardColor = "blue",
                GitHubUrl = "https://github.com",
                LinkedInUrl = "https://linkedin.com"
            },
            new MemberViewModel
            {
                Id = 2,
                FullName = "Neslihan",
                Role = "Core Team",
                TeamName = "Organizasyon",
                CardColor = "orange",
                GitHubUrl = "https://github.com",
                LinkedInUrl = "https://linkedin.com"
            },
            new MemberViewModel
            {
                Id = 3,
                FullName = "Eda",
                Role = "Sosyal Medya & İçerik",
                TeamName = "Medya",
                CardColor = "yellow",
                GitHubUrl = "https://github.com",
                LinkedInUrl = "https://linkedin.com"
            },
            new MemberViewModel
            {
                Id = 4,
                FullName = "Enes",
                Role = "Full Stack Developer",
                TeamName = "Yazılım",
                CardColor = "green",
                GitHubUrl = "https://github.com",
                LinkedInUrl = "https://linkedin.com"
            }
        };
    }
}
