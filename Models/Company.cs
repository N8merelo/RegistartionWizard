using System.ComponentModel.DataAnnotations;

namespace RegistrationWizard.Models
{
    public class Company
    {
        public int CompanyId { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public int IndustryId { get; set; }
        public Industry? Industry { get; set; }

        public ICollection<User>? Users { get; set; }
    }
}