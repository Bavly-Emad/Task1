using Microsoft.EntityFrameworkCore;
using Task1.Models;
namespace Task1.Data
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options) { }

        public DbSet<Credentials> Credential { get; set; }

        public DbSet<Employees> Employee { get; set; }

        public DbSet<Departments> Department { get; set; }

        public DbSet<Projects> Project { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employees>().Property(a => a.DateOfBirth).HasColumnType("date");
            modelBuilder.Entity<Employees>().Property(e => e.DateOfBirth).HasConversion(v => v.ToDateTime(TimeOnly.MinValue), v => DateOnly.FromDateTime(v));

            modelBuilder.Entity<Employees>().HasMany(e => e.Project).WithMany(p => p.Employee).UsingEntity<Dictionary<string, object>>
            (
                "EmployeeProject",
                j => j.HasOne<Projects>().WithMany().HasForeignKey("ProjectId"),
                j => j.HasOne<Employees>().WithMany().HasForeignKey("EmployeeId")
            );

            /*
            modelBuilder.Entity<Employees>().HasMany(e => e.Project).WithMany(p => p.Employee).UsingEntity<Dictionary<string, object>>
                (
                    "EmployeeProject",
                    j => j.HasOne<Projects>().WithMany().HasForeignKey("ProjectId"),
                    j => j.HasOne<Employees>().WithMany().HasForeignKey("EmployeeId")
                );
            */
            /*
            modelBuilder.Entity<EmployeeProject>().HasKey(p => new { p.EmployeeID, p.ProjectID });
            modelBuilder.Entity<EmployeeProject>().HasOne(a => a.Employee).WithMany(b => b.EP).HasForeignKey(c => c.EmployeeID);
            modelBuilder.Entity<EmployeeProject>().HasOne(a => a.Project).WithMany(b => b.EP).HasForeignKey(c => c.ProjectID);
            modelBuilder.Entity<Employees>().HasMany(a => a.Project).WithMany(b => b.Employee).UsingEntity<Dictionary<string, object>>(
                "EmployeeProject", c => c.HasOne<Projects>().WithMany().HasForeignKey("ID"), c => c.HasOne<Employees>().WithMany().HasForeignKey("ID")
                );*/
        }
    }
}
