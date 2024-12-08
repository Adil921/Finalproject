using Banking.Control.Panel.Model;
using Banking.Control.Panel.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;
using System.Security.Policy;

namespace banking.control.panel.ui.controllers
{
    public class Admindashboardcontroller : Controller
    {
        private readonly HttpClient _httpclient;

        public Admindashboardcontroller(HttpClient httpclient)
        {
            _httpclient = httpclient;
        }

        public async Task<IActionResult> AdminDashBoard(int userId = 1, string? searchText = null, string sortBy = "FirstName", bool ascending = true, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                // Construct the URL of your API
                var apiUrl = $"http://localhost:5069/api/Client?userId={userId}&searchText={searchText}&sortBy={sortBy}&ascending={ascending}&pageNumber={pageNumber}&pageSize={pageSize}";

                // Send GET request to your API
                var response = await _httpclient.GetAsync(apiUrl);

                // Check if the response is successful
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body to a list of Client objects
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var clients = JsonConvert.DeserializeObject<List<Banking.Control.Panel.Model.Client>>(responseContent);

                    // return view with the clients and pagination data
                    var viewmodel = new AdminDashBoard
                    {
                        Clients = clients,
                        SearchText = searchText,
                        PageNumber = pageNumber,
                        PageSize = pageSize
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
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }


        //public async Task<IActionResult> AdminDashboard(int pagenumber = 1, int pagesize = 10, string searchtext = "", string sortby = "firstname", bool ascending = true)
        //{

        //    // get paginated, sorted, and filtered client list
        //    var response = await _httpclient.GetFromJsonAsync<Client>("http://localhost:5069/api/Client/");
        //    var clients = await _httpclient.GetFromJsonAsync<IEnumerable<Client>>(1, searchtext, sortby, ascending, pagenumber, pagesize);

        //    //// return view with the clients and pagination data
        //    //var viewmodel = new clientviewmodel
        //    //{
        //    //    clients = clients,
        //    //    searchtext = searchtext,
        //    //    pagenumber = pagenumber,
        //    //    pagesize = pagesize
        //    //};

        //    return View(response);
        //}



    }
}
