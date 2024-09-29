namespace Employees_Domain.Entities
{
    public class Projects
    {
        public int ID { get; set; }
        public required string Name { get; set; }
        public ICollection<Employees> Employee { get; set; }
    }
}
