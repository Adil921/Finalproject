using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Banking.Control.Panel.Model
{
    public class SearchHistory
    {
        [Key]
        public int SearchId { get; set; }
        public string? SearchName { get; set; }
        [ForeignKey("ClientId")]
        public int ClientId { get; set; }
        [JsonIgnore]
        public virtual Client? Client { get; set; }
    }
}
