namespace RegistartionWizard.Models
{
    public class RegistrationRequest
    {
        // The company for the request
        public string? CompanyName { get; set; }
        public int IndustryId { get; set; }

        // The user for the request
        public string? Name { get; set; }
        public string? FirstName { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? PasswordRepeat { get; set; }
        public string? Email { get; set; }

        // the terms for the request
        public bool AcceptedTerms { get; set; }
        public bool AcceptedPrivacyPolicy { get; set; }
    }
}
