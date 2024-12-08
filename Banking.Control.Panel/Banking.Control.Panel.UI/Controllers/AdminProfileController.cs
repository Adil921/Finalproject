using Microsoft.AspNetCore.Mvc;

namespace Banking.Control.Panel.UI.Controllers
{
    public class AdminProfileController : Controller
    {
        public IActionResult Adminprofile()
        {
            return View();
        }
    }
}
