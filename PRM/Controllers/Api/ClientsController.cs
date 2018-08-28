using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using MongoDB.Bson;
using PRM.Models;

namespace PRM.Controllers.Api
{
    public class ClientsController : ApiController
    {
        private readonly DataAccess _dataAccess;

        public ClientsController()
        {
            _dataAccess = new DataAccess();
        }

        

        // GET: api/businesses
        [HttpGet]
        public Task<IEnumerable<Client>> Get()
        {
            return GetClients();
        }

        private async Task<IEnumerable<Client>> GetClients()
        {

            return await _dataAccess.GetAllClients();
        }

        // GET: api/businesses/1
        [HttpGet]
        public Task<Client> Get(string id)
        {
            return GetClient(id);
        }

        private async Task<Client> GetClient(string id)
        {
            return await _dataAccess.GetClient(id) ?? new Client();
        }

        // POST: api/businesses
        [HttpPost]
        public async void AddClient(Client client)
        {
            await _dataAccess.AddClient(client);
        }


        //PUT api/businesses/1
        [HttpPut]
        public async void UpdateClients(string id, Client client)
        {
            var recId = new ObjectId(id);
            await _dataAccess.UpdateClient(recId, client);
        }

        //DELETE api/businesses/1
        [HttpDelete]
        public async Task<bool> RemoveClient(string id)
        {
            return await _dataAccess.RemoveClient(id);
        }
    }
}
