using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegistartionWizard.Data;
using RegistartionWizard.Models;
using RegistrationWizard.Models;
using System.Security.Cryptography;
using System.Text;

namespace RegistartionWizard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly Db _db;

        public RegistrationController(Db db)
        {
            _db = db;
        }

        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegistrationRequest request)
        {
            // FIELD VALIDATION
            if (string.IsNullOrWhiteSpace(request.CompanyName) || string.IsNullOrWhiteSpace(request.Name) ||
                string.IsNullOrWhiteSpace(request.FirstName) || string.IsNullOrWhiteSpace(request.Username) ||
                string.IsNullOrWhiteSpace(request.Password) || string.IsNullOrWhiteSpace(request.PasswordRepeat))
                return BadRequest("All fields have to have a value.");

            if (request.Password != request.PasswordRepeat)
                return BadRequest("Passwords do not match.");

            if (!request.AcceptedTerms || !request.AcceptedPrivacyPolicy)
                return BadRequest("The TOS must be accepted.");

            if (!await _db.Industries.AnyAsync(i => i.IndustryId == request.IndustryId))
                return BadRequest("Selected industry does not exist.");

            if (await _db.Users.AnyAsync(u => u.Username == request.Username))
                return BadRequest("Username already exists.");

            using var transaction = await _db.Database.BeginTransactionAsync();

            try
            {
                var company = new Company
                {
                    Name = request.CompanyName,
                    IndustryId = request.IndustryId
                };
                _db.Companies.Add(company);
                await _db.SaveChangesAsync();

                var user = new User
                {
                    Name = request.Name,
                    FirstName = request.FirstName,
                    Username = request.Username,
                    PasswordHash = HashPassword(request.Password),
                    Email = request.Email,
                    CompanyId = company.CompanyId
                };
                _db.Users.Add(user);
                await _db.SaveChangesAsync();

                var termsAcceptance = new TermsAcceptance
                {
                    UserId = user.UserId,
                    TermsVersion = "Version v000.1",
                    AcceptedPrivacyPolicy = request.AcceptedPrivacyPolicy,
                    AcceptedTermsOfService = request.AcceptedTerms
                };
                _db.TermsAcceptances.Add(termsAcceptance);
                await _db.SaveChangesAsync();

                await transaction.CommitAsync();
                return Ok(new { message = "Registration was successful!" });

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, $"Registration failed: {ex.Message}");
            }
        }

        // Hash the passowrd gievn
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (var b in bytes)
                    builder.Append(b.ToString("x2"));
                return builder.ToString();
            }
        }
    }
}
