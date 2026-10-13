# Source review changes

This source handoff contains targeted corrections based on the latest supplied ZIP.

## Applied

- Login/logout HTTP endpoints explicitly validate antiforgery tokens instead of assuming the middleware automatically validates manually-read form data.
- Added per-IP fixed-window rate limits for login (10 requests / 5 minutes) and public contact submissions (5 requests / 10 minutes), 16 KiB request-body limits, plus a hidden honeypot field.
- Converted the public contact form to a standard POST/redirect/GET flow so HTTP endpoint rate limiting and antiforgery validation protect the actual submission. Contact submissions remain stored in PostgreSQL; no email delivery is claimed.
- Added shared server-side contact DTO validation and safe error logging.
- Hardened local redirect/external URL validation and made tests exercise the shared implementation rather than a copied function.
- Team edits now fail when a current member is unchecked without being reassigned, rather than reporting a successful update while leaving membership unchanged.
- Site settings validate names, email format, URL length and HTTP(S)-only external links; validation errors are shown as errors in the admin UI.
- The footer and contact page use persisted community/university details and only render valid external URLs. The unverified generic Discord invite was replaced with the existing official community page link.
- Admin bootstrap checks Identity operation results, fails startup when explicitly configured bootstrap cannot complete, and verifies the existing account password before promoting a non-admin account.
- The Projects CTA now uses the configured, sanitized GitHub organization URL.
- The default contact email is blank until the official address is confirmed.

## Validation not performed in this environment

The .NET SDK/CLI is not available in this editing environment, so `dotnet build` and `dotnet test` were not run here. The updated request limits, Identity bootstrap, and endpoint flows therefore still require local build and runtime validation. The source should be validated locally with:

```powershell
dotnet restore
dotnet build GDGOnCampusSakarya.sln
dotnet test GDG.Tests/GDG.Tests.csproj
```

No Supabase connection or migration was run by this edit. Review pending migrations before running `dotnet ef database update` against the shared database.

## Important deployment note

The rate limiter partitions by `HttpContext.Connection.RemoteIpAddress`. If production is behind a reverse proxy/load balancer, configure trusted forwarded headers for the actual hosting environment; do not blindly trust arbitrary `X-Forwarded-For` values.
