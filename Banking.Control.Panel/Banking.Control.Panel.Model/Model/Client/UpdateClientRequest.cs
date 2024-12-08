using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Banking.Control.Panel.Model.Model.Client
{
    public class UpdateClientRequest
    {
        public int ClientId { get; set; }

        [Required]
        [MaxLength(59)]
        public string? FirstName { get; set; }

        [Required]
        [MaxLength(59)]
        public string? LastName { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        [Required]
        [StringLength(11, MinimumLength = 11)]
        public string PersonalId { get; set; }

        [Required]
        [StringLength(11, MinimumLength = 11)]
        public string? MobileNumber { get; set; }

        [Required]
        public string? Sex { get; set; } // "Male" or "Female"

        // One-to-Many Relationship with Address
        public virtual ICollection<Address?> Address { get; set; }
        // One-to-Many Relationship with Account
        public string? Role { get; set; } // "Admin" or "User"

    }
}
