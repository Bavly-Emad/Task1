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
        public async Task<ActionResult<IEnumerable<Credentials>>> GetCredentials()
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
    }
}
