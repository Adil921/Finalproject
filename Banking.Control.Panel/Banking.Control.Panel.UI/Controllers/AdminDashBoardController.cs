﻿using Banking.Control.Panel.API.Model;
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
        public async Task<IActionResult> AdminDashboard(int pageNum = 1, int pageSize = 10, string? sort = null)
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
                var apiUrl = $"http://localhost:5069/api/Client/Pagination?pageNum={pageNum}&pageSize={pageSize}";
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

    }
}