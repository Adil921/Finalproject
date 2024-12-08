using Microsoft.AspNetCore.Mvc;

namespace Banking.Control.Panel.UI.Controllers
{
    public class UpdateClientController : Controller
    {
        private readonly HttpClient _httpClient;

        public UpdateClientController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpPut]
        public ActionResult UpdateClient ()
        {
            return View();
        }
    }
}
