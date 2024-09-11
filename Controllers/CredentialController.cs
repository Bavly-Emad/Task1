using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task1.Models;
using Task1.Data;
using Task1.Models;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;

namespace Task1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CredentialController : ControllerBase
    {
        private readonly DBContext cntxt;
        public CredentialController(DBContext context)
        {
            cntxt = context;
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

        //Employee CRUD Operations
        [HttpGet]
        public async Task<ActionResult<List<Employees>>> GetEmployees()
        {
            return await cntxt.Employee.Include(e => e.Credential).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employees>> GetEmployee(int id)
        {
            var employee = await cntxt.Employee.Include(e => e.Credential).FirstOrDefaultAsync(e => e.ID == id);

            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }

        [HttpPost]
        public async Task<ActionResult<Employees>> PostEmployee(Employees employee)
        {
            cntxt.Employee.Add(employee);
            await cntxt.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmployee), new { id = employee.ID }, employee);
        }
        
        [HttpPut("{id}")]
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await cntxt.Employee.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            cntxt.Employee.Remove(employee);
            await cntxt.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeExists(int id)
        {
            return cntxt.Employee.Any(e => e.ID == id);
        }

        [HttpPost("{id}/Credentials")]
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

            return CreatedAtAction(nameof(GetEmployee), new { id = employee.ID }, credential);
        }

        [HttpDelete("{employeeId}/Credentials/{credentialId}")]
        public async Task<IActionResult> RemoveCredential(int employeeId, int credentialId)
        {
            var credential = await cntxt.Credential.FindAsync(credentialId);
            if (credential == null || credential.ID != employeeId)
            {
                return NotFound();
            }

            cntxt.Credential.Remove(credential);
            await cntxt.SaveChangesAsync();

            return NoContent();
        }
    }
}
