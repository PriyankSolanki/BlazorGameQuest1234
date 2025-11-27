using Microsoft.AspNetCore.Mvc;
using SharedModels;

namespace GameServices.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SavesController : ControllerBase
    {
        private readonly AppDbContext _db;

        public SavesController(AppDbContext db)
        {
            _db = db;
        }

      
        [HttpGet]
        public ActionResult<IEnumerable<GameSave>> GetAll()
        {
            var saves = _db.GameSaves.OrderByDescending(s => s.Date).ToList();
            return Ok(saves);
        }

      
        [HttpPost]
        public ActionResult<GameSave> Save([FromBody] GameSave save)
        {
            if (save is null) return BadRequest();

            save.Date = DateTime.Now;
            _db.GameSaves.Add(save);
            _db.SaveChanges();

            return Ok(save);
        }

        
        [HttpGet("{id}")]
        public ActionResult<GameSave> GetById(int id)
        {
            var save = _db.GameSaves.FirstOrDefault(s => s.Id == id);
            return save is null ? NotFound() : Ok(save);
        }
    }
}
