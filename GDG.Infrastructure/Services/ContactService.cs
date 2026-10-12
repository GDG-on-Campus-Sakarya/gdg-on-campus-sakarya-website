using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GDG.Application.Interfaces;
using GDG.Application.Validation;
using GDG.Domain.Entities;
using GDG.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GDG.Infrastructure.Services
{
    public class ContactService : IContactService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private readonly ILogger<ContactService> _logger;

        public ContactService(
            IDbContextFactory<ApplicationDbContext> contextFactory,
            ILogger<ContactService> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<bool> SubmitMessageAsync(CreateContactMessageDto dto)
        {
            if (!ContactMessageValidator.TryValidate(
                dto, out var validationErrors))
            {
                var invalidFields = string.Join(",",
                    validationErrors
                        .SelectMany(e => e.MemberNames)
                        .Where(name => !string.IsNullOrWhiteSpace(name))
                        .Distinct());

                _logger.LogWarning(
                    "Contact submission rejected by validation. Fields={Fields}",
                    string.IsNullOrWhiteSpace(invalidFields)
                        ? "unknown"
                        : invalidFields);

                return false;
            }

            await using var db = await _contextFactory.CreateDbContextAsync();

            ContactMessage? message = null;

            try
            {
                message = new ContactMessage
                {
                    FullName = dto.FullName.Trim(),
                    Email = dto.Email.Trim().ToLowerInvariant(),
                    Subject = string.IsNullOrWhiteSpace(dto.Subject)
                        ? "Genel"
                        : dto.Subject.Trim(),
                    Message = dto.Message.Trim(),
                    CreatedAt = DateTime.UtcNow,
                    IsRead = false
                };

                db.ContactMessages.Add(message);
                await db.SaveChangesAsync();

                _logger.LogInformation(
                    "ContactService: Mesaj başarıyla kaydedildi. Id={MessageId}",
                    message.Id);

                return true;
            }
            catch (Exception ex)
            {
                if (message != null)
                    db.Entry(message).State = EntityState.Detached;

                _logger.LogError(
                    "ContactService.SubmitMessageAsync: Veritabanı kayıt hatası. ExceptionType={ExceptionType}",
                    ex.GetType().Name);

                return false;
            }
        }

        public async Task<List<ContactMessageDto>> GetAllMessagesAsync()
        {
            await using var db = await _contextFactory.CreateDbContextAsync();

            var messages = await db.ContactMessages
                .AsNoTracking()
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();

            return messages.Select(m => new ContactMessageDto
            {
                Id = m.Id,
                FullName = m.FullName,
                Email = m.Email,
                Subject = m.Subject,
                Message = m.Message,
                CreatedAt = TurkeyTime.FromUtc(m.CreatedAt),
                IsRead = m.IsRead
            }).ToList();
        }

        public async Task<ContactMessageDto?> GetMessageByIdAsync(int id)
        {
            await using var db = await _contextFactory.CreateDbContextAsync();

            var message = await db.ContactMessages
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (message == null)
                return null;

            return new ContactMessageDto
            {
                Id = message.Id,
                FullName = message.FullName,
                Email = message.Email,
                Subject = message.Subject,
                Message = message.Message,
                CreatedAt = TurkeyTime.FromUtc(message.CreatedAt),
                IsRead = message.IsRead
            };
        }

        public async Task<bool> MarkAsReadAsync(int id)
        {
            await using var db = await _contextFactory.CreateDbContextAsync();

            var message = await db.ContactMessages
                .FirstOrDefaultAsync(m => m.Id == id);

            if (message == null)
                return false;

            message.IsRead = true;
            await db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteMessageAsync(int id)
        {
            await using var db = await _contextFactory.CreateDbContextAsync();

            var message = await db.ContactMessages
                .FirstOrDefaultAsync(m => m.Id == id);

            if (message == null)
                return false;

            db.ContactMessages.Remove(message);
            await db.SaveChangesAsync();

            return true;
        }
    }
}