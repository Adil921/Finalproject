using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Security.Claims;

namespace Banking.Control.Panel.UI.Controllers
{
    public class UserDashBoardController : Controller
    {
        public IActionResult Userdashboard()
        {
            //var userId = jwtToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            return View();
        }
    }
}
