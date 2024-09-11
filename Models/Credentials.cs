using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;

namespace Task1.Models
{
    public class Credentials
    {
        public int ID { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

    public class Employees
    {
        public int ID { get; set; }
        public required string Name { get; set; }
        public ICollection<Credentials> Credential { get; set; }
    }
}
