using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using MongoDB.Driver;
using PRM.Models;

namespace PRM
{
    public class MongoContext
    {
        private readonly IMongoDatabase _database;
        public MongoContext()        //constructor   
        {
            var connectionString = ConfigurationManager.AppSettings.Get("MONGOLAB_URI_TEST");
            var url = new MongoUrl(connectionString);
            var client = new MongoClient(url);
            _database = client.GetDatabase(url.DatabaseName);

            // Reading credentials from Web.config file   
            /*var mongoDatabaseName = ConfigurationManager.AppSettings["MongoDatabaseName"]; //CarDatabase  
            var mongoUsername = ConfigurationManager.AppSettings["MongoUsername"]; //demouser  
            var mongoPassword = ConfigurationManager.AppSettings["MongoPassword"]; //Pass@123  
            var mongoPort = ConfigurationManager.AppSettings["MongoPort"];  //27017  
            var mongoHost = ConfigurationManager.AppSettings["MongoHost"];  //localhost  

            // Creating credentials  
            var credential = MongoCredential.CreateMongoCRCredential
                            (mongoDatabaseName,
                             mongoUsername,
                             mongoPassword);

            //// Creating MongoClientSettings  
            var settings = new MongoClientSettings
            {
                Credentials = new[] { credential },
                Server = new MongoServerAddress(mongoHost, Convert.ToInt32(mongoPort))
            };
            var client = new MongoClient(settings);
            _database = client.GetDatabase("PRM");*/
        }

        public IMongoCollection<Client> Clients
        {
            get
            {
                return _database.GetCollection<Client>("Client");
            }
        }

        public IMongoCollection<Doctor> Doctors
        {
            get
            {
                return _database.GetCollection<Doctor>("Doctor");
            }
        }

        public IMongoCollection<LabRequest> LabRequests
        {
            get
            {
                return _database.GetCollection<LabRequest>("LabRequest");
            }
        }

        public IMongoCollection<Examination> Examinations
        {
            get
            {
                return _database.GetCollection<Examination>("Examination");
            }
        }

        public IMongoCollection<CampaignCode> CampaignCodes
        {
            get
            {
                return _database.GetCollection<CampaignCode>("CampaignCode");
            }
        }

        public IMongoCollection<Models.PassItOn> PassItOns
        {
            get
            {
                return _database.GetCollection<Models.PassItOn>("PassItOn");
            }
        }

        public IMongoCollection<AdInfo> AdInfos
        {
            get
            {
                return _database.GetCollection<AdInfo>("AdInfo");
            }
        }

        public IMongoCollection<CodeCard> CodeCard
        {
            get
            {
                return _database.GetCollection<CodeCard>("CodeCard");
            }
        }

        public IMongoCollection<CodeFailure> CodeFailure
        {
            get
            {
                return _database.GetCollection<CodeFailure>("CodeFailure");
            }
        }
    }
}