using Banking.Control.Panel.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Policy;
using Login = Banking.Control.Panel.UI.Models.Login;

namespace Banking.Control.Panel.UI.Controllers
{
    public class LoginController : Controller
    {
        private readonly HttpClient _httpClient;

        public LoginController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        public IActionResult login()
        {
            return View();
        }

        // Login Action
        [HttpPost]
        public async Task<ActionResult<Login>> login(Login model)
        {
            var response = await _httpClient.PostAsJsonAsync<Login>("http://localhost:5069/api/Client/Login", model);
            var token = await response.Content.ReadAsStringAsync();

            // If the token is not null, proceed with setting the token in cookies
            if (!string.IsNullOrEmpty(token))
            {
                // Store the token in a cookie with a 30-minute expiration time
                Response.Cookies.Append("JwtToken", token, new CookieOptions
                {
                    Expires = DateTime.UtcNow.AddMinutes(30)
                });

                // Parse the token to extract the role (Admin or User)
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
                var role = jsonToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

                if (role == "Admin")
                {
                    return RedirectToAction("Admindashboard", "AdminDashBoard");
                }
                else if (role == "User")
                {
                    return RedirectToAction("Userdashboard", "UserDashBoard");
                }
            }
            return View(model);
        }
    }
}