# AI Reference — GDG on Campus Sakarya

Bu dosya, projede çalışacak AI araçları ve geliştiriciler için bağlayıcı proje mimarisi referansıdır.

## 1. Mimari Gerçeklik

Proje **.NET 8** tabanlı **Blazor Web App** mimarisine sahiptir.
Kök dizindeki React / Vite dosyaları (`src/`, `package.json` vb.) sadece Figma Make UI tasarım prototipidir.

- **Ana Çözüm:** `GDGOnCampusSakarya.sln`
- **Ana Çalışma Zamanı:** ASP.NET Core 8 & Blazor Interactive Server
- **Veri Katmanı:** Entity Framework Core + PostgreSQL (Supabase) + ASP.NET Core Identity

## 2. Katmanlar ve Önemli Dosyalar

- `GDG.Domain/Entities/`
  - `Event.cs`, `Member.cs`, `Project.cs`, `Team.cs`, `Technology.cs`
  - `SiteSetting.cs` (Dinamik ayarlar)
  - `ContactMessage.cs` (Ziyaretçi mesajları)
- `GDG.Application/`
  - `Interfaces/` (`IEventService`, `IMemberService`, `IProjectService`, `ITeamService`, `ITechnologyService`, `ISiteSettingsService`, `IContactService`)
  - `DTOs/` (Tüm entity DTO'ları ve validasyonlar)
- `GDG.Infrastructure/`
  - `Contexts/ApplicationDbContext.cs` (IdentityDbContext türevi)
  - `Services/` (EF Core tabanlı veritabanı servis uygulamaları)
  - `Migrations/` (Veritabanı şema versiyonları)
- `GDG.Web/`
  - `Program.cs` (Bağlantı kontrolü, Identity, Cookie auth, endpoint'ler ve servis enjeksiyonları)
  - `Components/Pages/` (Public sayfalar: `Home`, `Events`, `EventDetails`, `Projects`, `Team`, `Contact`, `About`)
  - `Components/Pages/Admin/` (Yönetim paneli: `Dashboard`, `Events`, `Projects`, `Members`, `Teams`, `Technologies`, `Messages`, `Settings`, `Login`)
  - `Components/Layout/` (`MainLayout`, `AdminLayout`, `AdminNavMenu`, `NavMenu`, `Footer`)
- `GDG.Tests/`
  - `BusinessRulesTests.cs` (Birim testler)

## 3. Güvenlik ve Kimlik Doğrulama

- Admin paneline erişim `[Authorize(Roles = "Admin")]` ile korunmaktadır.
- Giriş yapmayan kullanıcılar doğrudan `/admin/login` sayfasına yönlendirilir.
- Giriş işlemi `/api/auth/login` HTTP POST endpoint'i üzerinden HttpOnly cookie oluşturur.
- Çıkış işlemi `/api/auth/logout` HTTP POST endpoint'i ile oturumu güvenli sonlandırır.
- Admin bootstrap için `AdminBootstrap:Email` ve `AdminBootstrap:Password` kullanılır.

## 4. Supabase Bağlantısı

Bağlantı dizesi kaynak koda yazılmaz.
- Geliştirme: `dotnet user-secrets set "ConnectionStrings:DefaultConnection" "<SUPABASE_CONNECTION_STRING>" --project GDG.Web/GDG.Web.csproj`
- Canlı ortam: `ConnectionStrings__DefaultConnection`
