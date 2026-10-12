# GDG on Campus Sakarya — UI/UX Handoff & Tasarım Teslim Kılavuzu

**Tasarımcı:** Melek (UI/UX Designer)  
**Görev:** 6. Melek — UI/UX Kontrolü ve İyileştirme  
**Hedef Kitle:** Frontend Geliştiricileri (Ecrin & Web Ekibi)  
**Canlı Figma Projesi:** [Figma Make — gdg_web_design](https://www.figma.com/make/W5DjiAxUUrhzEo5fBiNqG7/gdg_web_design?t=IOPnDdJ2Jq0B4gaM-1&preview-route=%2Findex.html)

---

## 1. Genel Bakış ve Amaç

Bu doküman, Figma'da hazırlanan ve revize edilen arayüz standartlarının frontend ekibi tarafından `GDG.Web` (Blazor/CSS) ve web sayfalarına birebir, pürüzsüz ve çakışmasız şekilde aktarılması için hazırlanmış teknik tasarım teslim (handoff) rehberidir.

Tüm tasarımlar `src/styles/tokens.css` dosyasındaki CSS değişkenleriyle (design tokens) tam uyumludur. Sabit HEX renkler yerine bu değişkenler kullanılmalıdır.

---

## 2. Görev Maddeleri ve Frontend Uygulama Kuralları

### Madde 1: Ekip ve Projeler Sayfalarında Filtre / Kategori Alanları
**Tasarım Hedefi:** Masaüstünde tam ortalanmış, mobilde ise sol kenardan kırpılmadan pürüzsüz kaydırılabilen tutarlı kategori butonları.

- **Kapsayıcı (`.filter-section` / `.tabs-bar`):**
  - Masaüstü: `display: flex; justify-content: center;` (veya `.tabs-group` için `margin-inline: auto;`)
  - Mobil (< 768px): `overflow-x: auto; -webkit-overflow-scrolling: touch; scrollbar-width: none;`
  - Mobilde taşmayı önleme: İçerik flex-start ile başlayıp `margin-inline: auto` ile ortalandığında, küçük ekranda ilk sekme sola yapışık kalarak kaydırmaya izin verir ve veri kaybı (data-loss overflow) yaşanmaz.
- **Sekme Grubu (`.tabs-group`):**
  - Arka plan: `var(--surface)` (#FFFFFF)
  - Sınır: `1px solid var(--outline)` (#DFE1E5)
  - Radius: `var(--radius-pill)` (999px)
  - Padding: `4px`
- **Sekme Butonu (`.filter-tab`):**
  - Padding: `8px 20px` (0.5rem 1.25rem)
  - Tipografi: `font-size: 0.875rem (14px)`, `font-weight: 600`
  - Pasif renk: `var(--muted)` (#5F6368)
  - Hover: `color: var(--ink)` (#1F1F1F)
  - Radius: `var(--radius-pill)`
- **Aktif Durum Renk Kodları (Marka Uyumlu):**
  - **Ekip Sayfası:** `background: var(--red)` (#EA4335), `color: #ffffff`
  - **Projeler Sayfası:** `background: var(--green)` (#1E8E3E), `color: #ffffff`
  - **Etkinlikler Sayfası:** `background: var(--blue)` (#0B57D0), `color: #ffffff`

---

### Madde 2: Ekip Kartları (`MemberCard`) — Fotoğrafsız Durum Standartları
**Tasarım Hedefi:** Kartta profil fotoğrafı olmadığında veya resim yüklenemediğinde grid dengesinin, kart yüksekliğinin ve oranların bozulmaması.

- **Kart Kapsayıcısı (`.team-card` / `.member-card`):**
  - Minimum yükseklik: `min-height: 330px;` (Grid içindeki tüm kartlar eşit hizada olmalıdır)
  - Sınır & Radius: `1px solid var(--outline)`, `border-radius: var(--radius-lg)` (24px)
  - Arka plan: `var(--surface)` (#FFFFFF)
- **Görsel Alanı (`.avatar-area` / `.photo-placeholder`):**
  - Yükseklik: Sabit `220px` (veya 4:3 en-boy oranı), `overflow: hidden;`
  - Renk Teması Zeminleri:
    - Mavi kart: `background: var(--blue-soft)` (#EAF2FF)
    - Kırmızı kart: `background: var(--red-soft)` (#FCE8E6)
    - Sarı kart: `background: var(--yellow-soft)` (#FFF4D5)
    - Yeşil kart: `background: var(--green-soft)` (#E6F4EA)
- **Placeholder İçeriği:**
  - İkon Dairesi (`.placeholder-icon-wrap`): `56px x 56px` genişlik/yükseklik, tam ortalı, `border-radius: 50%`, `background: var(--surface)`, `border: 1px solid color-mix(in srgb, var(--muted) 18%, transparent)`.
  - İkon: Ortalanmış görsel / kullanıcı SVG ikonu, renk: `var(--muted)`.
  - Etiket: `Fotoğraf yakında`, `font-size: 11px`, `font-weight: 600`, `letter-spacing: 0.04em`, renk: `var(--muted)`.
- **Kırık Resim Fallback:**
  - `<img>` etiketine `loading="lazy"` ve `onerror="this.style.display='none'; if(this.nextElementSibling) this.nextElementSibling.style.display='flex';"` fallback mekanizması eklenmeli, böylece kırık 404 URL'lerinde bozuk simge yerine otomatik olarak şık placeholder görünmelidir.
- **Tipografi ve Metin Taşması:**
  - İsim (`.member-name`) ve Rol (`.member-role`) için tek satırda zorla kesilmek yerine 2 satıra kadar katlanabilen `-webkit-line-clamp: 2; word-break: break-word;` uygulanmalı, uzun unvanlar okunaksız kesilmemelidir.

---

### Madde 3: Etkinlik Detay Sayfası (`EventDetails`)
**Tasarım Hedefi:** Yeni eklenen detay sayfasının, sitenin Google GDG marka kimliği ve tasarım diliyle kusursuz bütünleşmesi.

- **Sayfa Hiyerarşisi:**
  1. **Geri Dön Butonu (`.back-link`):**
     - Metin: `← Tüm Etkinliklere Dön`
     - Stil: `var(--radius-pill)`, `border: 1px solid var(--outline)`, `color: var(--muted)`. Hover'da mavi vurgu ve sola hafif mikro animasyon (`transform: translateX(-3px)`).
  2. **Hero Alanı (`.event-details-hero`):**
     - Zemin: `var(--surface-soft)` veya yumuşak `var(--blue-soft)` radyal gradyanı (`mesh mesh--blue`).
     - Radius: `var(--radius-xl)` (32px), Sınır: `1px solid var(--outline)`.
     - Kicker: `ETKİNLİK DETAYI` + kırmızı yanıp sönen animasyonlu nokta (`.pulse-dot`).
     - Başlık: `font-size: clamp(2rem, 3.5vw, 2.75rem); font-weight: 750; color: var(--ink);`
     - Lead Açıklama: `color: var(--muted); line-height: 1.75; font-size: 1.0625rem;`
     - Aksiyonlar: "Hemen Katıl" (Filled `--blue` buton) ve "Google Takvime Ekle" (Outlined buton).
  3. **Bento Bilgi Kartları (`.meta-cards-grid`):**
     - 3 Sütunlu (Mobilde 1 sütun) bilgi rozetleri:
       - **Tarih Kartı:** Mavi ikon (`var(--blue-soft)` zemin) + Takvim ikonu + Kalın tarih metni.
       - **Saat Kartı:** Kırmızı ikon (`var(--red-soft)` zemin) + Saat ikonu + Saat metni.
       - **Konum Kartı:** Yeşil ikon (`var(--green-soft)` zemin) + Konum pini + Konum adı.
  4. **Detay ve İçerik Kartı (`.details-content-card`):**
     - Arka plan: `var(--surface)`, Radius: `var(--radius-xl)`.
     - "Etkinlik Hakkında" başlığı (`display-small`), etkinlik açıklaması ve madde işaretleri (`.bullet-dot--blue`, `--yellow`, `--green`).

---

### Madde 4: Mobil ve Masaüstü Kırılımları (Responsive Standartları)

| Ekran Boyutu | Hedef Cihaz | Grid ve Layout Davranışı |
|---|---|---|
| **< 480px (Mobil)** | Akıllı Telefonlar | Tek sütun layout, yatay kaydırılabilir filtre barları, min 44px dokunma hedefleri, ferah kart padding'leri (`16px - 20px`). |
| **481px - 768px (Tablet)** | Tablet / Küçük Ekran | Ekip kartları 2 sütun, etkinlik kartları 1-2 sütun, bento kartlar tek sütuna geçiş. |
| **769px - 1024px** | Küçük Masaüstü | Ekip kartları 2-3 sütun, etkinlik kartları 2 sütun. |
| **> 1024px (Masaüstü)** | Standart Masaüstü | Ekip kartları 4 sütun, etkinlik kartları 3 sütun, maksimum sayfa genişliği `var(--max-width)` (1240px). |

---

## 3. Token Eşleştirme Tablosu

Frontend geliştiricisi stilleri yazarken şu semantik tokenları kullanmalıdır:

| Eleman | Kullanılacak Token | Değer |
|---|---|---|
| Sayfa Arka Planı | `var(--surface)` | `#FFFFFF` |
| İkincil Kart Yüzeyi | `var(--surface-soft)` | `#F8F9FA` |
| Kart ve Input Sınırları | `var(--outline)` | `#DFE1E5` |
| Ana Metin ve Başlıklar | `var(--ink)` | `#1F1F1F` |
| Açıklamalar ve Alt Metinler | `var(--muted)` | `#5F6368` |
| Ana Mavi Vurgu | `var(--blue)` | `#0B57D0` |
| Kırmızı Vurgu | `var(--red)` | `#EA4335` |
| Sarı Vurgu | `var(--yellow)` | `#F9AB00` |
| Yeşil Vurgu | `var(--green)` | `#1E8E3E` |
| Standart Kart Radius | `var(--radius-lg)` | `24px` |
| Büyük Hero/Bölüm Radius | `var(--radius-xl)` | `32px` |
| Pill/Hap Buton Radius | `var(--radius-pill)` | `999px` |

---

## 4. İletişim & Teslimat Notu

- Canlı tasarım prototipi Figma üzerinden incelenebilir:  
  👉 [Figma Make gdg_web_design](https://www.figma.com/make/W5DjiAxUUrhzEo5fBiNqG7/gdg_web_design?t=IOPnDdJ2Jq0B4gaM-1&preview-route=%2Findex.html)
- Bu doküman, Melek'in UI/UX kontrol listesindeki tüm gereksinimleri karşılayacak şekilde teslim edilmiştir.
