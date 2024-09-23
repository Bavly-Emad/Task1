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
using Task1.Services;

namespace Task1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CredentialController : ControllerBase
    {
        private readonly EmployeeServices empls;
        private readonly DBContext cntxt;
        public CredentialController(EmployeeServices employeeServices, DBContext db)
        {
            empls = employeeServices;
            cntxt = db;
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
            var user = empls.LoginAsync(Username, Password);
            if (user != null)
            {
                return Ok(new { user });
            }
            return Unauthorized();
        }

        //Employee CRUD Operations
        [HttpGet, Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<Departments>>> GetEmployees()
        {
            return await empls.GetEmployeesAsync();
        }

        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<Employees>>> SearchEmployee([FromQuery] string name, [FromQuery] double? salary, [FromQuery] string projectname, [FromQuery] DateOnly? dateofbirth)
        {
            var query = cntxt.Employee.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(e => e.Salary == salary.Value);
            }

            if (!string.IsNullOrEmpty(projectname))
            {
                query = query.Where(e => e.EP.Any(ep => ep.Project.Name.Contains(projectname)));
            }

            if(dateofbirth.HasValue)
            {
                query = query.Where(e => e.DateOfBirth == dateofbirth.Value);
            }

            //var empl = await query.Include(e => e.Credential).Include(e => e.EP).ThenInclude(ep => ep.Project).ToListAsync();

            var empl = await query.Include(e => e.Credential).ToListAsync();

            return empl;
        }

        [HttpGet("{id}"), Authorize]
        public async Task<ActionResult<Employees>> GetEmployee(int id)
        {
            var employee = await empls.GetEmployeeAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            var result = new Employees_Projection
            {
                Name = employee.Name,
                Department_Name = employee.Department_Name
            };

            return Ok(result);
        }

        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<ActionResult<Employees>> PostEmployee(Employees employee)
        {
            empls.PostEmployeeAsync(employee);
            return CreatedAtAction(nameof(GetEmployee), new { id = employee.ID }, employee);
        }
        
        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutEmployee(int id, Employees employee)
        {
            if (id != employee.ID)
            {
                return BadRequest();
            }

            empls.PutEmployeeAsync(employee);

            return NoContent();
        }

        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await empls.DeleteEmployeeAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            return NoContent();
        }
        /*
        [HttpPost("{id}/Credentials"), Authorize]
        public async Task<ActionResult<Credential>> AddCredential(int id, Credentials credential)
        {
            var employee = await empls.AddCredentialAsync(credential);
            if (employee == null)
            {
                return NotFound();
            }

            credential.ID = id;

            return CreatedAtAction(nameof(GetEmployee), new { id }, credential);
        }

        [HttpDelete("{employeeId}/Credentials/{credentialId}"), Authorize]
        public async Task<IActionResult> RemoveCredential(int employeeId, int credentialId)
        {
            var credential = await empls.RemoveCredentialAsync(credentialId);
            if (credential == null || credential.ID != employeeId)
            {
                return NotFound();
            }

            return NoContent();
        }*/
    }
}
