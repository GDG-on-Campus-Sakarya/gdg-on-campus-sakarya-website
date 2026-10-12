using System;

namespace GDG.Infrastructure.Services
{
    /// <summary>
    /// UTC ile Türkiye saati arasında çeviri yapar.
    ///
    /// NEDEN GEREKLİ?
    /// Events.Date kolonu "timestamp with time zone" tipinde. Npgsql bu kolona sadece UTC tarih
    /// yazmaya izin veriyor. Admin formundan gelen tarih ise UTC değil (kullanıcının girdiği saat).
    /// Çevirmeden kaydetmeye çalışırsak uygulama şu hatayla çöker:
    ///   "Cannot write DateTime with Kind=Unspecified to PostgreSQL type 'timestamp with time zone'"
    ///
    /// Kural: Veritabanında UTC, DTO'larda (ekranda) Türkiye saati.
    /// </summary>
    internal static class TurkeyTime
    {
        private static readonly TimeZoneInfo Zone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Istanbul");

        /// <summary>Formdan gelen Türkiye saatini veritabanı için UTC'ye çevirir.</summary>
        public static DateTime ToUtc(DateTime turkeyTime) =>
            TimeZoneInfo.ConvertTimeToUtc(DateTime.SpecifyKind(turkeyTime, DateTimeKind.Unspecified), Zone);

        /// <summary>Veritabanındaki UTC tarihi ekranda göstermek için Türkiye saatine çevirir.</summary>
        public static DateTime FromUtc(DateTime utc) =>
            TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(utc, DateTimeKind.Utc), Zone);
    }
}
