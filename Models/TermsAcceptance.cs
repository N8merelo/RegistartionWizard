using System.ComponentModel.DataAnnotations;

namespace RegistrationWizard.Models
{
    public class TermsAcceptance
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public DateTime AcceptedOn { get; set; } = DateTime.UtcNow;

        [Required]
        public string? TermsVersion { get; set; }

        [Required]
        public bool AcceptedPrivacyPolicy { get; set; }

        [Required]
        public bool AcceptedTermsOfService { get; set; }
    }
}
