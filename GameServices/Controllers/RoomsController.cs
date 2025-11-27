using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedModels;
using Microsoft.EntityFrameworkCore;

namespace GameServices.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RoomsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RoomsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Room>> GetAll()
        {
            return Ok(_context.Rooms
                .Include(r => r.Player)
                .Include(r => r.Ennemie)
                .ToList());
        }

        [HttpGet("{id}")]
        public ActionResult<Room> GetById(int id)
        {
            var room = _context.Rooms
                .Include(r => r.Player)
                .Include(r => r.Ennemie)
                .FirstOrDefault(r => r.Id == id);

            if (room == null) return NotFound();
            return Ok(room);
        }

        [HttpPost]
        public ActionResult<Room> Create(Room room)
        {
            _context.Rooms.Add(room);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = room.Id }, room);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var room = _context.Rooms.Find(id);
            if (room == null) return NotFound();

            _context.Rooms.Remove(room);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
