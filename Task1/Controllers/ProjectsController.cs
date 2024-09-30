using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Employees_Infrastructure.DataContext;
using Employees_Domain.Entities;

namespace Task1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly DBContext cntxt;
        public ProjectsController(DBContext context)
        {
            cntxt = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Projects>>> GetProjects()
        {
            return await cntxt.Project.Include(e => e.Employee).ToListAsync();
        }

        [HttpGet("{ID}")]
        public async Task<ActionResult<List<Projects>>> GetProjects(int ID)
        {
            var proj = await cntxt.Project.Include(e => e.Employee).FirstOrDefaultAsync(e => e.ID == ID);

            if (proj == null)
            {
                return BadRequest("Project Not Found");
            }

            return Ok(proj);
        }

        [HttpPost]
        public async Task<ActionResult<Projects>> PostProject(Projects proj)
        {
            cntxt.Project.Add(proj);

            await cntxt.SaveChangesAsync();

            return CreatedAtAction("GetProjects", new { id = proj.ID }, proj);
        }

        [HttpPut("{ID}")]
        public async Task<IActionResult> PutProject(int ID, Projects proj)
        {
            if(ID != proj.ID)
            {
                return BadRequest("Project Not Found");
            }
            cntxt.Entry(proj).State = EntityState.Modified;
            try
            {
                await cntxt.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                if (!ProjectExists(ID))
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

        [HttpDelete("{ID}")]
        public async Task<IActionResult> DeleteProject(int ID)
        {
            var proj = await cntxt.Project.FindAsync(ID);

            if(proj == null)
            {
                return BadRequest("Project Not Found");
            }

            cntxt.Project.Remove(proj);

            await cntxt.SaveChangesAsync();

            return NoContent();
        }

        private bool ProjectExists(int ID)
        {
            return cntxt.Project.Any(e => e.ID == ID);
        }
    }
}
