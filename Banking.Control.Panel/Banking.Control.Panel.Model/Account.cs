using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Banking.Control.Panel.Model
{
    public class Account
    {
        public int AccountId { get; set; }
        

        // Foreign key for Client
        [ForeignKey("ClientId")]
        public int ClientId { get; set; }
        public int AccountNumber { get; set; }
        public string AccountType { get; set; }
        public double? Balance { get; set; }
        [JsonIgnore]
        public virtual Client? Client { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; }
    }
}
