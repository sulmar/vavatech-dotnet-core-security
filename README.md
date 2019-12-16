# Uwierzytelnianie
Weryfikacja tożsamości użytkownika żądającego dostępu do aplikacji.

## Basic

Headers 

| Key   | Value  |
|---|---|
| Authorization | Basic {Base64(login:password)}  |


### Utworzenie uchwytu

~~~ csharp

 public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IUsersService usersService;
        
        private const string authorizationKey = "Authorization";

        public BasicAuthenticationHandler(
            IUsersService usersService,
            IOptionsMonitor<AuthenticationSchemeOptions> options, 
            ILoggerFactory logger, 
            UrlEncoder encoder, 
            ISystemClock clock) : base(options, logger, encoder, clock)
        {
            this.usersService = usersService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {

            if (!Request.Headers.ContainsKey(authorizationKey))
            {
                return AuthenticateResult.Fail("Missing authorization header");
            }

            var authHeader = AuthenticationHeaderValue.Parse(Request.Headers[authorizationKey]);
            var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
            var credentials = Encoding.UTF8.GetString(credentialBytes).Split(":");

            var username = credentials[0];
            var password = credentials[1];

            if (!usersService.TryAuthenticate(username, password, out Customer customer))
            {
                return AuthenticateResult.Fail("Invalid username or password");
            }

            IIdentity identity = new ClaimsIdentity(Scheme.Name);
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);

        }
    }
~~~

#### Rejestracja
Startup.cs

~~~ csharp
 public void ConfigureServices(IServiceCollection services)
{

    services.AddAuthentication("Basic")
        .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("Basic", null);
  }
  
 public void Configure(IApplicationBuilder app, IHostingEnvironment env)
  {
      app.UseAuthentication();
      app.UseMvc();
    }

~~~

## OAuth 2.0

Headers 

| Key   | Value  |
|---|---|
| Authorization | Bearer {token}  |


## JWT

Headers 

| Key   | Value  |
|---|---|
| Authorization | Bearer {jwt_token}  |

https://github.com/sulmar/dotnet-core-jwt

# Autoryzacja
Weryfikacja dostępu do zasobów aplikacji.

## Role


~~~ csharp
 [Authorize(Roles = "Developer, Admin")]
 [Route("api/[controller]")]
 [ApiController]
 public class CustomersController : ControllerBase
 {
 }
~~~

~~~ csharp
[HttpGet]
public IActionResult Get()
{
    if (this.User.IsInRole("Manager"))
    {
        var customers = customerRepository.Get();
        return Ok(customers);
    }
    else
    {
        var customers = customerRepository.GetByUserName(this.User.Name);
        return Ok(customers);
    }
}
~~~

## Claim
~~~ csharp
ClaimsIdentity identity = new ClaimsIdentity("Basic");

 identity.AddClaim(new Claim(ClaimTypes.HomePhone, "555-444-333"));
 identity.AddClaim(new Claim(ClaimTypes.Role, "Developer")); 
 identity.AddClaim(new Claim(ClaimTypes.Role, "Trainer")); 

 ClaimsPrincipal principal = new ClaimsPrincipal(identity);
~~~

## Policy

# Zapobieganie atakom
## SQL injection

### Podatność
~~~ csharp
// api/customers/salary/customername=John'%20or%20'1'='1
public IEnumerable<decimal> GetSalary(string customerName)
{
string query = "SELECT salary FROM dbo.Customers WHERE Name= '" + customerName + "'";

ICollection<decimal> result = new List<decimal>();

using (SqlCommand command = new SqlCommand(query, connection))
{
    connection.Open();
    using (SqlDataReader reader = command.ExecuteReader())
    {
        while (reader.Read())
        {
            result.Add(Convert(reader));
        }
    }
    connection.Close();
}

return result;
}
~~~


### Rozwiązanie

~~~ csharp
public IEnumerable<decimal> GetSalary(string customerName)
{
  const string query = "SELECT salary FROM dbo.Customers WHERE Name= @customerName";

  ICollection<decimal> result = new List<decimal>();

  using (SqlCommand command = new SqlCommand(query, connection))
  {
    command.Parameters.AddWithValue("@customerName", customerName);

    connection.Open();
    using (SqlDataReader reader = command.ExecuteReader())
    {


        while (reader.Read())
        {
            result.Add(Convert(reader));
        }
    }
    connection.Close();
  }
  
  return result;
}
~~~



## Cross-site scripting (XSS)
## Cross-site Request Forgery (CSRF)
## Server-side Request Forgery (SSRF)

## Clickjacking 

### Atak
Atak polega na zagnieżdżaniu naszej strony w ramkach.

źródła:
https://www.keycdn.com/blog/x-frame-options
https://niebezpiecznik.pl/post/x-frame-options-zacznij-stosowac/

### Rozwiązanie

Dodanie nagłówka do odpowiedzi w celu zablokowania ramek.

~~~
X-Frame-Options: deny - tylko strony z tej samej domeny mogą stosować ramki
X-Frame-Options: sameorigin - żadna strona nie może wrzucić w ramkę tej strony
~~~

~~~ csharp
public void Configuration(IAppBuilder app)
{
    app.UseXfo(options => options.SameOrigin());
}
~~~

## Sanityzacja

HtmlSanitizer
https://github.com/mganss/HtmlSanitizer

## Ukrywanie informacji

~~~ csharp
public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel(c=>c.AddServerHeader = false)
                .UseStartup<Startup>();
~~~

# Szyfrowanie
## Klucze symetryczne

### AES

~~~ csharp
public byte[] Encrypt(string plainText, byte[] Key, byte[] IV)
        {
            using (SymmetricAlgorithm algorithm = new AesCryptoServiceProvider())
            {
                ICryptoTransform encryptor = algorithm.CreateEncryptor(Key, IV);
                using (MemoryStream stream = new MemoryStream())
                using (CryptoStream cryptoStream = new CryptoStream(stream, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter sw = new StreamWriter(cryptoStream))
                    {
                        sw.Write(plainText);
                    }

                    return stream.ToArray();
                }
            }
        }

        public string Decrypt(byte[] cipherText, byte[] Key, byte[] IV)
        {
            using (SymmetricAlgorithm algorithm = new AesCryptoServiceProvider())
            {
                ICryptoTransform decryptor = algorithm.CreateDecryptor(Key, IV);

                using (MemoryStream ms = new MemoryStream(cipherText))
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (StreamReader reader = new StreamReader(cs))
                {
                    return reader.ReadToEnd();
                }
            }
        }
~~~

## Klucze asymetryczne
