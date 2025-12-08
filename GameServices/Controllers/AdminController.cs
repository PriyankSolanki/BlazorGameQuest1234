using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedModels;

namespace GameServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _db;

        public AdminController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet("players")]
        public async Task<IActionResult> GetPlayers()
        {
            return Ok(await _db.Players.ToListAsync());
        }

        [HttpPost("players/{id}/toggle")]
        public async Task<IActionResult> TogglePlayer(int id)
        {
            var p = await _db.Players.FindAsync(id);
            if (p == null) return NotFound();

            p.IsActive = !p.IsActive;
            await _db.SaveChangesAsync();

            return Ok(p);
        }
        [HttpPost("players/add")]
        public async Task<IActionResult> AddPlayer([FromBody] PlayerCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Username))
                return BadRequest("Username is required.");

            var newPlayer = new Player
            {
                Username = dto.Username,
                Name = dto.Username,  // Nom du personnage = username (modifiable plus tard si besoin)
                PV = 100,
                ATQ = 10,
                Score = 0,
                IsActive = true
            };

            _db.Players.Add(newPlayer);
            await _db.SaveChangesAsync();

            return Ok(newPlayer);
        }

        public class PlayerCreateDto
        {
            public string Username { get; set; } = string.Empty;
        }


        [HttpGet("scores")]
        public async Task<IActionResult> GetScores()
        {
            return Ok(await _db.GameSessions.ToListAsync());
        }

        [HttpGet("sessions")]
        public async Task<IActionResult> GetSessions()
        {
            return Ok(await _db.GameSessions.ToListAsync());
        }

        [HttpGet("export")]
        public async Task<IActionResult> ExportPlayers()
        {
            var players = await _db.Players.ToListAsync();
            var csv = "Id,Username,IsActive\n" +
                      string.Join("\n", players.Select(p => $"{p.Id},{p.Username},{p.IsActive}"));

            return File(System.Text.Encoding.UTF8.GetBytes(csv),
                "text/csv", "players.csv");
        }
    }

}
