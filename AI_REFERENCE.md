# AI Reference — GDG on Campus Sakarya

Bu dosya, projede çalışacak AI araçları ve geliştiriciler için kısa ve
bağlayıcı proje referansıdır. Değişiklik yapmadan önce `AGENTS.md` ve bu dosyayı
okuyun.

## Proje amacı

GDG on Campus Sakarya topluluğu için React tabanlı, responsive bir tanıtım ve
etkinlik sitesi. Görsel dil Google marka renklerinden, geniş beyaz alanlardan,
soft renk yüzeylerinden ve yuvarlatılmış kartlardan oluşur.

## Teknoloji

- React 19
- TypeScript
- Vite 8
- Tailwind CSS 4 altyapısı
- Global component stilleri: `src/index.css`
- Tasarım tokenları: `src/styles/tokens.css`

Yeni bir UI kütüphanesi eklemeyin. Mevcut componentleri ve CSS tokenlarını
kullanın.

## Önemli dosyalar

- `src/main.tsx`: React giriş noktası
- `src/App.tsx`: Sayfalar, componentler ve örnek içerikler
- `src/index.css`: Layout ve component stilleri
- `src/styles/tokens.css`: Renk, spacing, radius, tipografi ve gölge tokenları
- `docs/design-system.md`: Figma ve frontend tasarım sistemi teslim rehberi
- `index.html`: Vite HTML kabuğu; Figma Make slotlarını koruyun
- `.figma/make/site.json`: Sayfa metadata ayarları

## Sayfalar

- Ana sayfa: `index.html`
- Etkinlikler: `index.html?page=events`

Query tabanlı sayfa seçimi bilinçli bir tercihtir. Böylece production çıktısı
bir ZIP içinden veya basit bir statik sunucudan ek yönlendirme ayarı olmadan
çalışır.

## Tasarım sistemi kuralları

1. Renkleri doğrudan HEX olarak tekrar yazmayın; `tokens.css` değişkenlerini
   kullanın.
2. Yeni spacing ve radius değerleri eklemeden önce mevcut token ölçeğini
   kontrol edin.
3. Primary aksiyonlar için `Action`, metin bağlantıları için `Link` kullanın.
4. Event kartlarında `EventCard`, ekip kartlarında `MemberCard` yapısını
   koruyun.
5. Header, footer, font bağlantısı, soft arka planlar ve responsive davranış
   kullanıcı açıkça istemedikçe değiştirilmemelidir.
6. Desktop ve mobil görünümü birlikte değerlendirin.
7. Kullanıcı içeriğini ve mevcut çalışan bölümleri silmeyin.

## Görsel dil

- Font: Inter
- Ana metin: `--ink`
- İkincil metin: `--muted`
- Kart radius: çoğunlukla `--radius-lg`
- Büyük yüzey radius: `--radius-xl`
- Ana aksiyon: `--blue`
- Marka vurguları: blue, red, yellow ve green tokenları
- Arka plan renkleri düşük opaklıklı ve yumuşak olmalıdır

## Komutlar

```bash
pnpm run dev
pnpm run build
pnpm run build:portable
pnpm run format
```

Figma Make geliştirme sunucusu normalde zaten çalışır; ikinci bir sunucu
başlatmayın.

## Doğrulama

UI değişikliğinden sonra:

1. Ana sayfayı kontrol edin.
2. `?page=events` sayfasını kontrol edin.
3. Mobil kırılımı kontrol edin.
4. `pnpm run build` komutunu çalıştırın.
5. ZIP teslimi gerekiyorsa `pnpm run build:portable` kullanın.

## ZIP çıktısı

Portable build, asset yollarını göreli üretir. `dist` klasörünün içeriği tek
başına arşivlenebilir. Arşiv açıldıktan sonra `index.html` doğrudan açılabilir
veya herhangi bir statik sunucuya yüklenebilir.
