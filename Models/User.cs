using System.ComponentModel.DataAnnotations;

namespace RegistrationWizard.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? Username { get; set; }

        [Required]
        public string? PasswordHash { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public string? Phone { get; set; }
        public string? Role { get; set; }

        public int CompanyId { get; set; }
        public Company? Company { get; set; }
    }
}