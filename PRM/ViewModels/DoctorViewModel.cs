using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PRM.Models;

namespace PRM.ViewModels
{
    public class DoctorViewModel
    {
        public IEnumerable<SelectListItem> GenderList { get; set; }
        public Doctor Doctor { get; set; }
        public string Id { get; set; }
    }
}