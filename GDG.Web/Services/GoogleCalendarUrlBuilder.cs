using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using GDG.Web.Models;

namespace GDG.Web.Services;

public static class GoogleCalendarUrlBuilder
{
    /// <summary>
    /// Etkinlik bilgileri önceden doldurulmuş Google Calendar
    /// oluşturma bağlantısı üretir.
    /// </summary>
    public static string Build(EventViewModel item)
    {
        ArgumentNullException.ThrowIfNull(item);

        // Etkinlik tarihleri Türkiye yerel saati olarak tutuluyor.
        // Saatin sunucunun saat dilimine göre dönüştürülmesini önle.
        var start = DateTime.SpecifyKind(
            item.Date,
            DateTimeKind.Unspecified);

        // Etkinliklerde bitiş saati tanımlı olmadığı için
        // varsayılan süreyi 1 saat kabul ediyoruz.
        var end = start.AddHours(1);

        var dateRange =
            $"{FormatCalendarDate(start)}/{FormatCalendarDate(end)}";

        var parameters = new Dictionary<string, string>
        {
            ["action"] = "TEMPLATE",
            ["text"] = item.Title ?? string.Empty,
            ["dates"] = dateRange,
            ["details"] = item.Description ?? string.Empty,
            ["location"] = item.Location ?? string.Empty,
            ["ctz"] = "Europe/Istanbul"
        };

        var query = string.Join(
            "&",
            parameters.Select(parameter =>
                $"{Uri.EscapeDataString(parameter.Key)}=" +
                $"{Uri.EscapeDataString(parameter.Value)}"));

        return $"https://calendar.google.com/calendar/render?{query}";
    }

    private static string FormatCalendarDate(DateTime date)
    {
        return date.ToString(
            "yyyyMMdd'T'HHmmss",
            CultureInfo.InvariantCulture);
    }
}