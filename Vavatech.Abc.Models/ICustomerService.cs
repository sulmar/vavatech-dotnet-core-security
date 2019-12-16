using System;
using System.Collections.Generic;
using System.Text;

namespace Vavatech.Abc.Domain
{
    public interface ICustomerService
    {
        IEnumerable<Customer> Get();
        Customer Get(int id);
        void Add(Customer customer);
    }
}
