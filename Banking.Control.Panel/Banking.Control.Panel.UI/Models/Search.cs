using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Banking.Control.Panel.UI.Models
{
    public class Search
    {
        public int SearchId { get; set; }
        public string? SearchName { get; set; }

        public int ClientId { get; set; }

    }
}