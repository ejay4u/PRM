using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using MongoDB.Bson;
using PRM.Models;

namespace PRM.Controllers.Api
{
    public class DoctorsController : ApiController
    {
        private readonly DataAccess _dataAccess;

        public DoctorsController()
        {
            _dataAccess = new DataAccess();
        }

        

        // GET: api/businesses
        [HttpGet]
        public Task<IEnumerable<Doctor>> Get()
        {
            return GetDoctors();
        }

        private async Task<IEnumerable<Doctor>> GetDoctors()
        {

            return await _dataAccess.GetAllDoctors();
        }

        // GET: api/businesses/1
        [HttpGet]
        public Task<Doctor> Get(string id)
        {
            return GetDoctor(id);
        }

        private async Task<Doctor> GetDoctor(string id)
        {
            return await _dataAccess.GetDoctor(id) ?? new Doctor();
        }

        // POST: api/businesses
        [HttpPost]
        public async void AddDoctor(Doctor Doctor)
        {
            await _dataAccess.AddDoctor(Doctor);
        }


        //PUT api/businesses/1
        [HttpPut]
        public async void UpdateDoctors(string id, Doctor Doctor)
        {
            var recId = new ObjectId(id);
            await _dataAccess.UpdateDoctor(recId, Doctor);
        }

        //DELETE api/businesses/1
        [HttpDelete]
        public async Task<bool> RemoveDoctor(string id)
        {
            return await _dataAccess.RemoveDoctor(id);
        }
    }
}
