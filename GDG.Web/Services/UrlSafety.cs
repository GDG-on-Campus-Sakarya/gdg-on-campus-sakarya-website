using System;
using System.Linq;

namespace GDG.Web.Services;

/// <summary>Validates URLs before they are used for redirects or external links.</summary>
public static class UrlSafety
{
    public static bool IsLocalUrl(string? url)
    {
        if (string.IsNullOrWhiteSpace(url) || !string.Equals(url, url.Trim(), StringComparison.Ordinal))
            return false;

        if (!url.StartsWith("/", StringComparison.Ordinal) ||
            url.StartsWith("//", StringComparison.Ordinal) ||
            url.StartsWith("/\\", StringComparison.Ordinal) ||
            url.Contains('\\') || url.Any(char.IsControl))
        {
            return false;
        }

        // Also reject encoded slash/backslash sequences that can be normalized by a proxy/browser.
        string decoded;
        try
        {
            decoded = Uri.UnescapeDataString(url);
        }
        catch (UriFormatException)
        {
            return false;
        }

        if (decoded.StartsWith("//", StringComparison.Ordinal) ||
            decoded.StartsWith("/\\", StringComparison.Ordinal) ||
            decoded.Contains('\\') || decoded.Any(char.IsControl) ||
            decoded.Contains("%2f", StringComparison.OrdinalIgnoreCase) ||
            decoded.Contains("%5c", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        // The leading single slash and the checks above exclude absolute/external URL forms.
        return true;
    }

    public static string? GetSafeHttpUrl(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        if (!Uri.TryCreate(value.Trim(), UriKind.Absolute, out var uri))
            return null;

        if ((uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps) ||
            !string.IsNullOrEmpty(uri.UserInfo))
        {
            return null;
        }

        return uri.AbsoluteUri;
    }
}
