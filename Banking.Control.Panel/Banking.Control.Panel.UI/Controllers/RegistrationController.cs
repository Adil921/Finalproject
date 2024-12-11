using Banking.Control.Panel.UI.Models;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Security.Policy;
using System.Text;

namespace Banking.Control.Panel.UI.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly HttpClient _httpClient;

        // Constructor that injects HttpClient
        public RegistrationController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            var model = new Registration();
            return View(model);
        }

        // POST: /Account/Register
        [HttpPost]
        public async Task<IActionResult> Register(Registration model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Process the profile photo (if any)
                    //string profilePath = null;
                    //if (model.ProfilePhoto != null)
                    //{
                    //    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", model.ProfilePhoto.FileName);
                    //    using (var stream = new FileStream(filePath, FileMode.Create))
                    //    {
                    //        await model.ProfilePhoto.CopyToAsync(stream);
                    //    }
                    //    profilePath = "/uploads/" + model.ProfilePhoto.FileName; // Save the path for storage
                    //}

                    //Create the Client object to send to the API
                    var addresses = new List<Banking.Control.Panel.UI.Models.Address>();
                    var address = new Banking.Control.Panel.UI.Models.Address
                    {
                        Country = model.Country,
                        City = model.City,
                        Street = model.Street,
                        ZipCode = model.ZipCode
                    };
                    addresses.Add(address);


                    var client = new Client
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        Password = model.Password,
                        PersonalId = model.PersonalId,
                        //ProfilePath = profilePath,
                        MobileNumber = model.MobileNumber,
                        Sex = model.Sex,
                        Role = model.Role,
                        IsActive = true,
                        Address = addresses,

                    };

                    // Send the constructed client data to the API
                    var response = await _httpClient.PostAsJsonAsync("http://localhost:5069/api/Client/Register", client);

                    if (response.IsSuccessStatusCode)
                    {
                        // Successful registration
                        TempData["SuccessMessage"] = "Registration successful. Please log in.";
                        return RedirectToAction("login", "Login");
                    }
                    else
                    {
                        // Handle failure response from the API
                        var errorMessage = await response.Content.ReadAsStringAsync();
                        TempData["ErrorMessage"] = $"Registration failed: {errorMessage}";
                        return View(model);
                    }
                }

                // If ModelState is not valid, return the model to the view for validation feedback
                return View(model);
            }
            catch (Exception ex)
            {
                // Log the exception (or handle it as needed)
                TempData["ErrorMessage"] = "An unexpected error occurred. Please try again.";
                return View(model);
            }
        }

        
    }
}
