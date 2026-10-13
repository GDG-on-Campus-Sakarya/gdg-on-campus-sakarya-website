# AGENTS.md — GDG on Campus Sakarya

Bu dosya, projede çalışacak yapay zeka asistanları ve geliştiriciler için temel mimari rehberdir.

## ⚠️ ÖNEMLİ MİMARİ BİLGİ

Bu projenin **ASIL VE ÇALIŞAN UYGULAMASI** bir **.NET 8 Blazor Web App** (Interactive Server) uygulamasıdır (`GDGOnCampusSakarya.sln`).

Kök dizinde yer alan `src/`, `package.json`, `vite.config.ts`, `index.html` dosyaları ve Figma Make yapılandırması **YALNIZCA GÖRSEL TASARIM REFERANSI VE PROTOTİPTİR**.

- ❌ Projeyi React veya Vite uygulamasına dönüştürmeyin.
- ❌ Blazor bileşenlerini silmeyin veya Node.js çalışma zamanını ana uygulama olarak çalıştırmayın.
- ❌ Kök dizindeki React dosyalarında backend mantığı aramayın.
- ✅ Tüm iş mantığı, veritabanı erişimi, kimlik doğrulama ve sayfalar `GDG.*` .NET projelerinde yer alır.

---

## Proje Yapısı (.NET Clean Architecture)

- `GDGOnCampusSakarya.sln` - Ana çözüm dosyası
- `GDG.Domain` - Temel entity'ler (`Event`, `Member`, `Project`, `Team`, `Technology`, `SiteSetting`, `ContactMessage`)
- `GDG.Application` - Servis arayüzleri (`IEventService`, `IMemberService`, `IProjectService`, `ITeamService`, `ITechnologyService`, `ISiteSettingsService`, `IContactService`) ve DTO sınıfları
- `GDG.Infrastructure` - Entity Framework Core `ApplicationDbContext` (IdentityDbContext), PostgreSQL / Npgsql eşlemeleri, migrations ve servis implementasyonları
- `GDG.Web` - ASP.NET Core 8 Blazor Web App, public sayfalar (`/`, `/events`, `/projects`, `/team`, `/contact`), admin paneli (`/admin/*`) ve kimlik doğrulama altyapısı
- `GDG.Tests` - Kritik iş kuralları ve DTO doğrulamaları için xUnit test projesi

---

## Veritabanı ve Supabase

- Veritabanı: **Supabase PostgreSQL**
- Bağlantı dizesi kaynak kodda veya `appsettings.json` içinde tutulmaz.
- Geliştirme ortamında: `dotnet user-secrets set "ConnectionStrings:DefaultConnection" "<SUPABASE_CONNECTION_STRING>" --project GDG.Web/GDG.Web.csproj`
- Canlı ortamda: `ConnectionStrings__DefaultConnection` ortam değişkeni.

---

## Temel Kurallar

1. Mevcut Google Workspace / GDG tasarım dilini, renklerini ve bileşen yapılarını koruyun.
2. Admin rotaları (`/admin/*`) ASP.NET Core Identity ve cookie tabanlı yetkilendirme ile korunur (Yalnızca `Admin` rolü).
3. İlk admin hesabı `AdminBootstrap:Email` ve `AdminBootstrap:Password` yapılandırması ile güvenli biçimde ayağa kaldırılır.
4. Kod değişikliklerinden sonra daima `dotnet build GDGOnCampusSakarya.sln` ve `dotnet test` ile derleme ve testleri doğrulayın.
