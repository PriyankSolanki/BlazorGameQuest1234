using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedModels;

namespace GameServices.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CharactersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CharactersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Charactere>> GetAll()
        {
            return Ok(_context.Characters.ToList());
        }


        [HttpGet("{id}")]
        public ActionResult<Charactere> GetById(int id)
        {
            var c = _context.Characters.FirstOrDefault(x => x.Id == id);
            if (c == null) return NotFound();
            return Ok(c);
        }


        //post: api/characters
        
        [HttpPost]
        public ActionResult<Charactere> CreateBase(Charactere chara)
        {
            _context.Characters.Add(chara); 
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = chara.Id }, chara);
        }

        //put: api/characters/5
        [HttpPut("{id:int}")]
        public IActionResult Update(int id, Charactere chara)
        {
            var existing = _context.Characters.FirstOrDefault(x => x.Id == id);
            if (existing == null) return NotFound();

            existing.name = chara.name;
            existing.PV = chara.PV;
            existing.ATQ = chara.ATQ;

            _context.SaveChanges();
            return NoContent();
        }

        //delete: api/characters/5
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var existing = _context.Characters.FirstOrDefault(x => x.Id == id);
            if (existing == null) return NotFound();

            _context.Characters.Remove(existing);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
