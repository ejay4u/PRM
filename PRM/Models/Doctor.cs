using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PRM.ViewModels;

namespace PRM.Models
{
    public class Doctor
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("DoctorId")]
        public string DoctorId { get; set; }

        [BsonRequired]
        [BsonElement("Surname")]
        public string Surname { get; set; }

        [BsonRequired]
        [BsonElement("OtherName")]
        public string OtherName { get; set; }

        [BsonRequired]
        [BsonElement("DateOfBirth")]
        public string DateOfBirth { get; set; }

        [BsonRequired]
        [BsonElement("Gender")]
        public string Gender { get; set; }

        [BsonElement("Address")]
        public string Address { get; set; }

        [BsonElement("PhoneNumber")]
        public string PhoneNumber { get; set; }

        [BsonElement("Email")]
        public string Email { get; set; }

        [BsonElement("Status")]
        [BsonDefaultValue(1)]
        public bool Status { get; set; }

        [BsonElement("CreatedBy")]
        public string CreatedBy { get; set; }

        [BsonElement("TimeCreated")]
        [BsonDateTimeOptions]
        public DateTime TimeCreated { get; set; }

        [BsonElement("UpdatedBy")]
        public string UpdatedBy { get; set; }

        [BsonElement("TimeUpdated")]
        public DateTime TimeUpdated { get; set; }
    }
}