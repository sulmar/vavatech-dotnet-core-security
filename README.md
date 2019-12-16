# Uwierzytelnianie
## Basic
## OAuth 2.0
## JWT
# Autoryzacja
## Role
## Claim
## Policy
# Zapobieganie atakom
## SQL injection
## Cross-site scripting (XSS)
## Cross-site Request Forgery (CSRF)
## Server-side Request Forgery (SSRF)
## Ukrywanie informacji

~~~ csharp
public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel(c=>c.AddServerHeader = false)
                .UseStartup<Startup>();
~~~

# Szyfrowanie
## Klucze symetryczne
## Klucze asymetryczne
