using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vavatech.Abc.Domain;

namespace Vavatech.Abc.Api.Controllers
{
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService customerService;

        public CustomersController(ICustomerService customerService)
        {
            this.customerService = customerService;
        }

        // GET api/customers
        [HttpGet]
        public IActionResult Get()
        {
            var customers = customerService.Get();

            return Ok(customers);
        }

        // GET api/customers/{id}
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Customer customer = customerService.Get(id);

            if (customer == null)
                return NotFound();

            return Ok(customer);
        }





    }
}
