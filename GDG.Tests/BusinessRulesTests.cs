using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Net.Mail;
using GDG.Application.DTOs.Event;
using GDG.Application.DTOs.Member;
using GDG.Application.DTOs.Technology;
using GDG.Application.Interfaces;
using GDG.Application.Validation;
using GDG.Web.Services;
using GDG.Domain.Entities;
using Xunit;

namespace GDG.Tests
{
    public class BusinessRulesTests
    {
        [Fact]
        public void EventDto_IsPast_CalculatesAccuratelyBasedOnTurkeyTime()
        {
            // Event date in past (yesterday in Turkey)
            var pastEvent = new EventDto
            {
                Id = 1,
                Title = "Past Workshop",
                Date = DateTime.UtcNow.AddHours(3).AddDays(-1)
            };

            // Event date in future (tomorrow in Turkey)
            var futureEvent = new EventDto
            {
                Id = 2,
                Title = "Future Summit",
                Date = DateTime.UtcNow.AddHours(3).AddDays(1)
            };

            Assert.True(pastEvent.IsPast);
            Assert.False(futureEvent.IsPast);
        }

        [Fact]
        public void CreateMemberDto_ValidationRequiresFullNameAndRole()
        {
            var invalidMember = new CreateMemberDto
            {
                FullName = "",
                Role = "",
                TeamId = 0
            };

            var context = new ValidationContext(invalidMember);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(invalidMember, context, results, true);

            Assert.False(isValid);
            Assert.Contains(results, r => r.MemberNames.Contains("FullName"));
            Assert.Contains(results, r => r.MemberNames.Contains("Role"));
            Assert.Contains(results, r => r.MemberNames.Contains("TeamId"));
        }

        [Fact]
        public void CreateMemberDto_ValidData_PassesValidation()
        {
            var validMember = new CreateMemberDto
            {
                FullName = "Ahmet Yılmaz",
                Role = "Core Team Member",
                TeamId = 1,
                Email = "ahmet@gdgsakarya.com"
            };

            var context = new ValidationContext(validMember);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(validMember, context, results, true);

            Assert.True(isValid);
        }

        [Fact]
        public void SiteSettingEntity_InitializesKeyAndValue()
        {
            var setting = new SiteSetting
            {
                Key = "CommunityName",
                Value = "GDG on Campus Sakarya"
            };

            Assert.Equal("CommunityName", setting.Key);
            Assert.Equal("GDG on Campus Sakarya", setting.Value);
            Assert.True(setting.UpdatedAt <= DateTime.UtcNow);
        }

        [Fact]
        public void ContactMessageEntity_DefaultsToUnread()
        {
            var message = new ContactMessage
            {
                FullName = "Test Ziyaretçi",
                Email = "ziyaretci@example.com",
                Subject = "Soru",
                Message = "Etkinlikler hakkında bilgi almak istiyorum."
            };

            Assert.False(message.IsRead);
            Assert.Equal("Test Ziyaretçi", message.FullName);
        }

        [Fact]
        public void UpdateTechnologyDto_ValidationRequiresName()
        {
            var invalidTech = new UpdateTechnologyDto
            {
                Id = 1,
                Name = ""
            };

            var context = new ValidationContext(invalidTech);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(invalidTech, context, results, true);

            Assert.False(isValid);
            Assert.Contains(results, r => r.MemberNames.Contains("Name"));
        }

        // ─── RETURN URL GÜVENLİK TESTLERİ ───────────────────────────────────────

        [Theory]
        [InlineData("/admin", true)]            // Geçerli yerel URL
        [InlineData("/admin/settings", true)]   // Geçerli yerel URL
        [InlineData("/", true)]                 // Kök sayfa
        [InlineData("//evil.com", false)]       // Protokol-bağıl harici URL (saldırı)
        [InlineData("/\\evil.com", false)]      // Ters eğik çizgi harici URL (saldırı)
        [InlineData("https://evil.com", false)] // Tam harici URL
        [InlineData("http://evil.com", false)]  // HTTP harici URL
        [InlineData("", false)]                 // Boş URL
        [InlineData(null, false)]               // Null URL
        [InlineData("javascript:alert(1)", false)] // XSS girişimi
        public void IsLocalUrl_ReturnsExpectedResult(string? url, bool expected)
        {
            var result = UrlSafety.IsLocalUrl(url);
            Assert.Equal(expected, result);
        }

        // ─── İLETİŞİM FORMU DOĞRULAMA TESTLERİ ──────────────────────────────────

        [Fact]
        public void CreateContactMessageDto_EmptyFullName_FailsValidation()
        {
            var dto = new CreateContactMessageDto
            {
                FullName = "",
                Email = "test@example.com",
                Subject = "Genel",
                Message = "Test mesajı"
            };

            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, context, results, true);

            Assert.False(isValid);
            Assert.Contains(results, r => r.MemberNames.Contains("FullName"));
        }

        [Fact]
        public void CreateContactMessageDto_InvalidEmail_FailsValidation()
        {
            var dto = new CreateContactMessageDto
            {
                FullName = "Test User",
                Email = "gecersiz-email",
                Subject = "Genel",
                Message = "Test mesajı"
            };

            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, context, results, true);

            Assert.False(isValid);
            Assert.Contains(results, r => r.MemberNames.Contains("Email"));
        }

        [Fact]
        public void CreateContactMessageDto_ValidData_PassesValidation()
        {
            var dto = new CreateContactMessageDto
            {
                FullName = "Ayşe Yılmaz",
                Email = "ayse@example.com",
                Subject = "Genel",
                Message = "Bu bir test mesajıdır."
            };

            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, context, results, true);

            Assert.True(isValid);
        }

        [Fact]
        public void CreateContactMessageDto_TooLongFullName_FailsValidation()
        {
            var dto = new CreateContactMessageDto
            {
                FullName = new string('A', 101), // 101 karakter, max 100
                Email = "test@example.com",
                Subject = "Genel",
                Message = "Test mesajı"
            };

            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, context, results, true);

            Assert.False(isValid);
            Assert.Contains(results, r => r.MemberNames.Contains("FullName"));
        }

        [Fact]
        public void CreateContactMessageDto_TooLongMessage_FailsValidation()
        {
            var dto = new CreateContactMessageDto
            {
                FullName = "Test User",
                Email = "test@example.com",
                Subject = "Genel",
                Message = new string('x', 2001) // 2001 karakter, max 2000
            };

            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, context, results, true);

            Assert.False(isValid);
            Assert.Contains(results, r => r.MemberNames.Contains("Message"));
        }

        // ─── SİTE AYARLARI TESTLERİ ──────────────────────────────────────────────

        [Fact]
        public void SiteSettingsDto_DefaultValues_AreNotEmpty()
        {
            var dto = new SiteSettingsDto();

            Assert.NotEmpty(dto.CommunityName);
            Assert.NotEmpty(dto.UniversityName);
            Assert.True(string.IsNullOrEmpty(dto.ContactEmail), "Resmi iletişim e-postası doğrulanana kadar varsayılan e-posta yayınlanmamalıdır.");
        }

        // ─── EKİP/ÜYE İLİŞKİSİ TESTLERİ ────────────────────────────────────────

        [Fact]
        public void MemberEntity_TeamId_CanBeAssigned()
        {
            var member = new Member
            {
                FullName = "Mehmet Öz",
                Role = "Developer",
                TeamId = 5
            };

            Assert.Equal(5, member.TeamId);
        }

        [Fact]
        public void TeamEntity_CanHaveMultipleMembers()
        {
            var team = new Team
            {
                Id = 1,
                Name = "Core Team",
                Description = "Ana ekip",
                Members = new List<Member>
                {
                    new Member { FullName = "Ali", Role = "Lead", TeamId = 1 },
                    new Member { FullName = "Veli", Role = "Member", TeamId = 1 }
                }
            };

            Assert.Equal(2, team.Members.Count);
            Assert.All(team.Members, m => Assert.Equal(1, m.TeamId));
        }


        [Theory]
        [InlineData("/%2f%2fevil.example", false)]
        [InlineData("/%252f%252fevil.example", false)]
        [InlineData("/%255c%255cevil.example", false)]
        [InlineData("/\\evil.example", false)]
        [InlineData("/admin\n", false)]
        [InlineData("admin", false)]
        public void UrlSafety_RejectsMalformedLocalRedirectTargets(string? url, bool expected)
        {
            Assert.Equal(expected, UrlSafety.IsLocalUrl(url));
        }

        [Theory]
        [InlineData("https://example.com/path", true)]
        [InlineData("javascript:alert(1)", false)]
        [InlineData("//evil.example", false)]
        [InlineData("https://trusted.example@evil.example", false)]
        public void UrlSafety_ExternalUrlAllowlistAcceptsOnlyHttpSchemes(string? url, bool expected)
        {
            Assert.Equal(expected, UrlSafety.GetSafeHttpUrl(url) != null);
        }

        [Fact]
        public void ContactMessageValidator_RejectsInvalidEmailAndLengths()
        {
            var dto = new CreateContactMessageDto
            {
                FullName = "Test User",
                Email = "not-an-email",
                Subject = "Genel",
                Message = "A test message"
            };

            Assert.False(ContactMessageValidator.TryValidate(dto, out var errors));
            Assert.Contains(errors, error => error.MemberNames.Contains(nameof(dto.Email)));
        }
    }
}
