using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using MongoDB.Bson;
using PRM.Models;
using PRM.ViewModels;

namespace PRM.Controllers.Api
{
    public class LabRequestsController : ApiController
    {
        private readonly DataAccess _dataAccess;

        public LabRequestsController()
        {
            _dataAccess = new DataAccess();
        }

        // GET: api/LabRequests
        [HttpGet]
        public Task<IEnumerable<LabRequest>> GetByType(string type)
        {
            return GetLabRequests(type);
        }

        private async Task<IEnumerable<LabRequest>> GetLabRequests(string type)
        {

            return await _dataAccess.GetAllLabRequests(type);
        }

        // GET: api/LabRequests/1
        [HttpGet]
        public Task<LabRequest> Get(string id)
        {
            return GetLabRequest(id);
        }

        private async Task<LabRequest> GetLabRequest(string id)
        {
            return await _dataAccess.GetLabRequest(id) ?? new LabRequest();
        }

        // POST: api/LabRequests
        [HttpPost]
        public async void AddLabRequest(LabViewModel LabRequestViewModel)
        {
            await _dataAccess.AddLabRequest(LabRequestViewModel);
        }


        //PUT api/LabRequests/1
        [HttpPut]
        public async void UpdateLabRequest(string id, LabRequest LabRequest)
        {
            var recId = new ObjectId(id);
            await _dataAccess.UpdateLabRequest(recId, LabRequest);
        }

        //DELETE api/LabRequests/1
        [HttpDelete]
        public async Task<bool> RemoveLabRequest(string id)
        {
            return await _dataAccess.RemoveLabRequest(id);
        }
    }
}
