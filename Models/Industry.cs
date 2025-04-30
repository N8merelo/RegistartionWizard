using System.ComponentModel.DataAnnotations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace RegistrationWizard.Models
{
    public class Industry
    {
        public int IndustryId { get; set; }

        [Required]
        public string? Name { get; set; }

        public ICollection<Company>? Companies { get; set; }
    }
}