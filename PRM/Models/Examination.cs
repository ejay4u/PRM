using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PRM.Models
{
    public class Examination
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Category { get; set; }
        public string ExaminationType { get; set; }
        public string InternalClient { get; set; }
        public string DirectService { get; set; }
        public string CooperateClient { get; set; }
        public string CtAdult { get; set; }
        public string CtChildren { get; set; }

        [BsonElement("Status")]
        [BsonDefaultValue(1)]
        public bool Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime TimeCreated { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime TimeUpdated { get; set; }
    }
}