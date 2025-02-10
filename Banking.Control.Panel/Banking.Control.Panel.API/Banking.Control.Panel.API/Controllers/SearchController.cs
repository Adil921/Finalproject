using Banking.Control.Panel.Service.Services.Search;
using Microsoft.AspNetCore.Mvc;
namespace Banking.Control.Panel.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly ISearchHistoryService _search;

        public SearchController(ISearchHistoryService search)
        {
            _search = search;
        }

        // Endpoint to get the last 3 search terms for a specific client

        [HttpGet("GetLastSearchHistory")]
        public async Task<IActionResult> GetLastSearchHistory(string name)
        {
            try
            {
                // Attempt to retrieve the last 3 search terms for the specified client
                var searchTerms = await _search.FilterClient(name);

                // If no search terms are found, return a NotFound response with a message
                if (searchTerms.Value == null)
                {
                    return NotFound("No search history available for this client.");
                }

                // Return the search terms with a 200 OK status if found
                return Ok(searchTerms.Value);
            }
            catch (Exception)
            {
                // If an exception occurs, return a 500 Internal Server Error response
                // with a generic error message (without exposing internal details)
                return StatusCode(500, "An unexpected error occurred while processing your request.");
            }
        }
    }
}

