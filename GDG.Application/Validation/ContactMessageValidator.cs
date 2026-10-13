using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using GDG.Application.Interfaces;

namespace GDG.Application.Validation;

/// <summary>Server-side validation for contact submissions, independent of UI validation.</summary>
public static class ContactMessageValidator
{
    public static bool TryValidate(CreateContactMessageDto dto, out IReadOnlyCollection<ValidationResult> errors)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(
            dto,
            new ValidationContext(dto),
            validationResults,
            validateAllProperties: true);

        var email = dto.Email?.Trim();
        if (!string.IsNullOrWhiteSpace(email) && new EmailAddressAttribute().IsValid(email))
        {
            try
            {
                var parsed = new MailAddress(email);
                if (!string.Equals(parsed.Address, email, StringComparison.OrdinalIgnoreCase))
                {
                    validationResults.Add(new ValidationResult(
                        "Geçerli bir e-posta adresi giriniz.", new[] { nameof(dto.Email) }));
                }
            }
            catch (FormatException)
            {
                validationResults.Add(new ValidationResult(
                    "Geçerli bir e-posta adresi giriniz.", new[] { nameof(dto.Email) }));
            }
        }

        errors = validationResults;
        return isValid && validationResults.Count == 0;
    }
}
