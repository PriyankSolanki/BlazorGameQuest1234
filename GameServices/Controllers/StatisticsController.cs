using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public StatisticsController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet("history/{playerId}")]
        public async Task<IActionResult> GetHistory(int playerId)
        {
            var history = await _db.GameSessions
                .Where(s => s.PlayerId == playerId)
                .OrderByDescending(s => s.DatePlayed)
                .ToListAsync();

            return Ok(history);
        }

        [HttpGet("leaderboard")]
        public async Task<IActionResult> GetLeaderboard()
        {
            var leaderboard = await _db.GameSessions
                .GroupBy(s => s.PlayerId)
                .Select(g => new
                {
                    PlayerId = g.Key,
                    BestScore = g.Max(x => x.Score)
                })
                .OrderByDescending(x => x.BestScore)
                .ToListAsync();

            return Ok(leaderboard);
        }
    }
}
