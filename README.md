# GDG on Campus Sakarya - Resmi Web Sitesi

Google Developer Groups (GDG) on Campus Sakarya Üniversitesi resmi web sitesi ve yönetim paneli platformudur.

---

## 🚀 Mimari ve Teknolojiler

Bu uygulama **.NET 8** üzerinde **Blazor Web App (Interactive Server)** mimarisiyle ve Clean Architecture prensiplerine uygun olarak geliştirilmiştir.

- **Mimari Katmanları:**
  - `GDG.Domain`: Temel varlıklar (`Event`, `Member`, `Project`, `Team`, `Technology`, `SiteSetting`, `ContactMessage`).
  - `GDG.Application`: Arayüzler (`Interfaces`), servis kontratları ve DTO yapıları.
  - `GDG.Infrastructure`: Entity Framework Core `ApplicationDbContext` (IdentityDbContext türevi), PostgreSQL (Npgsql) eşlemeleri, migrations ve servis implementasyonları.
  - `GDG.Web`: Blazor Web App (Public sayfalar, güvenli Admin paneli, Identity kimlik doğrulama, cookie yapılandırması).
  - `GDG.Tests`: xUnit tabanlı birim testleri.
- **Veritabanı:** Supabase PostgreSQL & ASP.NET Core Identity altyapısı.
- **Tasarım:** Google Workspace / GDG Materyal renk paleti, Scoped CSS ve responsive kart düzeni.

> **ÖNEMLİ BİLGİ (Figma / React Dosyaları):**
> Kök dizindeki `src/`, `package.json`, `vite.config.ts` ve `index.html` dosyaları, Figma Make üzerinden export edilmiş UI prototipidir. Ana çalışan uygulama `GDGOnCampusSakarya.sln` çözümüdür. Node.js veya npm bağımlılıklarını kurmanıza gerek yoktur.

---

## 💻 Kurulum ve Çalıştırma

### Gereksinimler
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Supabase PostgreSQL hesabı ve bağlantı dizesi

### 1. Çözümü Derleme
```bash
dotnet restore
dotnet build GDGOnCampusSakarya.sln
```

### 2. Supabase Bağlantısını Yapılandırma

`GDG.Web/appsettings.json` içerisindeki `ConnectionStrings:DefaultConnection` alanı güvenlik gereği boş bırakılmıştır. Uygulama boş veya eksik bağlantı dizesi ile başlatıldığında açık bir hata fırlatır.

#### Geliştirme Ortamı (User Secrets):
PowerShell veya Bash terminalinde şu komutu çalıştırarak Supabase PostgreSQL URI bilginizi tanımlayın:

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "<SUPABASE_POSTGRESQL_CONNECTION_STRING>" --project GDG.Web/GDG.Web.csproj
```

*Not: `<SUPABASE_POSTGRESQL_CONNECTION_STRING>` yerine Supabase projenizden aldığınız bağlantı dizesini yazınız (örneğin: `Host=aws-0-eu-central-1.pooler.supabase.com;Port=5432;Database=postgres;Username=postgres.xxx;Password=xxx;SSL Mode=Require;Trust Server Certificate=true;`).*

#### Canlı (Production) Ortam:
Canlı ortamda kimlik bilgilerini ortam değişkeni üzerinden verin:
```bash
ConnectionStrings__DefaultConnection="<SUPABASE_POSTGRESQL_CONNECTION_STRING>"
```

### 3. Veritabanı Migration'larını İnceleme ve Uygulama

Mevcut ve yeni eklenen migration'ları veritabanına uygulamak için:

```bash
# Migration listesini kontrol edin
dotnet ef migrations list --project GDG.Infrastructure/GDG.Infrastructure.csproj --startup-project GDG.Web/GDG.Web.csproj

# Veritabanına şemayı uygulayın
dotnet ef database update --project GDG.Infrastructure/GDG.Infrastructure.csproj --startup-project GDG.Web/GDG.Web.csproj
```

---

## 🔒 Yönetici Girişi ve İlk Admin Hesabı (Bootstrap)

Yönetim paneli rotaları (`/admin/*`) yalnızca `Admin` rolüne sahip kullanıcılar tarafından görüntülenebilir. Yetkisiz ziyaretçiler otomatik olarak `/admin/login` sayfasına yönlendirilir.

### İlk Admin Hesabını Oluşturma

Sistemde varsayılan/tahmin edilebilir sabit bir parola veya genel kayıt formu bulunmaz. İlk admin hesabı güvenli bootstrap mekanizmasıyla oluşturulur:

#### Geliştirme Ortamında:
```bash
dotnet user-secrets set "AdminBootstrap:Email" "YOUR_ADMIN_EMAIL" --project GDG.Web/GDG.Web.csproj
dotnet user-secrets set "AdminBootstrap:Password" "YOUR_UNIQUE_STRONG_PASSWORD" --project GDG.Web/GDG.Web.csproj
```

#### Canlı Ortamda:
```bash
AdminBootstrap__Email="YOUR_ADMIN_EMAIL"
AdminBootstrap__Password="YOUR_UNIQUE_STRONG_PASSWORD"
```

Uygulama açılışında belirtilen admin hesabı ve `Admin` rolü otomatik olarak oluşturulur. Yer tutucu değerleri aynen kullanmayın; kendinize ait e-posta ve güçlü, benzersiz bir parola girin. Mevcut bir kullanıcıya ilk kez Admin rolü verilecekse bootstrap parolası o kullanıcının mevcut parolasıyla eşleşmelidir.

---

## 🗺️ Sayfa Haritası

### Public Sayfalar (Veritabanına Bağlı)
- `/` - Ana Sayfa (Yaklaşan etkinlik geri sayımı, öne çıkan projeler, topluluk istatistikleri)
- `/events` - Etkinlikler & Atölyeler (Yaklaşan / Geçmiş filtreleri)
- `/events/{Id:int}` - Etkinlik Detayı (Gerçek veritabanı kaydı)
- `/projects` - Topluluk Projeleri (Teknoloji filtreleri, açık kaynak projeler)
- `/team` - Ekip & Çalışma Grupları (Departman filtreleri, liderler ve üyeler)
- `/contact` - İletişim Formu (Mesajlar doğrudan veritabanına kaydedilir)
- `/about` - Hakkımızda

### Yönetim Paneli (Admin - Yetkilendirme Korumalı)
- `/admin/login` - Güvenli Yönetici Girişi
- `/admin` veya `/admin/dashboard` - Yönetim Özeti & Canlı İstatistikler
- `/admin/events` - Etkinlik Ekleme, Düzenleme, Silme
- `/admin/projects` - Proje Ekleme, Düzenleme, Silme, Teknoloji İlişkileri
- `/admin/members` - Üye Ekleme, Düzenleme, Silme, Ekip Seçimi
- `/admin/teams` - Çalışma Grubu / Ekip Ekleme, Düzenleme, Güvenli Silme
- `/admin/technologies` - Teknoloji CRUD (İkon ve renk yönetimi)
- `/admin/messages` - Gelen İletişim Mesajlarını İnceleme, Okundu İşaretleme ve Silme
- `/admin/settings` - Topluluk Bilgileri ve Sosyal Medya Ayarları Kaydetme

---

## 🧪 Testler

Proje birim testlerini çalıştırmak için:
```bash
dotnet test
```
Business rules ve validasyon testleri hatasız olarak başarıyla geçmektedir.


## Güvenlik notları (iletişim formu ve giriş)

- Yönetici giriş/çıkış uç noktaları ve iletişim formu POST işlemi antiforgery tokenını sunucu tarafında doğrular.
- Yönetici girişi IP başına 5 dakikada 10, iletişim formu IP başına 10 dakikada 5 HTTP isteği ile sınırlandırılır. İletişim formunda ayrıca bir honeypot alanı bulunur.
- Uygulama bir reverse proxy veya yük dengeleyici arkasında yayınlanacaksa rate limit anahtarında gerçek istemci IP'sinin kullanılması için yalnızca hosting sağlayıcısının bilinen proxy adreslerini güvenilir kabul eden forwarded-headers yapılandırması eklenmelidir. Gelen `X-Forwarded-For` değerlerine gelişigüzel güvenilmemelidir.
- Resmi iletişim e-postası teyit edilene kadar varsayılan e-posta alanı boş bırakılır. Harici bağlantılar yalnızca HTTP/HTTPS URL'leri olarak kabul edilir.
