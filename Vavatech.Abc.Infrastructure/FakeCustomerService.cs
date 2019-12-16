using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Vavatech.Abc.Domain;

namespace Vavatech.Abc.Infrastructure
{

    public class FakeCustomerService : ICustomerService
    {
        private readonly ICollection<Customer> customers;

        public FakeCustomerService(Faker<Customer> customerFaker)
        {
            customers = customerFaker.Generate(100);
        }

        public void Add(Customer customer)
        {
            customers.Add(customer);
        }

        public IEnumerable<Customer> Get()
        {
            return customers;
        }

        public Customer Get(int id)
        {
            return customers.SingleOrDefault(c => c.Id == id);
        }
    }
}
