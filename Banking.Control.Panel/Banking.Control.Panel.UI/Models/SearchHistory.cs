using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Banking.Control.Panel.Model;

namespace Banking.Control.Panel.UI.Models
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
