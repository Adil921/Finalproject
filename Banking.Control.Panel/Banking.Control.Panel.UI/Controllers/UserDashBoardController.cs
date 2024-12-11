using Azure.Core;
using Banking.Control.Panel.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Policy;

namespace Banking.Control.Panel.UI.Controllers
{
    public class UserDashBoardController : Controller
    {
        private readonly HttpClient _httpClient;

        // Constructor that injects HttpClient
        public UserDashBoardController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        public async Task<ActionResult> Userdashboard()
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
                // var userEmail = jwtToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                var Client = jwtToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                // Get client data using the clientId passed to the URL
                // var response = await _httpClient.GetFromJsonAsync<Client>(URL + userId);
                var clients = await _httpClient.GetFromJsonAsync<List<Client>>("");
                if (clients != null)
                {
                    var filterClient = clients.Where(e => e.ClientId.ToString() == Client).FirstOrDefault();
                    // Populate the view with client data
                    ViewData["ClientId"] = filterClient.ClientId;
                    ViewData["Email"] = filterClient.Email;
                    ViewData["FirstName"] = filterClient.FirstName;
                    ViewData["LastName"] = filterClient.LastName;
                    ViewData["Mobile"] = filterClient.MobileNumber;
                    ViewData["PersonalId"] = filterClient.PersonalId;
                    ViewData["Sex"] = filterClient.Sex.ToString();
                    //TempData["ProfilePath"] = filterClient.ProfilePhoto;
                    //ViewData["Country"] = filterClient.Address!.Country;
                    //ViewData["City"] = filterClient.Address.City;
                    //ViewData["Street"] = filterClient.Address.Street;
                    //ViewData["ZipCode"] = filterClient.Address.ZipCode;
                    //ViewBag.Accounts = filterClient.Account;
                }
                else
                {
                    ViewData["Error"] = "No client data found.";
                }
                return View();
            }
            catch (Exception ex)
            {
                // Error retrieving client data.
                ViewData["Error"] = ex.Message;
                return View();
            }
        }
    }
}

  