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
    public class PeopleController : ControllerBase
    {
        private readonly DirectoryContext _context;

        public PeopleController(DirectoryContext context)
        {
            _context = context;
        }

        // GET: api/People
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> GetPeople()
        {
            return await _context.People.Include(p => p.FamilyRoles).ToListAsync();
        }

        // GET: api/People/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> GetPerson(Guid id)
        {
            var person = await _context.People.FindAsync(id);

            if (person == null)
            {
                return NotFound();
            }

            await _context.Entry(person).Collection(p => p.FamilyRoles).LoadAsync();

            return person;
        }

        // PUT: api/People/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPerson(Guid id, Person person)
        {
            if (id != person.PersonId)
            {
                return BadRequest();
            }

            if (person.FamilyRoles.Any(familyPerson => familyPerson.PersonId != person.PersonId))
            {
                return BadRequest();
            }

            var personOld = await _context.People.FindAsync(id);
            await _context.Entry(personOld).Collection(p => p.FamilyRoles).LoadAsync();

            personOld.Notes = person.Notes;
            personOld.EmailAddress = person.EmailAddress;
            personOld.Name = person.Name;
            personOld.PhoneNumber = person.PhoneNumber;
            personOld.FamilyRoles = person.FamilyRoles;
            _context.People.Update(personOld);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(id))
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

        // POST: api/People
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Person>> PostPerson(Person person)
        {
            if (person.FamilyRoles.Any(familyPerson => familyPerson.PersonId != person.PersonId))
            {
                return BadRequest();
            }

            _context.People.Add(person);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPerson", new {id = person.PersonId}, person);
        }

        // DELETE: api/People/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(Guid id)
        {
            var person = await _context.People.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }

            await _context.Entry(person).Collection(p => p.FamilyRoles).LoadAsync();

            _context.People.Remove(person);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PersonExists(Guid id)
        {
            return _context.People.Any(e => e.PersonId == id);
        }
    }
}