using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Policy;
using Banking.Control.Panel.UI.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
namespace Banking.Control.Panel.UI.Controllers
{
    public class UpdateClientController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IWebHostEnvironment _ihostingenvironment;
        public string UpdateURL = "http://localhost:5069/api/Client/";
        public UpdateClientController(HttpClient httpClient, IWebHostEnvironment ihostingenvironment)
        {
            _httpClient = httpClient;
            _ihostingenvironment = ihostingenvironment;
        }
        [Route("Client/Updateclient/{id}")]
        [HttpGet("{id}")]
        public async Task<ActionResult> Updateclient(int id)

        {
            try
            {
                // Set authorization header with the token (Admin should be logged in)
                var token = Request.Cookies["JwtToken"];
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized();
                }

                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                // Get client data using the clientId passed to the URL
                var response = await _httpClient.GetFromJsonAsync<Banking.Control.Panel.UI.Models.Client>("http://localhost:5069/api/Client/" + id);

                if (response == null)
                {
                    return NotFound();
                }

                var res = new UpdateClientRequest
                {
                    ClientId = response.ClientId,
                    FirstName = response.FirstName,
                    LastName = response.LastName,
                    Email = response.Email,
                    PersonalId = response.PersonalId,
                    MobileNumber = response.MobileNumber,
                    Sex = response.Sex,
                    Role = response.Role,
                    // Ensure address fields are properly populated, even if empty
                    Address = response.Address?.Select(a => new Banking.Control.Panel.UI.Models.Address
                    {
                        AddressId = a.AddressId,
                        Country = a.Country,
                        City = a.City,
                        Street = a.Street,
                        ZipCode = a.ZipCode
                    }).FirstOrDefault()

                };

                // Set the sex and role in ViewData to retain the selected value in dropdowns
                ViewData["Sex"] = response.Sex;
                ViewData["Role"] = response.Role;

                return View(res);
            }
            catch (Exception ex)
            {
                // Error retrieving client data.
                ViewData["Error"] = ex.Message;
                return View();
            }
        }
        [Route("Client/Updateclient/{id}")]
        [HttpPost]
        public async Task<ActionResult> Updateclient(UpdateClientRequest updateClient)
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
                return RedirectToAction("UserDashboard", "UserDashBoard");
            }
            else
            {
                TempData["ErrorMessage"] = "Something Went Wrong: " + response.ReasonPhrase;
                return View(updateClient);
            }
        }

    }



}
