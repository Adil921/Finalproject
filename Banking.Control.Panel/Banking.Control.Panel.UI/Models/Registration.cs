using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace Banking.Control.Panel.UI.Models
{
    public class Registration
    {
        public int ClientId { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(59, ErrorMessage = "First name should be less than 60 characters")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        [StringLength(59, ErrorMessage = "Last name should be less than 60 characters")]
        public string? LastName { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [StringLength(11, MinimumLength = 11)]
        public string PersonalId { get; set; }

        //public string ProfilePath { get; set; }

        [Required]
        [StringLength(14, MinimumLength = 14)]
        [RegularExpression(@"^\+92\s3\d{9}$", ErrorMessage = "Mobile number must start with '+92', followed by a space, and then '3' followed by 9 digits.")]
        public string? MobileNumber { get; set; }

        [Required]
        public string Sex { get; set; } // "Male" or "Female"
        public string Country { get; set; }
        public string City { get; set; }
        public string? Street { get; set; }
        public string? ZipCode { get; set; }
        //public string AccountNumber { get; set; }
        //public decimal Balance { get; set; }
        //public string AccountType { get; set; }

    }
}
