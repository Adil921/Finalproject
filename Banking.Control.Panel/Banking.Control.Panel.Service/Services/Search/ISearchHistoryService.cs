using Banking.Control.Panel.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Control.Panel.Service.Services.Search
{

    public interface ISearchHistoryService
    {
        //public Task<List<string>> GetLastSearchHistory(int clientId);
        public Task<ActionResult<List<Client>>> FilterClient(string name);

    }
}