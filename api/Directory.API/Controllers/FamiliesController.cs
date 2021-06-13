using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Directory.API.Models;

namespace Directory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FamiliesController : ControllerBase
    {
        private readonly DirectoryContext _context;

        public FamiliesController(DirectoryContext context)
        {
            _context = context;
        }

        // GET: api/Families
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Family>>> GetFamilies()
        {
            return await _context.Families.Include(f => f.FamilyMembers).ToListAsync();
        }

        // GET: api/Families/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Family>> GetFamily(Guid id)
        {
            var family = await _context.Families.FindAsync(id);
            if (family == null)
            {
                return NotFound();
            }

            await _context.Entry(family).Collection(f => f.FamilyMembers).LoadAsync();

            return family;
        }

        // PUT: api/Families/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFamily(Guid id, Family family)
        {
            if (id != family.FamilyId)
            {
                return BadRequest();
            }

            if (family.FamilyMembers.Any(familyMember => familyMember.FamilyId != family.FamilyId))
            {
                return BadRequest();
            }

            var familyOld = await _context.Families.FindAsync(id);
            await _context.Entry(familyOld).Collection(f => f.FamilyMembers).LoadAsync();

            familyOld.Notes = family.Notes;
            familyOld.FamilyMembers = family.FamilyMembers;
            _context.Families.Update(familyOld);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FamilyExists(id))
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

        // POST: api/Families
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Family>> PostFamily(Family family)
        {
            if (family.FamilyMembers.Any(familyMember => familyMember.FamilyId != family.FamilyId))
            {
                return BadRequest();
            }

            _context.Families.Add(family);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFamily", new {id = family.FamilyId}, family);
        }

        // DELETE: api/Families/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFamily(Guid id)
        {
            var family = await _context.Families.FindAsync(id);
            if (family == null)
            {
                return NotFound();
            }

            await _context.Entry(family).Collection(f => f.FamilyMembers).LoadAsync();

            _context.Families.Remove(family);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FamilyExists(Guid id)
        {
            return _context.Families.Any(e => e.FamilyId == id);
        }
    }
}