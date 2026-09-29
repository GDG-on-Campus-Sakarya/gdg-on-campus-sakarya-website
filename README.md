# GDG on Campus Sakarya - Resmi Web Sitesi

Google Developer Groups (GDG) on Campus Sakarya Üniversitesi resmi web sitesi ve yönetim paneli projesidir.

---

## 🚀 Teknoloji ve Mimari

Bu proje **.NET 8** üzerinde **Blazor Web App** mimarisi kullanılarak geliştirilmiştir.

- **Frontend & UI:** Blazor (Interactive Server / SSR), Scoped CSS, Vanilla CSS (`app.css`), Google Materyal / Tasarım Sistemi paleti.
- **Backend & Mimari:** Clean Architecture katmanları:
  - `GDG.Domain`: Varlıklar (Entities) ve temel iş kuralları.
  - `GDG.Application`: Arayüzler (Interfaces), servis tanımları, DTO'lar.
  - `GDG.Infrastructure`: Entity Framework Core `AppDbContext`, veritabanı eşlemeleri ve PostgreSQL entegrasyonu.
  - `GDG.Web`: Blazor Web App arayüzü, public sayfalar, admin paneli ve servis kayıtları.

> **ÖNEMLİ NOT (Figma / React Dosyaları Hakkında):**
> Kök dizinde yer alan `src/`, `package.json`, `vite.config.ts` ve `index.html` dosyaları, Figma Make üzerinden export edilen UI/UX tasarım prototipi ve referans handoff kodlarıdır.
> Blazor uygulaması bu dosyalara veya Node.js/npm bağımlılıklarına **ihtiyaç duymaz**. Uygulamayı çalıştırmak için `npm install` veya `npm run dev` yapmanıza **gerek yoktur**.

---

## 💻 Kurulum ve Çalıştırma

### Gereksinimler
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- (Opsiyonel) Visual Studio 2022 (v17.8+), JetBrains Rider veya VS Code (C# Dev Kit eklentisi ile)

### 1. Terminal / CLI ile Çalıştırma
```bash
# Bağımlılıkları geri yükleyin ve derleyin
dotnet build

# GDG.Web projesini ayağa kaldırın
dotnet run --project GDG.Web
```

### 2. IDE (Visual Studio / Rider) ile Çalıştırma
1. `GDGOnCampusSakarya.sln` çözüm dosyasını açın.
2. Çözüm Gezgini'nde (Solution Explorer) `GDG.Web` projesine sağ tıklayıp **"Set as Startup Project"** (Başlangıç Projesi Olarak Ayarla) seçeneğini seçin.
3. `Ctrl + F5` (Hata ayıklama olmadan başlat) veya `F5` ile projeyi çalıştırın.

Tarayıcınızda otomatik olarak açılacaktır (Varsayılan olarak `https://localhost:7198` veya `http://localhost:5246`).

---

## 🗺️ Sayfa ve Rota Haritası

### Public Sayfalar
- `/` - Ana Sayfa
- `/about` - Hakkımızda
- `/events` - Etkinlikler
- `/team` - Ekip & Organizasyon
- `/projects` - Projeler & Açık Kaynak
- `/contact` - İletişim

### Yönetim Paneli (Admin)
- `/admin` veya `/admin/dashboard` - Yönetim Özeti & İstatistikler
- `/admin/events` - Etkinlik Yönetimi
- `/admin/members` - Üye Yönetimi
- `/admin/teams` - Takım Yönetimi
- `/admin/projects` - Proje Yönetimi
- `/admin/technologies` - Teknoloji Yönetimi
- `/admin/settings` - Panel Ayarları

---

## 🔒 Güvenlik ve Geliştirme Notları

- **Bağlantı Dizgisi (Connection String):** `GDG.Web/appsettings.json` içindeki varsayılan veritabanı şifresi yerel geliştirme içindir. Canlı ortamlarda veya bireysel geliştirmede şifrelerinizi `appsettings.Development.json` veya `dotnet user-secrets` ile yönetin, hassas kimlik bilgilerini git reposuna commit etmeyin.
- **Servis Durumu:** Veritabanı ve CRUD görevleri ekibe dağıtılana kadar public sayfalar ve admin paneli in-memory / mock servislerle kesintisiz çalışmaktadır.
