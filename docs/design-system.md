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

Figma projesinde aşağıdaki sayfaları oluşturun:

1. `00 — Cover`
2. `01 — Foundations`
3. `02 — Components`
4. `03 — Home`
5. `04 — Events`

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

### Navigation item

Figma component adı: `Navigation Item`

Properties:

- `State`: Default / Hover / Active

Frontend sınıfları: `.nav-link` ve `.nav-link--active`.

### Event card

Figma component adı: `Card / Event`

İçerik alanları:

- Etkinlik görseli
- Tarih etiketi
- Başlık
- Saat
- Konum
- Takvime ekle aksiyonu

Frontend karşılığı: `EventCard`.

### Team card

Figma component adı: `Card / Team Member`

İçerik alanları:

- Profil görseli
- İsim
- Rol
- GitHub ve LinkedIn aksiyonları

Frontend karşılığı: `MemberCard`.

## 7. Responsive teslim

Her sayfa için en az iki frame hazırlayın:

- Desktop: `1440px`
- Mobile: `390px`

Componentlerde Auto Layout kullanın. Metinleri sabit yükseklikle kesmeyin ve
kart içeriklerinin uzayabilmesine izin verin.

## 8. Teslim kontrol listesi

- Figma linkinin görüntüleme izni açık.
- Renkler Local Variables olarak kayıtlı.
- Tipografiler Text Styles olarak kayıtlı.
- Buton, navigation, event card ve team card component yapılmış.
- Component durumları variant olarak tanımlanmış.
- Home ve Events sayfalarının desktop tasarımları hazır.
- Home ve Events sayfalarının mobile taslakları hazır.
- Frontend ekibi `src/styles/tokens.css` dosyasını kullanıyor.
- Figma isimleri ile CSS token isimleri eşleşiyor.
