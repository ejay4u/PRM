using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PRM.Models;

namespace PRM.ViewModels
{
    public class ClientViewModel
    {
        public IEnumerable<SelectListItem> GenderList { get; set; }
        public Client Client { get; set; }
        public string Id { get; set; }
    }
}