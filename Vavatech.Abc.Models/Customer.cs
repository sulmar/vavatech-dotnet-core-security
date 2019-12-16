using System;

namespace Vavatech.Abc.Domain
{
    public abstract class Base
    {
    }

    public abstract class BaseEntity : Base
    {
        public int Id { get; set; }
    }

    public class Customer : BaseEntity
    {
        public string Name { get; set; }
        public decimal Salary { get; set; }
        public string Username { get; set; }
        public string HashPassword { get; set; }
    }
}
