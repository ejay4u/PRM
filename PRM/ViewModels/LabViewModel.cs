using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PRM.Models;

namespace PRM.ViewModels
{
    public class LabViewModel
    {
        public IEnumerable<SelectListItem>ClientIdList { get; set; }
        public IEnumerable<SelectListItem> DoctorIdList { get; set; }
        public IEnumerable<SelectListItem> CountryList { get; set; }
        public IEnumerable<SelectListItem> AlergyList { get; set; }
        public IEnumerable<SelectListItem> PaymentTypeList { get; set; }
        public IEnumerable<SelectListItem> CategoryList { get; set; }
        public IEnumerable<SelectListItem> ExaminationTypeList { get; set; }
        public IEnumerable<string> SelectedExaminationType { get; set; }
        public Examination Examination { get; set; }
        public LabRequest LabRequest { get; set;}
        public string Id { get; set; }
    }
}