using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegistartionWizard.Data;
using RegistrationWizard.Models;

namespace RegistartionWizard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IndustryController : ControllerBase
    {
        private readonly Db _db;

        public IndustryController(Db db)
        {
            _db = db;
        }

        // GET: api/industry
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Industry>>> GetIndustries()
        {
            var industries = await _db.Industries.ToListAsync();
            return Ok(industries);
        }
    }
}
