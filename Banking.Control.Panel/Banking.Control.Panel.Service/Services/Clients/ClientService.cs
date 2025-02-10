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

        public async Task<Pagination> GetClientsPagination(int pageNum, int pageSize, string sort)
        {
            var totalClientsRecord = await _applicationDbContext.Clients.CountAsync();

            var totalPages = (int)Math.Ceiling(totalClientsRecord / (double)pageSize);
            var clients = await _applicationDbContext.Clients
                .Include(e => e.Address)
                .Include(e => e.Accounts)
                .ToListAsync();


            switch (sort)
            {
                case "asc":
                    clients = clients.Skip((pageNum - 1) * pageSize)
                         .Take(pageSize)
                         .OrderBy(c => c.FirstName)
                         .ToList();
                    break;

                case "desc":
                    clients = clients.Skip((pageNum - 1) * pageSize)
                         .Take(pageSize)
                         .OrderBy(c => c.FirstName)
                         .OrderByDescending(c => c.FirstName)
                         .ToList();
                    break;

                default:
                    clients = clients
                         .Skip((pageNum - 1) * pageSize)
                         .Take(pageSize)
                         .ToList();

                    break;

            }

            var response = new Pagination
            {
                TotalRecords = totalClientsRecord,
                TotalPages = totalPages,
                CurrentPage = pageNum,
                PageSize = pageSize,
                Clients = clients
            };

            return response;

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
            var client = _applicationDbContext.Clients.Where(e => e.ClientId == id).Include(e => e.Address).Include(e => e.Accounts).FirstOrDefault();
            return client;
        }



        public async Task<Client> UpdateClient(int id, UpdateClientRequest client)
        {
            // Fetch the client from the database along with its related addresses and accounts.
            var dbClient = await _applicationDbContext.Clients
                .Where(e => e.ClientId == id)
                .Include(e => e.Address)
                .Include(e => e.Accounts)
                .FirstOrDefaultAsync();

            if (dbClient == null)
            {
                // If the client is not found, return null or handle it accordingly (e.g., NotFound).
                return null;
            }

            // Update the client details.
            dbClient.FirstName = client.FirstName;
            dbClient.LastName = client.LastName;
            dbClient.PersonalId = client.PersonalId;
            dbClient.MobileNumber = client.MobileNumber;
            dbClient.Role = client.Role;
            dbClient.Sex = client.Sex;

            if (client.Address != null)
            {
                foreach (var address in client.Address)
                {
                    // Look for existing address by comparing the address ID.
                    var existingAddress = dbClient.Address
                        .FirstOrDefault(a => a.AddressId == address.AddressId);

                    if (existingAddress != null)
                    {
                        // If the address exists, update it.
                        //existingAddress.IsActive = address.IsActive;
                        existingAddress.Country = address.Country;
                        existingAddress.City = address.City;
                        existingAddress.Street = address.Street;
                        existingAddress.ZipCode = address.ZipCode;
                        // Ensure the foreign key is correctly set
                        existingAddress.ClientId = dbClient.ClientId;

                        _applicationDbContext.Addresses.Update(existingAddress);
                    }
                    else
                    {
                        // If it's a new address, add it and ensure the foreign key is set correctly.
                        address.ClientId = dbClient.ClientId;  // Ensure the address is linked to the correct client
                        _applicationDbContext.Addresses.Add(address);
                    }
                }
            }

            // Save changes for both the client and associated addresses.
            await _applicationDbContext.SaveChangesAsync();

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
                new Claim(ClaimTypes.NameIdentifier, userAccount.ClientId.ToString())
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


