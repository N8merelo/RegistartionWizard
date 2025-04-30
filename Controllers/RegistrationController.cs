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
        public async Task<IActionResult> Register([FromBody] RegistrationRequest request)
        {
            // FIelds avlidation
            if (string.IsNullOrWhiteSpace(request.CompanyName) || string.IsNullOrWhiteSpace(request.Name) ||
                string.IsNullOrWhiteSpace(request.FirstName) || string.IsNullOrWhiteSpace(request.Username) ||
                string.IsNullOrWhiteSpace(request.Password) || string.IsNullOrWhiteSpace(request.PasswordRepeat))
                return BadRequest("All fields have to have a value.");

            // password & confrim password
            if (request.Password != request.PasswordRepeat)
                return BadRequest("Passwords do not match.");

            // tos validation
            if (!request.AcceptedTerms || !request.AcceptedPrivacyPolicy)
                return BadRequest("The TOS must be accepted.");

            // username must be unique
            bool usernameExists = await _db.Users.AnyAsync(u => u.Username == request.Username);
            if (usernameExists)
                return BadRequest("Username already exists.");


            var industryExists = await _db.Industries.AnyAsync(i => i.IndustryId == request.IndustryId);
            if (!industryExists)
                return BadRequest("Selected industry does not exist.Please check available industries in api/Industry link.");

            // add the company
            var company = new Company {
                Name = request.CompanyName,
                IndustryId = request.IndustryId
            };
            _db.Companies.Add(company);
            await _db.SaveChangesAsync();

            // add the user
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

            // save tos
            var termsAcceptance = new TermsAcceptance
            {
                UserId = user.UserId,
                TermsVersion = "Version v000.1", // Hardcoded now, can be dynamic
                AcceptedPrivacyPolicy = request.AcceptedPrivacyPolicy,
                AcceptedTermsOfService = request.AcceptedTerms
            };
            _db.TermsAcceptances.Add(termsAcceptance);

            await _db.SaveChangesAsync();

            return Ok("Registration was successful!");
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
