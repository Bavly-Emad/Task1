using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;

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
        public required string Department_Name { get; set; }
        public ICollection<Credentials> Credential { get; set; }
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
}
