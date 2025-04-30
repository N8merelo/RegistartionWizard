using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegistartionWizard.Data;
using RegistartionWizard.Models;
using RegistrationWizard.Models;

namespace RegistartionWizard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly Db _db;

        public CompaniesController(Db db)
        {
            _db = db;
        }

        // GET: api/companies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Company>>> GetCompanies()
        {
            var companies = await _db.Companies
                .Include(c => c.Industry)
                .Include(c => c.Users)
                .ToListAsync();

            return Ok(companies);
        }

        // GET: api/companies/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Company>> GetCompany(int id)
        {
            var company = await _db.Companies
                .Include(c => c.Industry)
                .Include(c => c.Users)
                .FirstOrDefaultAsync(c => c.CompanyId == id);

            if (company == null)
                return NotFound("Company not found.");

            return Ok(company);
        }
    }
}
