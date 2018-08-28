using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using MongoDB.Bson;
using PRM.Models;

namespace PRM.Controllers.Api
{
    public class ExaminationController : ApiController
    {
        private readonly DataAccess _dataAccess;

        public ExaminationController()
        {
            _dataAccess = new DataAccess();
        }

        

        // GET: api/businesses
        [HttpGet]
        public Task<IEnumerable<Examination>> Get()
        {
            return GetExaminations();
        }

        private async Task<IEnumerable<Examination>> GetExaminations()
        {

            return await _dataAccess.GetAllExaminations();
        }

        // GET: api/businesses/1
        [HttpGet]
        public Task<Examination> Get(string id)
        {
            return GetExamination(id);
        }

        private async Task<Examination> GetExamination(string id)
        {
            return await _dataAccess.GetExamination(id) ?? new Examination();
        }

        // POST: api/businesses
        [HttpPost]
        public async void AddExamination(Examination Examination)
        {
            await _dataAccess.AddExamination(Examination);
        }


        //PUT api/businesses/1
        [HttpPut]
        public async void UpdateExaminations(string id, Examination Examination)
        {
            var recId = new ObjectId(id);
            await _dataAccess.UpdateExamination(recId, Examination);
        }

        //DELETE api/businesses/1
        [HttpDelete]
        public async Task<bool> RemoveExamination(string id)
        {
            return await _dataAccess.RemoveExamination(id);
        }
    }
}
