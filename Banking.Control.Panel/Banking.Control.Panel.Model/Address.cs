using Banking.Control.Panel.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

public class Address
{
    public int AddressId { get; set; }

    public string Country { get; set; }
    public string City { get; set; }
    public string? Street { get; set; }
    public string ZipCode { get; set; }

    // Foreign key for Client
    public int ClientId { get; set; }
    [JsonIgnore]
    public virtual Client? Clients { get; set; }

    [DefaultValue(true)]
    public bool IsActive { get; set; }
}
