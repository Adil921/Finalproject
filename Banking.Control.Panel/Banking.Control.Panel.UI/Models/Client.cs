using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Net;
using System.Text.Json.Serialization;
namespace Banking.Control.Panel.UI.Models
{
    public class Client
    {
        public int ClientId { get; set; }

        [Required]
        [MaxLength(59)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(59)]
        public string LastName { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [StringLength(11, MinimumLength = 11)]
        public string PersonalId { get; set; }
        public string? ProfilePath { get; set; }

        [Required]
        [StringLength(14, MinimumLength = 14)]
        [RegularExpression(@"^\+92\s3\d{9}$", ErrorMessage = "Mobile number must start with '+92', followed by a space, and then '3' followed by 9 digits.")]
        public string? MobileNumber { get; set; }

        [Required]
        public string Sex { get; set; } // "Male" or "Female"

        //One-to-Many Relationship with Address
        public List<Address> Address { get; set; }

        //One-to-Many Relationship with Account
        //public List<Account>? Accounts { get; set; }
        public string Role { get; set; } // "Admin" or "User"

        [DefaultValue(true)]
        public bool IsActive { get; set; }
    }
}

