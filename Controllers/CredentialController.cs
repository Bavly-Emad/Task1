using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task1.Models;
using Task1.Data;
using Task1.Models;

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
    }
}
