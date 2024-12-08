using Banking.Control.Panel.Model;
using Banking.Control.Panel.Service.Services.Search;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Control.Panel.Service.Services.Search
{
    public class SearchHistoryService : ISearchHistoryService
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public SearchHistoryService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        // Get the last 3 search terms for a client
        public async Task<List<string>> GetLastSearchHistory(int clientId)
        {
            // Fetch the last 3 search terms for the given client, ordered by SearchId (or another field as necessary)

            return await _applicationDbContext.SearchHistorys

                // Filter by client ID
                .Where(sh => sh.ClientId == clientId)

                // Assuming SearchId is incremental (or use another date field if necessary)
                .OrderByDescending(sh => sh.SearchId)
                // Take the last 3 records

                .Take(3)

                // Only select the SearchName property
                .Select(sh => sh.SearchName)
                .ToListAsync();
        }
    }
}

       
       