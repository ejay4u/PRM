using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PRM.Models
{
    public class LabRequest
    {
        public LabRequest()
        {
            Examinationtype = new List<string>();
        }

        [BsonId]
        public ObjectId Id { get; set; }
        public string RequestType { get; set; }
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public string Consultant { get; set; }
        public string Ward { get; set; }
        public string LMPDate { get; set; }
        public string DoctorName { get; set; }
        public bool IsPregnant { get; set; }
        public bool UG1Endoscopy { get; set; }
        public bool Colonoscopy{ get; set; }
        public bool Sigmoidoscopy { get; set; }
        public bool Proctoscopy { get; set; }
        public bool Other { get; set; }
        public string OtherRequest { get; set; }
        public string ClinicalSummary { get; set; }
        public string PreviousResults { get; set; }
        public string HasAlergy { get; set; }
        public string PaymentType { get; set; }
        public string ExaminationCategory { get; set; }
        public List<string> Examinationtype { get; set; }
        public double Amount { get; set; }
        public string RequestedExams { get; set; }
        public string RequestDate { get; set; }
        public string ScanId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime TimeCreated { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime TimeUpdated { get; set; }
    }
}