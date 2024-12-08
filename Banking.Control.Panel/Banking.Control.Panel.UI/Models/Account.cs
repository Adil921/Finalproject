using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Banking.Control.Panel.UI.Models
{
    public class Account
    {
        public int AccountId { get; set; }

        public int ClientId { get; set; }
        public int AccountNumber { get; set; }
        public string AccountType { get; set; }
        public double? Balance { get; set; }
        public bool IsActive { get; set; }
    }
}

