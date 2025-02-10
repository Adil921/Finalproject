using Banking.Control.Panel.Model;
using Banking.Control.Panel.Service.Services.Search;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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
        //public async Task<ActionResult<List<Client>>> FilterClient(string name)
        //{
        //    var client = await _applicationDbContext.Clients
        //        .Where(e => e.FirstName == name)
        //        .Include(e => e.Address)
        //        .Include(e => e.Accounts)
        //        .Where(e => e.Accounts!.Any())
        //        .Select(e => new Client
        //        {
        //            ClientId = e.ClientId,
        //            FirstName = e.FirstName,
        //            LastName = e.LastName,
        //            PersonalId = e.PersonalId,
        //            ProfilePath = e.ProfilePath,
        //            MobileNumber = e.MobileNumber,
        //            Sex = e.Sex,
        //            Address = e.Address.Select(a => new Banking.Control.Panel.Model.Model.Client.Address
        //            {
        //                AddressId = a.AddressId,
        //                Country = a.Country,
        //                City = a.City,
        //                Street = a.Street,
        //                ZipCode = a.ZipCode
        //            }).ToList(), // Convert the result to a list
        //        })
        //        .ToListAsync();

        //    // Log search results into Search table
        //    if (client.Count != 0)
        //    {
        //        var searchs = new SearchHistory
        //        {
        //            SearchName = client.Select(e => e.FirstName).FirstOrDefault(),
        //            ClientId = client.Select(e => e.ClientId).FirstOrDefault(),
        //        };
        //        await _applicationDbContext.AddAsync(searchs);
        //    }

        //    return client!;
        //}
        public async Task<ActionResult<List<Client>>> FilterClient(string name)
        {
            var client = await _applicationDbContext.Clients
      .Where(e => e.FirstName == name)
      .Include(e => e.Address)
      .Include(e => e.Accounts)
      .Where(e => e.Accounts!.Any())
      .Select(e => new Client
      {
          ClientId = e.ClientId,
          FirstName = e.FirstName,
          LastName = e.LastName,
          PersonalId = e.PersonalId,
          ProfilePath = e.ProfilePath,
          MobileNumber = e.MobileNumber,
          Sex = e.Sex,
          Address = e.Address.Select(a => new Banking.Control.Panel.Model.Model.Client.Address
          {
              AddressId = a.AddressId,
              Country = a.Country,
              City = a.City,
              Street = a.Street,
              ZipCode = a.ZipCode
          }).ToList(),
      })
      .ToListAsync();

            return client!;
        }
    }
}



