using Employees_Domain.Entities;

namespace Employees_Domain.Entities
{
    public class Departments
    {
        public int ID { get; set; }
        public required string Name { get; set; }
        public ICollection<Employees> Employee { get; set; }
    }
}
