using System.ComponentModel.DataAnnotations;
using Employees_Domain.Entities;

namespace Employees_Domain.Entities
{
    public class Credentials
    {
        public int ID { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public bool IsAdmin { get; set; }
    }

}
