using Microsoft.EntityFrameworkCore;
using Task1.Models;
namespace Task1.Data
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options) { }

        public DbSet<Credentials> Credential { get; set; }

        public DbSet<Employees> Employee { get; set; }

    }
}
