using Banking.Control.Panel.API.Model;
using Banking.Control.Panel.Model;
using Banking.Control.Panel.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;
using System.Security.Policy;
using Client = Banking.Control.Panel.UI.Models.Client;

namespace banking.control.panel.ui.controllers
{
    public class AdminDashBoardController : Controller
    {
        private readonly HttpClient _httpclient;

        public AdminDashBoardController(HttpClient httpclient)
        {
            _httpclient = httpclient;
        }

        [HttpGet]
        public async Task<IActionResult> AdminDashboard(int pageNum = 1, int pageSize = 10, string sort = "null")
        {
            try
            {
                //string? searchText = null;
                //string sortBy = "FirstName";
                //bool ascending = true;
                //int pageNumber = 1;
                //int pageSize = 10;
                // Construct the URL of your API
                //var apiUrl = $"http://localhost:5069/api/Client?userId={userId}&searchText={searchText}&sortBy={sortBy}&ascending={ascending}&pageNumber={pageNumber}&pageSize={pageSize}";
                var apiUrl = $"http://localhost:5069/api/Client/Pagination?pageNum={pageNum}&pageSize={pageSize}&sort={sort}";
                // Send GET request to your API
                var response = await _httpclient.GetFromJsonAsync<Banking.Control.Panel.UI.Models.Pagination>(apiUrl);

                // Check if the response is successful
                if (response != null)
                {
                    // Parse the response body to a list of Client objects
                    //var responseContent = await response.Content.ReadAsStringAsync();
                    //var clients = _httpclient.GetFromJsonAsync<List<Client>>(responseContent);

                    // return view with the clients and pagination data
                    var viewmodel = new Banking.Control.Panel.UI.Models.Pagination
                    {
                        Clients = response.Clients.Where(e => e.Role == "User").ToList(),
                        CurrentPage = response.CurrentPage,
                        PageSize = response.PageSize,
                        TotalPages = response.TotalPages,
                    };

                    // Return the data to the view
                    return View(viewmodel);
                }
                else
                {
                    // Handle failure response (e.g., NotFound)
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                //return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> AdminDashboard(int userId = 1, string? searchText = null, string sortBy = "FirstName", bool ascending = true, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                // Construct the URL of your API
                var apiUrl = $"http://localhost:5069/api/Client?userId={userId}&searchText={searchText}&sortBy={sortBy}&ascending={ascending}&pageNumber={pageNumber}&pageSize={pageSize}";

                // Send GET request to your API
                var response = await _httpclient.GetFromJsonAsync<Banking.Control.Panel.UI.Models.Pagination>(apiUrl);

                // Check if the response is successful
                if (response != null)
                {
                    // Parse the response body to a list of Client objects
                    //var responseContent = await response.Content.ReadAsStringAsync();
                    //var clients = _httpclient.GetFromJsonAsync<List<Client>>(responseContent);

                    // return view with the clients and pagination data
                    var viewmodel = new Banking.Control.Panel.UI.Models.Pagination
                    {
                        Clients = response.Clients,
                        CurrentPage = response.CurrentPage,
                        PageSize = response.PageSize,
                        TotalPages = response.TotalPages,
                    };

                    // Return the data to the view
                    return View(viewmodel);
                }
                else
                {
                    // Handle failure response (e.g., NotFound)
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                //return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
                return View();
            }

        }

        [HttpGet]
        public async Task<IActionResult> AddClient()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddClient(Registration model)
        {
            try
            {
                if (ModelState.IsValid)
                {

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
                    var response = await _httpclient.PostAsJsonAsync("http://localhost:5069/api/Client/Register", client);

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        ModelState.AddModelError(string.Empty, $"Registration failed: {errorContent}");
                        return View(model); // Show the form again with an error message
                    }
                    else
                    {
                        TempData["SuccessMessage"] = "Client registered successfully!";
                        return RedirectToAction("Index"); // Redirect to success page after successful registration
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
