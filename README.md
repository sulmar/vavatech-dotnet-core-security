# Uwierzytelnianie
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
## JWT
# Autoryzacja
## Role
## Claim
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
