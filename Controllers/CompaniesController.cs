using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegistartionWizard.Data;
using RegistartionWizard.Models;
using RegistrationWizard.Models;
using RegistrationWizard.Dtos;

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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetCompanies()
        {
            var companies = await _db.Companies
                .Include(c => c.Industry)
                .Select(c => new CompanyDto
                {
                    CompanyId = c.CompanyId,
                    Name = c.Name,
                    IndustryId = c.IndustryId,
                    IndustryName = c.Industry.Name
                })
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
