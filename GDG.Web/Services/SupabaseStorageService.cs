using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace GDG.Web.Services
{
    public sealed class SupabaseStorageService
    {
        private const long MaxAvatarSizeBytes = 5 * 1024 * 1024;

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public SupabaseStorageService(
            HttpClient httpClient,
            IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<string> UploadMemberAvatarAsync(
            Stream imageStream,
            string contentType,
            CancellationToken cancellationToken = default)
        {
            var projectUrl = _configuration["Supabase:Url"];
            var bucket = _configuration["Supabase:StorageBucket"];
            var serviceRoleKey = _configuration["Supabase:ServiceRoleKey"];

            if (string.IsNullOrWhiteSpace(projectUrl)
                || string.IsNullOrWhiteSpace(bucket)
                || string.IsNullOrWhiteSpace(serviceRoleKey))
            {
                throw new InvalidOperationException(
                    "Supabase Storage ayarları eksik. " +
                    "Supabase:Url, Supabase:StorageBucket ve " +
                    "Supabase:ServiceRoleKey ayarlarını kontrol edin.");
            }

            if (!Uri.TryCreate(
                    projectUrl,
                    UriKind.Absolute,
                    out var projectUri)
                || projectUri.Scheme != Uri.UriSchemeHttps)
            {
                throw new InvalidOperationException(
                    "Supabase proje URL'si geçerli bir HTTPS adresi olmalıdır.");
            }

            contentType = contentType.Trim().ToLowerInvariant();

            var extension = contentType switch
            {
                "image/jpeg" => ".jpg",
                "image/png" => ".png",
                "image/webp" => ".webp",
                _ => throw new InvalidOperationException(
                    "Yalnızca JPG, PNG ve WebP görselleri yüklenebilir.")
            };

            // Dosya boyutunu akış okunurken sınırla.
            await using var memoryStream = new MemoryStream();

            var buffer = new byte[81920];

            while (true)
            {
                var bytesRead = await imageStream.ReadAsync(
                    buffer.AsMemory(0, buffer.Length),
                    cancellationToken);

                if (bytesRead == 0)
                    break;

                if (memoryStream.Length + bytesRead > MaxAvatarSizeBytes)
                {
                    throw new InvalidOperationException(
                        "Fotoğrafın boyutu 5 MB sınırını aşıyor.");
                }

                await memoryStream.WriteAsync(
                    buffer.AsMemory(0, bytesRead),
                    cancellationToken);
            }

            var imageBytes = memoryStream.ToArray();

            if (imageBytes.Length == 0)
            {
                throw new InvalidOperationException(
                    "Seçilen dosya boş.");
            }

            // Tarayıcının bildirdiği içerik türüne tek başına güvenme.
            if (!HasValidImageSignature(imageBytes, contentType))
            {
                throw new InvalidOperationException(
                    "Seçilen dosyanın içeriği geçerli bir JPG, PNG veya WebP görseli değil.");
            }

            // Aynı isimdeki dosyanın üzerine yazmak yerine benzersiz bir isim kullan.
            var objectPath = $"members/{Guid.NewGuid():N}{extension}";

            var encodedBucket = Uri.EscapeDataString(bucket);

            var encodedObjectPath = string.Join(
                "/",
                objectPath.Split('/').Select(Uri.EscapeDataString));

            var baseUrl = projectUri.GetLeftPart(UriPartial.Authority);

            var uploadUrl =
                $"{baseUrl}/storage/v1/object/{encodedBucket}/{encodedObjectPath}";

            using var request = new HttpRequestMessage(
                HttpMethod.Post,
                uploadUrl);

            // Bu anahtar yalnızca sunucuda kullanılmalıdır.
            request.Headers.TryAddWithoutValidation(
                "apikey",
                serviceRoleKey);

            request.Headers.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    serviceRoleKey);

            request.Content = new ByteArrayContent(imageBytes);

            request.Content.Headers.ContentType =
                new MediaTypeHeaderValue(contentType);

            using var response = await _httpClient.SendAsync(
                request,
                cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync(
                    cancellationToken);

                throw new InvalidOperationException(
                    $"Fotoğraf Supabase Storage'a yüklenemedi. " +
                    $"HTTP {(int)response.StatusCode}. {responseBody}");
            }

            // Public bucket için görüntünün adresi.
            return
                $"{baseUrl}/storage/v1/object/public/{encodedBucket}/{encodedObjectPath}";
        }

        private static bool HasValidImageSignature(
            byte[] bytes,
            string contentType)
        {
            return contentType switch
            {
                "image/jpeg" =>
                    bytes.Length >= 3
                    && bytes[0] == 0xFF
                    && bytes[1] == 0xD8
                    && bytes[2] == 0xFF,

                "image/png" =>
                    bytes.Length >= 8
                    && bytes.AsSpan(0, 8).SequenceEqual(
                        new byte[]
                        {
                            0x89, 0x50, 0x4E, 0x47,
                            0x0D, 0x0A, 0x1A, 0x0A
                        }),

                "image/webp" =>
                    bytes.Length >= 12
                    && Encoding.ASCII.GetString(bytes, 0, 4) == "RIFF"
                    && Encoding.ASCII.GetString(bytes, 8, 4) == "WEBP",

                _ => false
            };
        }
    }
}