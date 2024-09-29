using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Employees_Domain.Entities;
using Microsoft.Extensions.Configuration;
using Employees_Infrastructure.DataContext;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;

namespace Employees_Application.Services
{
    public class EmployeeServices
    {
        private readonly DBContext cntxt;
        private readonly IConfiguration cnfig;
        private readonly ILogger<EmployeeServices> lger;

        public EmployeeServices(DBContext DBContext, IConfiguration configuration, ILogger<EmployeeServices> logger)
        {
            cntxt = DBContext;
            cnfig = configuration;
            lger = logger;
        }

        public string LoginAsync(string Username, string Password)
        {
            var user = cntxt.Credential.SingleOrDefault(u => u.Email == Username && u.Password == Password);
            if (user != null)
            {
                lger.LogInformation("\nLogged In Successfully\n");
                var token = GenerateJwtToken(Username);
                return token;
            }
            return "Unauthorized Login";
        }

        private string GenerateJwtToken(string username)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username)
            };

            var user = cntxt.Credential.FirstOrDefault(c => c.Email == username);

            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(cnfig.GetSection("AppSettings:Token").Value));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            if (user.IsAdmin)
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public async Task<IEnumerable<Employees>> SearchEmployeeAsync([FromQuery] string name, [FromQuery] double? salary, [FromQuery] string projectname, [FromQuery] DateOnly? dateofbirth)
        {
            /*var query = cntxt.Employee.SingleOrDefault(u => u.Name == name && u.Salary == salary && u.Project_Name == projectname && u.DateOfBirth == dateofbirth);

            return Ok(query);*/

            var query = cntxt.Employee.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(e => e.Name.Contains(name));
            }

            if (salary.HasValue)
            {
                query = query.Where(e => e.Salary == salary.Value);
            }

            if (!string.IsNullOrEmpty(projectname))
            {
                query = query.Where(e => e.Project.Any(p => p.Name.Contains(projectname)));
            }

            if (dateofbirth.HasValue)
            {
                query = query.Where(e => e.DateOfBirth == dateofbirth.Value);
            }

            return await query.Include(e => e.Project).ToListAsync();
        }

        public async Task<List<Departments>> GetEmployeesAsync()
        {
            lger.LogInformation("\nAll Departments & Employees Included Had Been Retrieved After Departments Endpoint Was Called\n");
            return await cntxt.Department.Include(e => e.Employee).ThenInclude(e => e.Credential).ToListAsync();
        }

        public async Task<Employees?> GetEmployeeAsync(int id)
        {
            return await cntxt.Employee.Include(e => e.Credential).FirstOrDefaultAsync(e => e.ID == id);
        }

        public async Task<Employees> PostEmployeeAsync(Employees employee)
        {
            cntxt.Employee.Add(employee);
            await cntxt.SaveChangesAsync();
            lger.LogInformation("\nEmployee has been Added successfully\n");
            return employee;
        }

        public async Task<bool> PutEmployeeAsync(Employees employee)
        {
            cntxt.Entry(employee).State = EntityState.Modified;
            try
            {
                await cntxt.SaveChangesAsync();
                lger.LogInformation("\nEmployee has been Updated successfully\n");
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await EmployeeExistsAsync(employee.ID))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            var employee = await cntxt.Employee.FindAsync(id);
            if (employee == null)
            {
                return false;
            }

            cntxt.Employee.Remove(employee);
            await cntxt.SaveChangesAsync();
            return true;
        }

        private async Task<bool> EmployeeExistsAsync(int id)
        {
            return await cntxt.Employee.AnyAsync(e => e.ID == id);
        }
        
        public async Task<int> AddCredentialAsync(Credentials credential)
        {
            cntxt.Credential.Add(credential);
            await cntxt.SaveChangesAsync();
            lger.LogInformation("\nEmployee's Credentials have been Added successfully\n");
            return credential.ID;
        }

        public async Task<int?> RemoveCredentialAsync(int credentialId)
        {
            var credential = await cntxt.Credential.FindAsync(credentialId);
            if (credential == null)
            {
                return null;
            }
            cntxt.Credential.Remove(credential);
            await cntxt.SaveChangesAsync();
            lger.LogInformation("\nEmployee's Credentials have been Deleted successfully\n");
            return credential.ID;
        }
    }
}
