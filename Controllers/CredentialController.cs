using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.EntityFrameworkCore;
using Task1.Models;
using Task1.Data;
using Task1.Models;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Task1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CredentialController : ControllerBase
    {
        private readonly DBContext cntxt;
        private readonly IConfiguration cnfig;
        private readonly ILogger<CredentialController> lger;
        public CredentialController(DBContext context, IConfiguration configuration, ILogger<CredentialController> logger)
        {
            cntxt = context;
            cnfig = configuration;
            lger = logger;
        }

        /*
        [HttpGet]
        public async Task<ActionResult<List<Credentials>>> GetCredentials()
        {
            return await cntxt.Credential.ToListAsync();
        }

        [HttpGet("{ID}")]
        public async Task<ActionResult<Credentials>> GetCredential(int ID)
        {
            var cred = await cntxt.Credential.FindAsync(ID);
            if (cred == null)
            {
                return NotFound();
            }
            else
                return cred;
        }

        [HttpPost]
        public async Task<ActionResult<Credentials>> AddCredentials(Credentials cc)
        {
            cntxt.Credential.Add(cc);
            await cntxt.SaveChangesAsync();
            return Ok(await cntxt.Credential.ToListAsync());
        }

        [HttpPut]
        public async Task<ActionResult<Credentials>> UpdateCredentials(Credentials UpdatedCred)
        {
            var cred = await cntxt.Credential.FindAsync(UpdatedCred.ID);
            if (cred == null)
                return NotFound();
            else
            {
                cred.Name = UpdatedCred.Name;
                cred.Email = UpdatedCred.Email;
                cred.Password = UpdatedCred.Password;
                await cntxt.SaveChangesAsync();
                return Ok(await cntxt.Credential.ToListAsync());
            }
        }

        [HttpDelete]
        public async Task<ActionResult<Credentials>> DeleteCredentials(int id)
        {
            var cred = await cntxt.Credential.FindAsync(id);
            if (cred == null)
                return NotFound();
            else
            {
                cntxt.Credential.Remove(cred);
                await cntxt.SaveChangesAsync();
                return Ok(await cntxt.Credential.ToListAsync());
            }
        }
        */

        [HttpGet("Login")]
        public IActionResult Login(string Username, string Password)
        {
            var user = cntxt.Credential.SingleOrDefault(u => u.Email == Username && u.Password == Password);
            if (user != null)
            {
                lger.LogInformation("\nLogged In Successfully\n");
                var token = GenerateJwtToken(Username);
                return Ok(new { token });
            }
            return Unauthorized();
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

            /*var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };*/

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        //Employee CRUD Operations
        [HttpGet, Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<Departments>>> GetEmployees()
        {
            lger.LogInformation("\nAll Departments & Employees Included Had Been Retrieved After Departments Endpoint Was Called\n");
            return await cntxt.Department.Include(e => e.Employee).ThenInclude(e => e.Credential).ToListAsync();
        }

        [HttpGet("{id}"), Authorize]
        public async Task<ActionResult<Employees>> GetEmployee(int id)
        {
            var employee = await cntxt.Employee.Include(e => e.Credential).FirstOrDefaultAsync(e => e.ID == id);

            if (employee == null)
            {
                return NotFound();
            }

            var result = new Employees_Projection
            {
                Name = employee.Name,
                Department_Name = employee.Department_Name
            };

            lger.LogInformation("\nEmployee's Name and his Department name have been retrieved Successfully\n");

            return Ok(result);
        }

        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<ActionResult<Employees>> PostEmployee(Employees employee)
        {
            cntxt.Employee.Add(employee);
            await cntxt.SaveChangesAsync();
            lger.LogInformation("\nEmployee has been Added successfully\n");
            return CreatedAtAction(nameof(GetEmployee), new { id = employee.ID }, employee);
        }
        
        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutEmployee(int id, Employees employee)
        {
            if (id != employee.ID)
            {
                return BadRequest();
            }

            cntxt.Entry(employee).State = EntityState.Modified;

            try
            {
                await cntxt.SaveChangesAsync();
                lger.LogInformation("\nEmployee has been Updated successfully\n");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await cntxt.Employee.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            cntxt.Employee.Remove(employee);
            await cntxt.SaveChangesAsync();
            lger.LogInformation("\nEmployee has been Deleted successfully\n");
            return NoContent();
        }

        private bool EmployeeExists(int id)
        {
            return cntxt.Employee.Any(e => e.ID == id);
        }

        [HttpPost("{id}/Credentials"), Authorize]
        public async Task<ActionResult<Credential>> AddCredential(int id, Credentials credential)
        {
            var employee = await cntxt.Employee.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            credential.ID = id;
            cntxt.Credential.Add(credential);
            await cntxt.SaveChangesAsync();
            lger.LogInformation("\nEmployee's Credentials have been Added successfully\n");
            return CreatedAtAction(nameof(GetEmployee), new { id = employee.ID }, credential);
        }

        [HttpDelete("{employeeId}/Credentials/{credentialId}"), Authorize]
        public async Task<IActionResult> RemoveCredential(int employeeId, int credentialId)
        {
            var credential = await cntxt.Credential.FindAsync(credentialId);
            if (credential == null || credential.ID != employeeId)
            {
                return NotFound();
            }

            cntxt.Credential.Remove(credential);
            await cntxt.SaveChangesAsync();
            lger.LogInformation("\nEmployee's Credentials have been Deleted successfully\n");
            return NoContent();
        }
    }
}
