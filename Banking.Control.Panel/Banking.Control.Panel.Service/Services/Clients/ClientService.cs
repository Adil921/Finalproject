using Banking.Control.Panel.API.Model;
using Banking.Control.Panel.Model;
using Banking.Control.Panel.Model.Model.Account;
using Banking.Control.Panel.Model.Model.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Banking.Control.Panel.Service
{
    public class ClientService : IClientService
    {
        public readonly ApplicationDbContext _applicationDbContext;
        private readonly IConfiguration _configuration;
        public ClientService(ApplicationDbContext applicationDbContext, IConfiguration configuration)
        {
            _configuration = configuration;
            _applicationDbContext = applicationDbContext;
        }
        public async Task<Client> AddClient(Client client)
        {
            client.Password = PasswordHashHandler.HashPassword(client.Password);
            await _applicationDbContext.Clients.AddAsync(client);
            await _applicationDbContext.SaveChangesAsync();
            return client;

        }

        public async Task<Client> DeleteClient(int id)
        {
            var client = await _applicationDbContext.Clients.FindAsync(id);
            _applicationDbContext.Clients.Remove(client);

            // Save the changes to the database
            await _applicationDbContext.SaveChangesAsync();

            // Optionally, you can return the deleted client or an appropriate result
            return client;

        }

        public async Task<List<Client>> GetAllClient(int pageNumber, int pageSize)
        {
            //var client =  await _applicationDbContext.Clients.ToListAsync();

            //  return client;
            var client = await _applicationDbContext.Clients
            .Include(e => e.Address)
            .Include(e => e.Accounts)
             .Skip((pageNumber - 1) * pageSize)
             .Take(pageSize)
                      .ToListAsync();
            return client;


        }

        public async Task<IEnumerable<Client>> GetClients(int userId, string? searchText, string sortBy, bool ascending, int pageNumber, int pageSize)
        {
            IQueryable<Client> query = _applicationDbContext.Clients;

            // Filtering
            if (!string.IsNullOrEmpty(searchText))
                query = query.Where(c => c.FirstName.Contains(searchText) || c.LastName.Contains(searchText) || c.Email.Contains(searchText));
            // Sorting
            query = ascending
                ? query.OrderBy(c => EF.Property<object>(c, sortBy))
                : query.OrderByDescending(c => EF.Property<object>(c, sortBy));

            // Pagination
            var paginatedResults = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            if (searchText != null)
            {
                var dbSearchHistory = _applicationDbContext.SearchHistorys.FirstOrDefault(c => c.SearchName.Contains(searchText) && c.ClientId == userId);
                if (dbSearchHistory == null)
                {
                    var searchHistory = new SearchHistory
                    {
                        ClientId = userId,
                        SearchName = searchText,
                    };
                    _applicationDbContext.SearchHistorys.Add(searchHistory);
                    _applicationDbContext.SaveChanges();
                }
            }
            return paginatedResults;
        }

        public async Task<Client> GetClientById(int id)
        {
            var client = await _applicationDbContext.Clients.FindAsync($"{id}");
            return client;
        }

        public Task<Client> GetPagedData()
        {
            throw new NotImplementedException();
        }

        public async Task<Client> UpdateClient(UpdateClientRequest client)
        {
            var dbClient = await _applicationDbContext.Clients.Include(e => e.Address)
           .Include(e => e.Accounts)
           .FirstOrDefaultAsync(e => e.ClientId == client.ClientId);

            if (dbClient != null)
            {
                dbClient!.FirstName = client.FirstName;
                dbClient.LastName = client.LastName;
                dbClient.PersonalId = client.PersonalId;
                dbClient.MobileNumber = client.MobileNumber;
                dbClient.Role = client.Role;
                dbClient.Sex = client.Sex;

                if (client.Address != null)
                {
                    foreach (var address in client.Address)
                    {
                        var existingAddress = dbClient.Address.FirstOrDefault(a => a.AddressId == address.AddressId);
                        if (existingAddress != null)
                        {
                            existingAddress.IsActive = address.IsActive;
                            existingAddress.Country = address.Country;
                            existingAddress.City = address.City;
                            existingAddress.Street = address.Street;
                            existingAddress.ZipCode = address.ZipCode;
                            _applicationDbContext.Addresses.Update(existingAddress);
                        }
                        else
                        {
                            _applicationDbContext.Addresses.Add(address);
                        }
                    }
                    await _applicationDbContext.SaveChangesAsync();
                }
            }
            return dbClient;
        }

        public async Task<Client> UpdateClientProfilePath(int clientId, string profilePath)
        {
            var client = await _applicationDbContext.Clients.FindAsync($"{clientId}");
            if (client != null)
            {
                client.ProfilePath = profilePath;
                await _applicationDbContext.SaveChangesAsync();

            }
            return client;
        }

        public async Task<string> Authenticate(LoginRequestModel request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                return null;
            var userAccount = await _applicationDbContext.Clients.FirstOrDefaultAsync(x => x.Email == request.Email);
            if (userAccount == null || !PasswordHashHandler.VerifyPassword(request.Password, userAccount.Password))
                return null;


            var claims = new List<Claim>
            {

                new Claim(ClaimTypes.Email, request.Email),
                new Claim(ClaimTypes.Role, userAccount.Role),
            };


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));


            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );


            return new JwtSecurityTokenHandler().WriteToken(token);

        }

    }
}

