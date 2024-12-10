using Banking.Control.Panel.Model;

namespace banking.control.panel.ui.controllers
{
    internal class AdminDashboard
    {
        public List<Client> Clients { get; set; }
        public string SearchText { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}