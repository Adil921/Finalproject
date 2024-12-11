using Banking.Control.Panel.API.Model;
using Banking.Control.Panel.Model;
using Banking.Control.Panel.Model.Model.Account;
using Banking.Control.Panel.Model.Model.Client;
using Microsoft.AspNetCore.Mvc;

namespace Banking.Control.Panel.Service
{
    public interface IClientService
    {
        public Task<List<Client>> GetAllClient(int pageNumber, int pageSize);
        public Task<Client> GetClientById(int id);
        public Task<Client> AddClient(Client client);
        public Task<Client> UpdateClient(UpdateClientRequest client);
        public Task<Client> DeleteClient(int id);
        public Task<string> Authenticate(LoginRequestModel request);
        public Task<Client> UpdateClientProfilePath(int clientId, string profilePath);
        public Task<IEnumerable<Client>> GetClients(int userId, string? searchText, string sortBy, bool ascending, int pageNumber, int pageSize);
        // public Task<ActionResult<Pagination>> GetPageData(int pageNumber, int pageSize);
        public Task<Pagination> GetClientsPagination(int pageNum, int pageSize, string? sort);



    }
}
