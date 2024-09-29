using System.ComponentModel.DataAnnotations;
using Employees_Domain.Entities;

namespace Employees_Domain.Entities
{
    public class Employees
    {
        public int ID { get; set; }
        public required string Name { get; set; }
        public double Salary { get; set; }
        [Display(Name = "Date Of Birth")]
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }
        public required string Project_Name { get; set; }
        public required string Department_Name { get; set; }
        public ICollection<Credentials> Credential { get; set; }
        public ICollection<Projects> Project { get; set; }
        public required string Gender { get; set; }
        public required string Marital_Status { get; set; }
        public bool Inactive { get; set; }
    }
}
