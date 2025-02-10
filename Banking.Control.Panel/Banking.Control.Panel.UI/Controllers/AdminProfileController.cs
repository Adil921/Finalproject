using Azure;
using Banking.Control.Panel.Model;
using Banking.Control.Panel.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using NuGet.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Policy;
using Client = Banking.Control.Panel.UI.Models.Client;


namespace Banking.Control.Panel.UI.Controllers
{
    public class AdminProfileController : Controller
    {
        private readonly HttpClient _httpClient;

        public string UpdateURL = "http://localhost:5069/api/Client/";

        // Constructor that injects HttpClient
        public AdminProfileController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        // GET: UserProfile/Details/{id}
        [HttpGet]
        public async Task<IActionResult> Adminprofile(int id)
        {
            var token = Request.Cookies["JwtToken"];

            if (string.IsNullOrEmpty(token))
            {
                // If token is missing or empty, return Unauthorized response
                return Unauthorized();
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // Set the Authorization header with Bearer token
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var userId = jwtToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var user = await _httpClient.GetFromJsonAsync<Banking.Control.Panel.UI.Models.Client>("http://localhost:5069/api/Client/" + userId);

            if (user == null)
            {
                return NotFound();
            }

            var response = new UpdateClientRequest
            {
                ClientId = user.ClientId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PersonalId = user.PersonalId,
                MobileNumber = user.MobileNumber,
                Sex = user.Sex,
                Role = user.Role,
                // Ensure address fields are properly populated, even if empty
                Address = user.Address?.Select(a => new Banking.Control.Panel.UI.Models.Address
                {
                    AddressId = a.AddressId,
                    Country = a.Country,
                    City = a.City,
                    Street = a.Street,
                    ZipCode = a.ZipCode
                }).FirstOrDefault()
            };

            // Set the sex and role in ViewData to retain the selected value in dropdowns
            ViewData["Sex"] = user.Sex;
            ViewData["Role"] = user.Role;

            return View(response);
        }


        [HttpPost]
        public async Task<IActionResult> Adminprofile(UpdateClientRequest updateClient)
        {
            var token = Request.Cookies["JwtToken"];

            if (string.IsNullOrEmpty(token))
            {
                // If token is missing or empty, return Unauthorized response
                return Unauthorized();
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // Set the Authorization header with Bearer token
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var userId = jwtToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            List<Banking.Control.Panel.Model.Model.Client.Address> LocalAddress = new List<Banking.Control.Panel.Model.Model.Client.Address>();
            LocalAddress.Add(new Banking.Control.Panel.Model.Model.Client.Address
            {
                AddressId = updateClient.Address.AddressId,
                Country = updateClient.Address.Country,
                City = updateClient.Address.City,
                Street = updateClient.Address.Street,
                ZipCode = updateClient.Address.ZipCode,
            });
            // Prepare the client data for update
            var UpdateClient = new Banking.Control.Panel.Model.Model.Client.UpdateClientRequest
            {
                ClientId = Convert.ToInt32(userId),
                FirstName = updateClient.FirstName,
                LastName = updateClient.LastName,
                Email = updateClient.Email,
                PersonalId = updateClient.PersonalId,
                MobileNumber = updateClient.MobileNumber,
                Sex = updateClient.Sex,
                Role = updateClient.Role,
                Address = LocalAddress  // Ensure the address is correctly mapped
            };

            // Send the PUT request to the API for client update
            var response = await _httpClient.PutAsJsonAsync(UpdateURL + userId, UpdateClient);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Client Updated Successfully";
                return RedirectToAction("AdminDashBoard", "AdminDashboard");
            }
            else
            {
                TempData["ErrorMessage"] = "Something Went Wrong: " + response.ReasonPhrase;
                return View(updateClient);
            }
        }

    }
}
