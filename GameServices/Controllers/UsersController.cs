using Microsoft.AspNetCore.Mvc;
using SharedModels;

namespace GameServices.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<User>> GetAll()
        {
            return Ok(_context.Users.ToList());
        }

        [HttpGet("{id}")]
        public ActionResult<User> GetById(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public ActionResult<User> Create(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        [HttpPost("login")]
        public ActionResult<User> Login([FromBody] User loginUser)
        {
            var user = _context.Users.FirstOrDefault(u =>
                u.Login == loginUser.Login && u.Password == loginUser.Password);

            if (user == null)
                return Unauthorized("Login ou mot de passe incorrect.");

            return Ok(user);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, User user)
        {
            var existing = _context.Users.Find(id);
            if (existing == null) return NotFound();

            existing.Login = user.Login;
            existing.Password = user.Password;
            existing.IsAdmin = user.IsAdmin;

            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null) return NotFound();

            _context.Users.Remove(user);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
