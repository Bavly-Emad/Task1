using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using System.ComponentModel.DataAnnotations;

namespace Task1.Models
{
    public class Credentials
    {
        public int ID { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public bool IsAdmin { get; set; }
    }

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
        public ICollection<EmployeeProject> EP { get; set; }
    }

    public class Employees_Projection
    {
        public required string Name { get; set; }
        public required string Department_Name { get; set; }
    }

    public class Departments
    {
        public int ID { get; set; }
        public required string Name { get; set; }
        public ICollection<Employees> Employee { get; set; }
    }

    public class Projects
    {
        public int ID { get; set; }
        public required string Name { get; set; }
        public ICollection<EmployeeProject> EP { get; set; }
    }

    public class EmployeeProject
    {
        public int EmployeeID { get; set; }
        public Employees Employee { get; set; }
        public int ProjectID { get; set; }
        public Projects Project { get; set; }
    }
}
