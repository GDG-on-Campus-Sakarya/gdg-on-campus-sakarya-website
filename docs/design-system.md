# GDG on Campus Sakarya — Design System

Bu belge, Figma tasarımları ile frontend uygulamasının aynı görsel dili
kullanmasını sağlar. Frontend için teknik kaynak `src/styles/tokens.css`,
component örnekleri için kaynak ise `src/App.tsx` ve `src/index.css` dosyalarıdır.

## 1. Frontend kurulumu

Global stil giriş noktası `src/index.css` dosyasıdır. Token dosyası burada import
edildiği için componentlerde değişkenler doğrudan kullanılabilir:

```css
.example-card {
  padding: var(--space-6);
  border: 1px solid var(--outline);
  border-radius: var(--radius-lg);
  color: var(--ink);
  background: var(--surface);
}
```

Yeni componentlerde sabit renk değeri yazmak yerine semantik token kullanın.
Örneğin `#5f6368` yerine `var(--muted)` tercih edin.

## 2. Figma dosya yapısı

Figma Proje Bağlantısı: [gdg_web_design Figma Make Projesi](https://www.figma.com/make/W5DjiAxUUrhzEo5fBiNqG7/gdg_web_design?t=IOPnDdJ2Jq0B4gaM-1&preview-route=%2Findex.html)

Figma projesinde aşağıdaki sayfaları oluşturun:

1. `00 — Cover`
2. `01 — Foundations`
3. `02 — Components`
4. `03 — Home`
5. `04 — Events`
   - Alt görünüm / Detay: `Events / Event Details` (Etkinlik Detayı)
6. `05 — Team`
7. `06 — Projects`

`Foundations` sayfasında renk, tipografi, spacing ve radius değerleri;
`Components` sayfasında tekrar kullanılan componentler bulunmalıdır.

## 3. Renkler

Figma Local Variables koleksiyonunun adı `GDG / Colors` olsun.

| Figma adı | CSS token | Değer | Kullanım |
| --- | --- | --- | --- |
| `Brand/Blue` | `--blue` | `#0B57D0` | Ana buton ve aktif durum |
| `Brand/Blue Bright` | `--blue-bright` | `#4285F4` | Marka vurgusu |
| `Brand/Red` | `--red` | `#EA4335` | Kırmızı marka vurgusu |
| `Brand/Yellow` | `--yellow` | `#F9AB00` | Sarı marka vurgusu |
| `Brand/Green` | `--green` | `#1E8E3E` | Yeşil marka vurgusu |
| `Text/Primary` | `--ink` | `#1F1F1F` | Başlıklar ve ana metin |
| `Text/Secondary` | `--muted` | `#5F6368` | Açıklamalar ve metadata |
| `Border/Default` | `--outline` | `#DFE1E5` | Kart ve input sınırları |
| `Surface/Default` | `--surface` | `#FFFFFF` | Ana yüzey |
| `Surface/Soft` | `--surface-soft` | `#F8F9FA` | İkincil yüzey |
| `Surface/Blue Soft` | `--blue-soft` | `#EAF2FF` | Mavi kart yüzeyi |
| `Surface/Red Soft` | `--red-soft` | `#FCE8E6` | Kırmızı kart yüzeyi |
| `Surface/Yellow Soft` | `--yellow-soft` | `#FFF4D5` | Sarı kart yüzeyi |
| `Surface/Green Soft` | `--green-soft` | `#E6F4EA` | Yeşil kart yüzeyi |

## 4. Tipografi

Font ailesi `Inter`dır. Figma Text Styles isimleri:

| Figma stili | CSS karşılığı | Ağırlık | Kullanım |
| --- | --- | --- | --- |
| `Display/Large` | `--font-display-large` | 700 | Hero başlığı |
| `Display/Medium` | `--font-display-medium` | 680–700 | Bölüm başlığı |
| `Display/Small` | `--font-display-small` | 650–700 | Büyük kart başlığı |
| `Body/Large` | `--font-body-large` | 400 | Hero açıklaması |
| `Body/Regular` | `--font-body` | 400 | Normal açıklama |
| `Label/Small` | `--font-label` | 700 | Kicker, tarih ve etiket |

Display stillerinde sıkı harf aralığı; body stillerinde yaklaşık `1.65–1.75`
satır yüksekliği kullanın.

## 5. Spacing ve radius

Figma spacing variable değerleri CSS ile aynı isim mantığını kullanır:

`4, 8, 12, 16, 20, 24, 32, 40, 48, 64, 80`

Radius değerleri:

| Figma adı | CSS token | Değer |
| --- | --- | --- |
| `Radius/Small` | `--radius-sm` | 8 |
| `Radius/Medium` | `--radius-md` | 16 |
| `Radius/Large` | `--radius-lg` | 24 |
| `Radius/Extra Large` | `--radius-xl` | 32 |
| `Radius/Pill` | `--radius-pill` | 999 |

## 6. Temel componentler

### Button

Figma component adı: `Button`

Properties:

- `Variant`: Primary / Outlined
- `State`: Default / Hover / Disabled
- `Icon`: None / Leading / Trailing
- `Size`: Default

Frontend karşılığı:

```tsx
<Action href="index.html?page=events">Etkinlikleri İncele</Action>
<Action href="index.html?page=events" kind="outlined">Detaylar</Action>
```

### Navigation item & Navbar Yapısı

Figma component adı: `Navigation Item`

Properties:

- `State`: Default / Hover / Active

Frontend sınıfları: `.nav-link` ve `.nav-link--active`.

> **Önemli:** Navbar'da "Ekibimiz", "Etkinlikler" ve "Projeler" bağlantıları artık sayfa içi çapa linki (`#`) değil, ayrı sayfalara yönlendirilmektedir:
> - **Ana Sayfa:** `index.html`
> - **Etkinlikler:** `index.html?page=events`
> - **Ekibimiz:** `index.html?page=team`
> - **Projeler:** `index.html?page=projects`

### Event card

Figma component adı: `Card / Event`

İçerik alanları:

- Etkinlik görseli
- Tarih etiketi
- Başlık
- Saat
- Konum
- Detaylar ve Takvime ekle aksiyonları

Frontend karşılığı: `EventCard`.

### Team card

Figma component adı: `Card / Team Member`

İçerik alanları:

- Profil görseli (görsel olmadığında bozulmayan baş harf/avatar alanı)
- İsim
- Rol
- GitHub ve LinkedIn aksiyonları

Frontend karşılığı: `MemberCard`.

### Page Hero

Figma component adı: `Page Hero`

İçerik alanları:

- Sayfa Kicker / Badge (`.hero-kicker-tag`)
- Sayfa Başlığı (`Display/Large` veya `Display/Medium`)
- Açıklama Metni (`Body/Large`)
- İkincil Aksiyonlar / Buton Grubu

Frontend karşılığı: `.page-hero` / `PageHero`.

### Stat Tile

Figma component adı: `Stat Tile`

İçerik alanları:

- Sayısal Veri / Metrik (`Display/Large` veya `Display/Medium`)
- Açıklama Etiketi (`Label/Small` veya `Body/Regular`)
- Vurgu Rengi Çizgisi veya Arka Planı (Mavi/Kırmızı/Sarı/Yeşil soft)

Frontend karşılığı: `.stat-tile` / `StatTile`.

### Card / Info

Figma component adı: `Card / Info`

İçerik alanları:

- İkon Alanı (`.icon-wrap`)
- Başlık (`Display/Small`)
- Detay Metni (`Body/Regular`)

Frontend karşılığı: `.info-card` / `InfoCard`.

### Card / Project Facts

Figma component adı: `Card / Project Facts`

İçerik alanları:

- Proje Başlığı ve Kısa Açıklama
- Kullanılan Teknolojiler / Etiketler (`badge-pill`)
- Katkıda Bulunanlar / Ekip Bilgisi
- GitHub ve Canlı Demo Linkleri

Frontend karşılığı: `.project-facts-card` / `ProjectFactsCard`.

### Category Filters

Figma component adı: `Category Filters`

İçerik alanları:

- Kategori / Sekme Butonları (`Pill` radius)
- Aktif ve Varsayılan Durumlar (`.filter-tab--active`)

Frontend karşılığı: `.category-filters` / `.section-filters-bar`.

## 7. Ekran Genişliklerine Göre Dizilim (Responsive Layouts)

Bileşenlerin ekran genişliklerine göre yerleşim kuralları:

| Bileşen | Desktop (1440px) | Tablet (768px) | Mobile (390px) |
| --- | --- | --- | --- |
| **Page Hero** | 1200px maksimum genişlik, merkezlenmiş içerik, yatay buton grubu (`gap: 16px`). | Akışkan genişlik, padding: 32px, butonlar yatay. | Tam genişlik, padding: 20px, butonlar tam genişlik ve alt alta (`column`). |
| **Category Filters** | Sayfa ortasında merkezlenmiş, yatay sıralı (`justify-content: center`). | Merkezlenmiş, esnek satır kaydırma (`flex-wrap`). | Tek satırda yatay kaydırılabilir (`overflow-x: auto`, `white-space: nowrap`), mobilde taşmaz. |
| **Card / Event** | 3 kolonlu grid (`grid-template-columns: repeat(3, 1fr)`). | 2 kolonlu grid. | 1 kolon (tek sütun dikey liste), kart genişliği %100. |
| **Card / Team Member** | 4 kolonlu grid (`grid-template-columns: repeat(4, 1fr)`). | 2 veya 3 kolonlu grid. | 2 kolon kompakt grid veya tek sütun. |
| **Stat Tile** | 4 kolonlu yatay sıralı grid. | 2 kolonlu grid (`repeat(2, 1fr)`). | 2 kolonlu kompakt veya alt alta tek kolon. |
| **Card / Info** | 3 kolonlu grid (`repeat(3, 1fr)`). | 2 kolonlu grid. | 1 kolon dikey liste. |
| **Card / Project Facts** | 2 veya 3 kolonlu bento grid yapısı. | 2 kolonlu grid. | Tek sütun dikey akış. |

Tüm componentlerde Auto Layout kullanın. Metinleri sabit yükseklikle kesmeyin ve
kart içeriklerinin dinamik olarak uzayabilmesine izin verin.

## 8. Teslim kontrol listesi

- Figma linkinin görüntüleme izni açık.
- Renkler Local Variables olarak kayıtlı.
- Tipografiler Text Styles olarak kayıtlı.
- Buton, navigation, event card, team card, page hero, stat tile, info card ve category filters component yapılmış.
- Component durumları variant olarak tanımlanmış.
- Home, Events (ve Event Details), Team ve Projects sayfalarının desktop tasarımları hazır.
- Home, Events, Team ve Projects sayfalarının mobile taslakları hazır.
- Kategori filtreleri mobilde yatay kaydırılabilir yapıda test edilmiş.
- Navbar bağlantıları bağımsız sayfalara (`?page=events`, `?page=team`, `?page=projects`) yönlenecek şekilde planlanmış.
- Frontend ekibi `src/styles/tokens.css` dosyasını kullanıyor.
- Figma isimleri ile CSS token isimleri eşleşiyor.
