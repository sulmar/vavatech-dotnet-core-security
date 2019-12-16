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
