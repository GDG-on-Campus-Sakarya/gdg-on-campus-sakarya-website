using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;
using GDG.Application.DTOs;
using GDG.Application.Interfaces;
using GDG.Domain.Entities;
using GDG.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace GDG.Infrastructure.Services
{
    public class SiteSettingsService : ISiteSettingsService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public SiteSettingsService(
            IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<string> GetSettingAsync(
            string key,
            string defaultValue = "")
        {
            await using var db = await _contextFactory.CreateDbContextAsync();

            var setting = await db.SiteSettings
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Key == key);

            return setting != null ? setting.Value : defaultValue;
        }

        public async Task SetSettingAsync(string key, string value)
        {
            await using var db = await _contextFactory.CreateDbContextAsync();

            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException(
                    "Ayar anahtarı zorunludur.",
                    nameof(key));

            value = (value ?? string.Empty).Trim();
            ValidateSettingValue(key, value);

            var setting = await db.SiteSettings
                .FirstOrDefaultAsync(s => s.Key == key);

            if (setting == null)
            {
                db.SiteSettings.Add(new SiteSetting
                {
                    Key = key,
                    Value = value,
                    UpdatedAt = DateTime.UtcNow
                });
            }
            else
            {
                setting.Value = value;
                setting.UpdatedAt = DateTime.UtcNow;
            }

            await db.SaveChangesAsync();
        }

        public async Task<SiteSettingsDto> GetAllSettingsAsync()
        {
            await using var db = await _contextFactory.CreateDbContextAsync();

            var settings = await db.SiteSettings
                .AsNoTracking()
                .ToListAsync();

            var dict = new Dictionary<string, string>(
                StringComparer.OrdinalIgnoreCase);

            foreach (var setting in settings)
            {
                dict[setting.Key] = setting.Value;
            }

            var dto = new SiteSettingsDto();

            if (dict.TryGetValue("CommunityName", out var cn))
                dto.CommunityName = cn;

            if (dict.TryGetValue("UniversityName", out var un))
                dto.UniversityName = un;

            if (dict.TryGetValue("CampusLocation", out var cl))
                dto.CampusLocation = cl;

            if (dict.TryGetValue("ContactEmail", out var ce))
                dto.ContactEmail = ce;

            if (dict.TryGetValue("GitHubUrl", out var gh))
                dto.GitHubUrl = gh;

            if (dict.TryGetValue("CommunityDevUrl", out var cd))
                dto.CommunityDevUrl = cd;

            if (dict.TryGetValue("LinkedInUrl", out var li))
                dto.LinkedInUrl = li;

            if (dict.TryGetValue("InstagramUrl", out var ig))
                dto.InstagramUrl = ig;

            return dto;
        }

        public async Task SaveAllSettingsAsync(SiteSettingsDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            ValidateSettings(dto);

            await using var db = await _contextFactory.CreateDbContextAsync();

            await SetSettingInternalAsync(
                db, "CommunityName", dto.CommunityName);

            await SetSettingInternalAsync(
                db, "UniversityName", dto.UniversityName);

            await SetSettingInternalAsync(
                db, "CampusLocation", dto.CampusLocation);

            await SetSettingInternalAsync(
                db, "ContactEmail", dto.ContactEmail);

            await SetSettingInternalAsync(
                db, "GitHubUrl", dto.GitHubUrl);

            await SetSettingInternalAsync(
                db, "CommunityDevUrl", dto.CommunityDevUrl);

            await SetSettingInternalAsync(
                db, "LinkedInUrl", dto.LinkedInUrl);

            await SetSettingInternalAsync(
                db, "InstagramUrl", dto.InstagramUrl);

            await db.SaveChangesAsync();
        }

        private async Task SetSettingInternalAsync(
            ApplicationDbContext db,
            string key,
            string value)
        {
            value = (value ?? string.Empty).Trim();

            var setting = await db.SiteSettings
                .FirstOrDefaultAsync(s => s.Key == key);

            if (setting == null)
            {
                db.SiteSettings.Add(new SiteSetting
                {
                    Key = key,
                    Value = value,
                    UpdatedAt = DateTime.UtcNow
                });
            }
            else
            {
                setting.Value = value;
                setting.UpdatedAt = DateTime.UtcNow;
            }
        }

        private static void ValidateSettings(SiteSettingsDto dto)
        {
            ValidateSettingValue("CommunityName", dto.CommunityName);
            ValidateSettingValue("UniversityName", dto.UniversityName);
            ValidateSettingValue("CampusLocation", dto.CampusLocation);
            ValidateSettingValue("ContactEmail", dto.ContactEmail);
            ValidateSettingValue("GitHubUrl", dto.GitHubUrl);
            ValidateSettingValue("CommunityDevUrl", dto.CommunityDevUrl);
            ValidateSettingValue("LinkedInUrl", dto.LinkedInUrl);
            ValidateSettingValue("InstagramUrl", dto.InstagramUrl);
        }

        private static void ValidateOptionalHttpUrl(
            string? value,
            string fieldName)
        {
            if (string.IsNullOrWhiteSpace(value))
                return;

            if (value.Trim().Length > 2048 ||
                !Uri.TryCreate(
                    value.Trim(),
                    UriKind.Absolute,
                    out var uri) ||
                (uri.Scheme != Uri.UriSchemeHttps &&
                 uri.Scheme != Uri.UriSchemeHttp) ||
                !string.IsNullOrEmpty(uri.UserInfo))
            {
                throw new ArgumentException(
                    $"{fieldName} bağlantısı http:// veya https:// ile başlayan geçerli bir URL olmalıdır.");
            }
        }

        private static void ValidateSettingValue(
            string key,
            string? value)
        {
            value ??= string.Empty;

            switch (key)
            {
                case "CommunityName":
                    if (string.IsNullOrWhiteSpace(value) ||
                        value.Trim().Length > 120)
                    {
                        throw new ArgumentException(
                            "Topluluk adı zorunludur ve 120 karakteri aşamaz.");
                    }
                    break;

                case "UniversityName":
                    if (string.IsNullOrWhiteSpace(value) ||
                        value.Trim().Length > 160)
                    {
                        throw new ArgumentException(
                            "Üniversite adı zorunludur ve 160 karakteri aşamaz.");
                    }
                    break;

                case "CampusLocation":
                    if (string.IsNullOrWhiteSpace(value) ||
                        value.Trim().Length > 200)
                    {
                        throw new ArgumentException(
                            "Kampüs/konum alanı zorunludur ve 200 karakteri aşamaz.");
                    }
                    break;

                case "ContactEmail":
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        try
                        {
                            var parsed = new MailAddress(value.Trim());

                            if (value.Trim().Length > 254 ||
                                !string.Equals(
                                    parsed.Address,
                                    value.Trim(),
                                    StringComparison.OrdinalIgnoreCase))
                            {
                                throw new ArgumentException(
                                    "Geçerli bir iletişim e-posta adresi girin.");
                            }
                        }
                        catch (FormatException)
                        {
                            throw new ArgumentException(
                                "Geçerli bir iletişim e-posta adresi girin.");
                        }
                    }
                    break;

                case "GitHubUrl":
                case "CommunityDevUrl":
                case "LinkedInUrl":
                case "InstagramUrl":
                    ValidateOptionalHttpUrl(value, key);
                    break;

                default:
                    throw new ArgumentException("Bilinmeyen site ayarı.");
            }
        }
    }
}