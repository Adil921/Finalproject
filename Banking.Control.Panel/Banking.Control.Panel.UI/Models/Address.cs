using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Banking.Control.Panel.UI.Models
{
    public class Address
    {
        public int AddressId { get; set; }

        public string Country { get; set; }
        public string City { get; set; }
        public string? Street { get; set; }
        public string ZipCode { get; set; }
        public bool IsActive { get; set; }
    }
}
