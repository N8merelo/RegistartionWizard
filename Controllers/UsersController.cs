using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegistartionWizard.Data;
using RegistartionWizard.Models;
using RegistrationWizard.Models;

namespace RegistartionWizard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly Db _db;

        public UsersController(Db db)
        {
            _db = db;
        }

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _db.Users
                .Include(u => u.Company)
                .ToListAsync();

            return Ok(users);
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _db.Users
                .Include(u => u.Company)
                .FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
                return NotFound("User not found.");

            return Ok(user);
        }
    }
}
