using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedModels;

namespace GameServices.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PlayersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PlayersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Player>> GetAll()
        {
            return Ok(_context.Players.ToList());
        }

        [HttpGet("{id}")]
        public ActionResult<Player> GetById(int id)
        {
            var player = _context.Players.Find(id);
            if (player == null) return NotFound();
            return Ok(player);
        }

        [HttpPost]
        public ActionResult<Player> Create(Player player)
        {
            _context.Players.Add(player);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = player.Id }, player);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Player player)
        {
            var existing = _context.Players.Find(id);
            if (existing == null) return NotFound();

            existing.name = player.name;
            existing.PV = player.PV;
            existing.ATQ = player.ATQ;
            existing.Score = player.Score;

            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var player = _context.Players.Find(id);
            if (player == null) return NotFound();

            _context.Players.Remove(player);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
