using Bogus;
using Vavatech.Abc.Domain;

namespace Vavatech.Abc.Infrastructure
{
    // dotnet add package Bogus
    public class CustomerFaker : Faker<Customer>
    {
        public CustomerFaker()
        {
            RuleFor(p => p.Id, f => f.IndexFaker);
            RuleFor(p => p.Name, f => f.Company.CompanyName());
            RuleFor(p => p.Salary, f => f.Finance.Amount(100, 1000));
            RuleFor(p => p.Username, f => f.Internet.UserName());
            RuleFor(p => p.HashPassword, f => f.Internet.Password());
        }
    }
}
