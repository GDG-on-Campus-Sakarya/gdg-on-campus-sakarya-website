using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace GDG.Application.Interfaces
{
    public interface IContactService
    {
        Task<bool> SubmitMessageAsync(CreateContactMessageDto dto);
        Task<List<ContactMessageDto>> GetAllMessagesAsync();
        Task<ContactMessageDto?> GetMessageByIdAsync(int id);
        Task<bool> MarkAsReadAsync(int id);
        Task<bool> DeleteMessageAsync(int id);
    }

    public class CreateContactMessageDto
    {
        [Required(ErrorMessage = "Ad ve soyad alanı zorunludur.")]
        [StringLength(100, ErrorMessage = "Ad ve soyad en fazla 100 karakter olabilir.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-posta adresi zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Konu başlığı zorunludur.")]
        [StringLength(100, ErrorMessage = "Konu en fazla 100 karakter olabilir.")]
        public string Subject { get; set; } = "Genel";

        [Required(ErrorMessage = "Mesaj alanı zorunludur.")]
        [StringLength(2000, ErrorMessage = "Mesaj en fazla 2000 karakter olabilir.")]
        public string Message { get; set; } = string.Empty;
    }

    public class ContactMessageDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
    }
}
