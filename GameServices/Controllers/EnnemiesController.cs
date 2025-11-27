using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedModels;

namespace GameServices.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EnnemiesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EnnemiesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Ennemie>> GetAll()
        {
            return Ok(_context.Ennemies.ToList());
        }

        [HttpGet("{id}")]
        public ActionResult<Ennemie> GetById(int id)
        {
            var ennemie = _context.Ennemies.Find(id);
            if (ennemie == null) return NotFound();
            return Ok(ennemie);
        }

        [HttpPost]
        public ActionResult<Ennemie> Create(Ennemie ennemie)
        {
            _context.Ennemies.Add(ennemie);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = ennemie.Id }, ennemie);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Ennemie ennemie)
        {
            var existing = _context.Ennemies.Find(id);
            if (existing == null) return NotFound();

            existing.name = ennemie.name;
            existing.PV = ennemie.PV;
            existing.ATQ = ennemie.ATQ;

            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var ennemie = _context.Ennemies.Find(id);
            if (ennemie == null) return NotFound();

            _context.Ennemies.Remove(ennemie);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
