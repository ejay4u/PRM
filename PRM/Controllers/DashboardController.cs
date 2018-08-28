using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using MongoDB.Bson;
using MongoDB.Driver;
using System.IO;
using ImageMagick;
using PRM.Models;
using PRM.ViewModels;

namespace PRM.Controllers
{
    public class DashboardController : Controller
    {
        readonly MongoContext _dbContext;

        public DashboardController()
        {
            _dbContext = new MongoContext();
        }

        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }

        //GET: Dashboard/Business
        public ActionResult Client()
        {
            // Build a List<SelectListItem>
            List<string> gender = new List<string>(new[] { "Male", "Female"});
            var genderList = gender.Select(x => new SelectListItem() { Value = x, Text = x }).ToList();

            var clientViewModel = new ClientViewModel()
            {
                GenderList = genderList
            };
            return View(clientViewModel);
        }


        //POST: Dashboard/Business
        [HttpPost]
        public ActionResult Client(Client client, ClientViewModel clientViewModel)
        {
            if(clientViewModel.Id.IsNullOrWhiteSpace())
            {
                client.ClientId = new AccountNoGen().RandomAccountNo();
                client.TimeCreated = DateTime.Now;
                client.TimeUpdated = DateTime.Now;
                client.Status = true;
                var builder = Builders<Client>.Filter;
                var filter = builder.Eq("ClientId", client.ClientId);
                var query = _dbContext.Clients.Find(filter).ToList();

                if (query.Count == 0)
                {
                    _dbContext.Clients.InsertOne(client);
                }
                else
                {
                    List<string> gender = new List<string>(new[] { "Male", "Female" });
                    var genderList = gender.Select(x => new SelectListItem() { Value = x, Text = x }).ToList();

                    var cliViewModel = new ClientViewModel()
                    {
                        GenderList = genderList,
                        Client = client
                    };
                    return View("Client", cliViewModel);
                }
                return RedirectToAction("Client");
            }
            else
            {
                client.Id = ObjectId.Parse(clientViewModel.Id);
                client.TimeUpdated = DateTime.Now;
                var builder = Builders<Client>.Filter;
                var filter = builder.Eq("_id", client.Id);
                /*var query = _dbContext.Businesses.Find(filter).ToEnumerable();
                business.Id = query.ElementAt(0).Id;*/

                //Update Business
                _dbContext.Clients.ReplaceOne(filter, client, new UpdateOptions { IsUpsert = false });

                return RedirectToAction("Client");
            }
            
        }

        public ActionResult EditClient(string id)
        {
            var filter = Builders<Client>.Filter.Eq("_id", ObjectId.Parse(id));

            var client = _dbContext.Clients.Find(filter).First();

            List<string> gender = new List<string>(new[] { "Male", "Female" });
            var genderList = gender.Select(x => new SelectListItem() { Value = x, Text = x }).ToList();

            var clientViewModel = new ClientViewModel()
            {
                GenderList = genderList,
                Client = client,
                Id = client.Id.ToString()
            };

            return View("Client", clientViewModel);
        }

        public ActionResult ClientDetails(string id)
        {
            var filter = Builders<Client>.Filter.Eq("_id", ObjectId.Parse(id));

            var clients = _dbContext.Clients.Find(filter).First();

            return View("ClientDetails", clients);
        }

        //GET: Dashboard/Business
        public ActionResult Doctor()
        {
            // Build a List<SelectListItem>
            List<string> gender = new List<string>(new[] { "Male", "Female" });
            var genderList = gender.Select(x => new SelectListItem() { Value = x, Text = x }).ToList();

            var doctorViewModel = new DoctorViewModel()
            {
                GenderList = genderList
            };
            return View(doctorViewModel);
        }


        //POST: Dashboard/Business
        [HttpPost]
        public ActionResult Doctor(Doctor doctor, DoctorViewModel clinicianViewModel)
        {
            if (clinicianViewModel.Id.IsNullOrWhiteSpace())
            {
                doctor.DoctorId = new AccountNoGen().RandomAccountNo();
                doctor.TimeCreated = DateTime.Now;
                doctor.TimeUpdated = DateTime.Now;
                doctor.Status = true;
                var builder = Builders<Doctor>.Filter;
                var filter = builder.Eq("DoctorId", doctor.DoctorId);
                var query = _dbContext.Doctors.Find(filter).ToList();

                if (query.Count == 0)
                {
                    _dbContext.Doctors.InsertOne(doctor);
                }
                else
                {
                    List<string> gender = new List<string>(new[] { "Male", "Female" });
                    var genderList = gender.Select(x => new SelectListItem() { Value = x, Text = x }).ToList();

                    var cliViewModel = new DoctorViewModel()
                    {
                        GenderList = genderList,
                        Doctor = doctor
                    };
                    return View("Doctor", cliViewModel);
                }
                return RedirectToAction("Doctor");
            }
            else
            {
                doctor.Id = ObjectId.Parse(clinicianViewModel.Id);
                doctor.TimeUpdated = DateTime.Now;
                var builder = Builders<Doctor>.Filter;
                var filter = builder.Eq("_id", doctor.Id);
                /*var query = _dbContext.Businesses.Find(filter).ToEnumerable();
                business.Id = query.ElementAt(0).Id;*/

                //Update Business
                _dbContext.Doctors.ReplaceOne(filter, doctor, new UpdateOptions { IsUpsert = false });

                return RedirectToAction("Doctor");
            }

        }

        public ActionResult EditDoctor(string id)
        {
            var filter = Builders<Doctor>.Filter.Eq("_id", ObjectId.Parse(id));

            var doctor = _dbContext.Doctors.Find(filter).First();

            List<string> gender = new List<string>(new[] { "Male", "Female" });
            var genderList = gender.Select(x => new SelectListItem() { Value = x, Text = x }).ToList();

            var doctorViewModel = new DoctorViewModel()
            {
                GenderList = genderList,
                Doctor = doctor,
                Id = doctor.Id.ToString()
            };

            return View("Doctor", doctorViewModel);
        }

        public ActionResult DoctorDetails(string id)
        {
            var filter = Builders<Doctor>.Filter.Eq("_id", ObjectId.Parse(id));

            var doctor = _dbContext.Doctors.Find(filter).First();

            return View("DoctorDetails", doctor);
        }

        //GET: Dashboard/Examination
        public ActionResult Examination()
        {
            // Build a List<SelectListItem>
            List<string> category = new List<string>(new[]
            {
                "Radiology - Head",
                "Radiology - Contrast Examinations",
                "Radiology - Abdominal Cavity",
                "Radiology - Vertebral Column",
                "Radiology - Ultra Sonography (USG)",
                "Radiology - Thoracic Region",
                "Radiology - Upper Limb",
                "Radiology - Lower Limb",
                "Computed Tomography (CT SCANS)"
            });
            var categoryList = category.Select(x => new SelectListItem() { Value = x, Text = x }).ToList();

            var labviewModel = new LabViewModel()
            {
                CategoryList = categoryList
            };
            
            return View(labviewModel);
        }

        //POST: Dashboard/Business
        [HttpPost]
        public ActionResult Examination(LabViewModel labViewModel)
        {
            if (labViewModel.Id.IsNullOrWhiteSpace())
            {
                labViewModel.Examination.TimeCreated = DateTime.Now;
                labViewModel.Examination.TimeUpdated = DateTime.Now;
                labViewModel.Examination.Status = true;

                _dbContext.Examinations.InsertOne(labViewModel.Examination);
                return RedirectToAction("Examination");
            }
            else
            {
                labViewModel.Examination.Id = ObjectId.Parse(labViewModel.Id);
                labViewModel.Examination.TimeUpdated = DateTime.Now;
                var builder = Builders<Examination>.Filter;
                var filter = builder.Eq("_id", labViewModel.Examination.Id);
                /*var query = _dbContext.Businesses.Find(filter).ToEnumerable();
                business.Id = query.ElementAt(0).Id;*/

                //Update Business
                _dbContext.Examinations.ReplaceOne(filter, labViewModel.Examination, new UpdateOptions { IsUpsert = false });

                return RedirectToAction("Examination");
            }

        }

        public ActionResult EditExamination(string id)
        {
            var filter = Builders<Examination>.Filter.Eq("_id", ObjectId.Parse(id));

            var examination = _dbContext.Examinations.Find(filter).First();

            List<string> category = new List<string>(new[]
            {
                "Radiology - Head",
                "Radiology - Contrast Examinations",
                "Radiology - Abdominal Cavity",
                "Radiology - Vertebral Column",
                "Radiology - Ultra Sonography (USG)",
                "Radiology - Thoracic Region",
                "Radiology - Upper Limb",
                "Radiology - Lower Limb",
                "Computed Tomography (CT SCANS)"
            });
            var categoryList = category.Select(x => new SelectListItem() { Value = x, Text = x }).ToList();

            var labViewModel = new LabViewModel()
            {
                CategoryList = categoryList,
                Examination = examination,
                Id = examination.Id.ToString()
            };

            return View("Examination", labViewModel);
        }

        public ActionResult ExaminationDetails(string id)
        {
            var filter = Builders<Examination>.Filter.Eq("_id", ObjectId.Parse(id));

            var examination = _dbContext.Examinations.Find(filter).First();

            return View("ExaminationDetails", examination);
        }

        public ActionResult UltraSound()
        {
            var filter = Builders<Client>.Filter.Empty;
            var projection = Builders<Client>.Projection.Include("ClientId").Include("Surname").Include("OtherName");
            var clients = _dbContext.Clients.Find(filter).Project(projection).ToEnumerable();

            List<SelectListItem> clientList = new List<SelectListItem>();
            foreach (var cli in clients)
            {
                SelectListItem name = new SelectListItem()
                {
                    Text = cli.GetElement(1).Value.AsString,
                    Value = cli.GetElement(2).Value.AsString + " " + cli.GetElement(3).Value.AsString
                };
                clientList.Add(name);
            }

            var filter2 = Builders<Doctor>.Filter.Empty;
            var projection2 = Builders<Doctor>.Projection.Include("DoctorId").Include("Surname").Include("OtherName");
            var doctor = _dbContext.Doctors.Find(filter2).Project(projection2).ToEnumerable();

            List<SelectListItem> doctorList = new List<SelectListItem>();
            foreach (var doc in doctor)
            {
                SelectListItem name = new SelectListItem()
                {
                    Text = doc.GetElement(2).Value.AsString + " " + doc.GetElement(3).Value.AsString,
                    Value = doc.GetElement(2).Value.AsString + " " + doc.GetElement(3).Value.AsString
                };
                doctorList.Add(name);
            }

            List<string> paymentType = new List<string>(new[] { "Internal Client", "Direct Service", "Cooperate", "CT Adult", "CT Children" });
            var paymentTypeList = paymentType.Select(x => new SelectListItem() { Value = x, Text = x }).ToList();

            var filter3 = Builders<Examination>.Filter.Empty;
            var projection3 = Builders<Examination>.Projection.Include("Category");
            var category = _dbContext.Examinations.Find(filter3).Project(projection3).ToEnumerable();

            List<SelectListItem> categoryList = new List<SelectListItem>();
            foreach (var cat in category)
            {
                SelectListItem name = new SelectListItem()
                {
                    Text = cat.GetElement(1).Value.AsString,
                    Value = cat.GetElement(1).Value.AsString
                };
                categoryList.Add(name);
            }

            var viewModel = new LabViewModel
            {
                ClientIdList = clientList,
                CategoryList = categoryList,
                DoctorIdList = doctorList,
                PaymentTypeList = paymentTypeList
            };

            return View(viewModel);
        }

        //POST: Dashboard/AddCampaign
        [HttpPost]
        public ActionResult UltraSound(LabRequest labRequest, LabViewModel labViewModel)
        {
            if (labViewModel.Id.IsNullOrWhiteSpace())
            {
                labViewModel.LabRequest.RequestType = "UltraSound";
                labViewModel.LabRequest.TimeCreated = DateTime.Now;
                labViewModel.LabRequest.TimeUpdated = DateTime.Now;
                
                foreach(var exam in labViewModel.ExaminationTypeList)
                {
                    labViewModel.LabRequest.Amount += double.Parse(exam.Value);
                }

                _dbContext.LabRequests.InsertOne(labViewModel.LabRequest);

                return RedirectToAction("UltraSound");
            }
            else
            {
                labViewModel.LabRequest.Id = ObjectId.Parse(labViewModel.Id);
                labViewModel.LabRequest.TimeUpdated = DateTime.Now;
                
                foreach(var exam in labViewModel.ExaminationTypeList)
                {
                    labViewModel.LabRequest.Amount += double.Parse(exam.Value);
                }

                //Update Campaign
                var builder = Builders<LabRequest>.Filter;
                var filter = builder.Eq("_id", ObjectId.Parse(labViewModel.Id));
                _dbContext.LabRequests.ReplaceOne(filter, labViewModel.LabRequest, new UpdateOptions { IsUpsert = false });

                return RedirectToAction("UltraSound");
            }

            
        }

        public ActionResult EditUltraSound(string id)
        {
            var filter = Builders<LabRequest>.Filter.Eq("_id", ObjectId.Parse(id));
            var ultraSound = _dbContext.LabRequests.Find(filter).First();

            var filter2 = Builders<Client>.Filter.Empty;
            var projection = Builders<Client>.Projection.Include("ClientId").Include("Surname").Include("OtherName");
            var clients = _dbContext.Clients.Find(filter2).Project(projection).ToEnumerable();

            List<SelectListItem> clientList = new List<SelectListItem>();
            foreach (var cli in clients)
            {
                SelectListItem name = new SelectListItem()
                {
                    Text = cli.GetElement(1).Value.AsString,
                    Value = cli.GetElement(2).Value.AsString + " " + cli.GetElement(3).Value.AsString
                };
                clientList.Add(name);
            }

            var filter3 = Builders<Doctor>.Filter.Empty;
            var projection2 = Builders<Doctor>.Projection.Include("DoctorId").Include("Surname").Include("OtherName");
            var doctor = _dbContext.Doctors.Find(filter3).Project(projection2).ToEnumerable();

            List<SelectListItem> doctorList = new List<SelectListItem>();
            foreach (var doc in doctor)
            {
                SelectListItem name = new SelectListItem()
                {
                    Text = doc.GetElement(2).Value.AsString + " " + doc.GetElement(3).Value.AsString,
                    Value = doc.GetElement(2).Value.AsString + " " + doc.GetElement(3).Value.AsString
                };
                doctorList.Add(name);
            }

            var filter4 = Builders<Examination>.Filter.Empty;
            var projection3 = Builders<Examination>.Projection.Include("ExaminationType");
            var ExaminationType = _dbContext.Examinations.Find(filter4).Project(projection3).ToEnumerable();

            List<SelectListItem> examinationTypeList = new List<SelectListItem>();
            foreach (var exam in ExaminationType)
            {
                SelectListItem name = new SelectListItem()
                {
                    Text = exam.GetElement(1).Value.AsString,
                    Value = exam.GetElement(1).Value.AsString
                };
                examinationTypeList.Add(name);
            }

            List<string> paymentType = new List<string>(new[] { "Internal Client", "Direct Service", "Cooperate", "CT Adult", "CT Children" });
            var paymentTypeList = paymentType.Select(x => new SelectListItem() { Value = x, Text = x }).ToList();

            var filter5 = Builders<Examination>.Filter.Empty;
            var projection4 = Builders<Examination>.Projection.Include("Category");
            var category = _dbContext.Examinations.Find(filter5).Project(projection4).ToEnumerable();

            List<SelectListItem> categoryList = new List<SelectListItem>();
            foreach (var cat in category)
            {
                SelectListItem name = new SelectListItem()
                {
                    Text = cat.GetElement(1).Value.AsString,
                    Value = cat.GetElement(1).Value.AsString
                };
                categoryList.Add(name);
            }

            var labviewModel = new LabViewModel
            {
                LabRequest = ultraSound,
                CategoryList = categoryList,
                ClientIdList = clientList,
                DoctorIdList = doctorList,
                ExaminationTypeList = examinationTypeList,
                SelectedExaminationType = ultraSound.Examinationtype,
                PaymentTypeList = paymentTypeList,
                Id = ultraSound.Id.ToString()
            };

            return View("UltraSound",labviewModel);
        }

        public ActionResult UltraSoundDetails(string id)
        {
            var filter = Builders<LabRequest>.Filter.Eq("_id", ObjectId.Parse(id));

            var ultraSound = _dbContext.LabRequests.Find(filter).First();

            return View("UltraSoundDetails", ultraSound);
        }

        public ActionResult Radiology()
        {
            var filter = Builders<Client>.Filter.Empty;
            var projection = Builders<Client>.Projection.Include("ClientId").Include("Surname").Include("OtherName");
            var clients = _dbContext.Clients.Find(filter).Project(projection).ToEnumerable();

            List<SelectListItem> clientList = new List<SelectListItem>();
            foreach (var cli in clients)
            {
                SelectListItem name = new SelectListItem()
                {
                    Text = cli.GetElement(1).Value.AsString,
                    Value = cli.GetElement(2).Value.AsString + " " + cli.GetElement(3).Value.AsString
                };
                clientList.Add(name);
            }

            var filter2 = Builders<Doctor>.Filter.Empty;
            var projection2 = Builders<Doctor>.Projection.Include("DoctorId").Include("Surname").Include("OtherName");
            var doctor = _dbContext.Doctors.Find(filter2).Project(projection2).ToEnumerable();

            List<SelectListItem> doctorList = new List<SelectListItem>();
            foreach (var doc in doctor)
            {
                SelectListItem name = new SelectListItem()
                {
                    Text = doc.GetElement(2).Value.AsString + " " + doc.GetElement(3).Value.AsString,
                    Value = doc.GetElement(2).Value.AsString + " " + doc.GetElement(3).Value.AsString
                };
                doctorList.Add(name);
            }

            List<string> alergy = new List<string>(new[] { "Yes", "No", "Unknown" });
            var alergyList = alergy.Select(x => new SelectListItem() { Value = x, Text = x }).ToList();

            List<string> paymentType = new List<string>(new[] { "Internal Client", "Direct Service", "Cooperate", "CT Adult", "CT Children" });
            var paymentTypeList = paymentType.Select(x => new SelectListItem() { Value = x, Text = x }).ToList();

            var filter3 = Builders<Examination>.Filter.Empty;
            var projection3 = Builders<Examination>.Projection.Include("Category");
            var category = _dbContext.Examinations.Find(filter3).Project(projection3).ToEnumerable();

            List<SelectListItem> categoryList = new List<SelectListItem>();
            foreach (var cat in category)
            {
                SelectListItem name = new SelectListItem()
                {
                    Text = cat.GetElement(1).Value.AsString,
                    Value = cat.GetElement(1).Value.AsString
                };
                categoryList.Add(name);
            }

            var viewModel = new LabViewModel
            {
                ClientIdList = clientList,
                CategoryList = categoryList,
                DoctorIdList = doctorList,
                AlergyList = alergyList,
                PaymentTypeList = paymentTypeList
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Radiology(LabRequest labRequest, LabViewModel labViewModel)
        {
            if (labViewModel.Id.IsNullOrWhiteSpace())
            {
                labViewModel.LabRequest.RequestType = "Radiology";
                labViewModel.LabRequest.TimeCreated = DateTime.Now;
                labViewModel.LabRequest.TimeUpdated = DateTime.Now;

                foreach (var exam in labViewModel.ExaminationTypeList)
                {
                    labViewModel.LabRequest.Amount += double.Parse(exam.Value);
                }

                _dbContext.LabRequests.InsertOne(labViewModel.LabRequest);

                return RedirectToAction("Radiology");
            }
            else
            {
                labViewModel.LabRequest.Id = ObjectId.Parse(labViewModel.Id);
                labViewModel.LabRequest.TimeUpdated = DateTime.Now;

                foreach (var exam in labViewModel.ExaminationTypeList)
                {
                    labViewModel.LabRequest.Amount += double.Parse(exam.Value);
                }

                //Update Campaign
                var builder = Builders<LabRequest>.Filter;
                var filter = builder.Eq("_id", ObjectId.Parse(labViewModel.Id));
                _dbContext.LabRequests.ReplaceOne(filter, labViewModel.LabRequest, new UpdateOptions { IsUpsert = false });

                return RedirectToAction("Radiology");
            }


        }

        public ActionResult EditRadiology(string id)
        {
            var filter = Builders<LabRequest>.Filter.Eq("_id", ObjectId.Parse(id));
            var ultraSound = _dbContext.LabRequests.Find(filter).First();

            var filter2 = Builders<Client>.Filter.Empty;
            var projection = Builders<Client>.Projection.Include("ClientId").Include("Surname").Include("OtherName");
            var clients = _dbContext.Clients.Find(filter2).Project(projection).ToEnumerable();

            List<SelectListItem> clientList = new List<SelectListItem>();
            foreach (var cli in clients)
            {
                SelectListItem name = new SelectListItem()
                {
                    Text = cli.GetElement(1).Value.AsString,
                    Value = cli.GetElement(2).Value.AsString + " " + cli.GetElement(3).Value.AsString
                };
                clientList.Add(name);
            }

            var filter3 = Builders<Doctor>.Filter.Empty;
            var projection2 = Builders<Doctor>.Projection.Include("DoctorId").Include("Surname").Include("OtherName");
            var doctor = _dbContext.Doctors.Find(filter3).Project(projection2).ToEnumerable();

            List<SelectListItem> doctorList = new List<SelectListItem>();
            foreach (var doc in doctor)
            {
                SelectListItem name = new SelectListItem()
                {
                    Text = doc.GetElement(2).Value.AsString + " " + doc.GetElement(3).Value.AsString,
                    Value = doc.GetElement(2).Value.AsString + " " + doc.GetElement(3).Value.AsString
                };
                doctorList.Add(name);
            }

            List<string> alergy = new List<string>(new[] { "Yes", "No", "Unknown" });
            var alergyList = alergy.Select(x => new SelectListItem() { Value = x, Text = x }).ToList();

            List<string> paymentType = new List<string>(new[] { "Internal Client", "Direct Service", "Cooperate", "CT Adult", "CT Children" });
            var paymentTypeList = paymentType.Select(x => new SelectListItem() { Value = x, Text = x }).ToList();

            var filter4 = Builders<Examination>.Filter.Empty;
            var projection3 = Builders<Examination>.Projection.Include("Category");
            var category = _dbContext.Examinations.Find(filter4).Project(projection3).ToEnumerable();

            List<SelectListItem> categoryList = new List<SelectListItem>();
            foreach (var cat in category)
            {
                SelectListItem name = new SelectListItem()
                {
                    Text = cat.GetElement(1).Value.AsString,
                    Value = cat.GetElement(1).Value.AsString
                };
                categoryList.Add(name);
            }

            var labviewModel = new LabViewModel
            {
                LabRequest = ultraSound,
                CategoryList = categoryList,
                ClientIdList = clientList,
                DoctorIdList = doctorList,
                AlergyList = alergyList,
                PaymentTypeList = paymentTypeList,
                Id = ultraSound.Id.ToString()
            };

            return View("Radiology", labviewModel);
        }

        public ActionResult RadiologyDetails(string id)
        {
            var filter = Builders<LabRequest>.Filter.Eq("_id", ObjectId.Parse(id));

            var ultraSound = _dbContext.LabRequests.Find(filter).First();

            return View("RadiologyDetails", ultraSound);
        }

        public ActionResult Endoscopy()
        {
            var filter = Builders<Client>.Filter.Empty;
            var projection = Builders<Client>.Projection.Include("ClientId").Include("Surname").Include("OtherName");
            var clients = _dbContext.Clients.Find(filter).Project(projection).ToEnumerable();

            List<SelectListItem> clientList = new List<SelectListItem>();
            foreach (var cli in clients)
            {
                SelectListItem name = new SelectListItem()
                {
                    Text = cli.GetElement(1).Value.AsString,
                    Value = cli.GetElement(2).Value.AsString + " " + cli.GetElement(3).Value.AsString
                };
                clientList.Add(name);
            }

            var filter2 = Builders<Doctor>.Filter.Empty;
            var projection2 = Builders<Doctor>.Projection.Include("DoctorId").Include("Surname").Include("OtherName");
            var doctor = _dbContext.Doctors.Find(filter2).Project(projection2).ToEnumerable();

            List<SelectListItem> doctorList = new List<SelectListItem>();
            foreach (var doc in doctor)
            {
                SelectListItem name = new SelectListItem()
                {
                    Text = doc.GetElement(2).Value.AsString + " " + doc.GetElement(3).Value.AsString,
                    Value = doc.GetElement(2).Value.AsString + " " + doc.GetElement(3).Value.AsString
                };
                doctorList.Add(name);
            }

            List<string> paymentType = new List<string>(new[] { "Internal Client", "Direct Service", "Cooperate", "CT Adult", "CT Children" });
            var paymentTypeList = paymentType.Select(x => new SelectListItem() { Value = x, Text = x }).ToList();

            var filter3 = Builders<Examination>.Filter.Empty;
            var projection3 = Builders<Examination>.Projection.Include("Category");
            var category = _dbContext.Examinations.Find(filter3).Project(projection3).ToEnumerable();

            List<SelectListItem> categoryList = new List<SelectListItem>();
            foreach (var cat in category)
            {
                SelectListItem name = new SelectListItem()
                {
                    Text = cat.GetElement(1).Value.AsString,
                    Value = cat.GetElement(1).Value.AsString
                };
                categoryList.Add(name);
            }

            var viewModel = new LabViewModel
            {
                ClientIdList = clientList,
                CategoryList = categoryList,
                DoctorIdList = doctorList,
                PaymentTypeList = paymentTypeList
            };

            return View(viewModel);
        }

        //POST: Dashboard/AddCampaign
        [HttpPost]
        public ActionResult Endoscopy(LabRequest labRequest, LabViewModel labViewModel)
        {
            if (labViewModel.Id.IsNullOrWhiteSpace())
            {
                labViewModel.LabRequest.RequestType = "Endoscopy";
                labViewModel.LabRequest.TimeCreated = DateTime.Now;
                labViewModel.LabRequest.TimeUpdated = DateTime.Now;

                foreach (var exam in labViewModel.ExaminationTypeList)
                {
                    labViewModel.LabRequest.Amount += double.Parse(exam.Value);
                }

                _dbContext.LabRequests.InsertOne(labViewModel.LabRequest);

                return RedirectToAction("Endoscopy");
            }
            else
            {
                labViewModel.LabRequest.Id = ObjectId.Parse(labViewModel.Id);
                labViewModel.LabRequest.TimeUpdated = DateTime.Now;

                foreach (var exam in labViewModel.ExaminationTypeList)
                {
                    labViewModel.LabRequest.Amount += double.Parse(exam.Value);
                }

                //Update Campaign
                var builder = Builders<LabRequest>.Filter;
                var filter = builder.Eq("_id", ObjectId.Parse(labViewModel.Id));
                _dbContext.LabRequests.ReplaceOne(filter, labViewModel.LabRequest, new UpdateOptions { IsUpsert = false });

                return RedirectToAction("Endoscopy");
            }


        }

        public ActionResult EditEndoscopy(string id)
        {
            var filter = Builders<LabRequest>.Filter.Eq("_id", ObjectId.Parse(id));
            var ultraSound = _dbContext.LabRequests.Find(filter).First();

            var filter2 = Builders<Client>.Filter.Empty;
            var projection = Builders<Client>.Projection.Include("ClientId").Include("Surname").Include("OtherName");
            var clients = _dbContext.Clients.Find(filter2).Project(projection).ToEnumerable();

            List<SelectListItem> clientList = new List<SelectListItem>();
            foreach (var cli in clients)
            {
                SelectListItem name = new SelectListItem()
                {
                    Text = cli.GetElement(1).Value.AsString,
                    Value = cli.GetElement(2).Value.AsString + " " + cli.GetElement(3).Value.AsString
                };
                clientList.Add(name);
            }

            var filter3 = Builders<Doctor>.Filter.Empty;
            var projection2 = Builders<Doctor>.Projection.Include("DoctorId").Include("Surname").Include("OtherName");
            var doctor = _dbContext.Doctors.Find(filter3).Project(projection2).ToEnumerable();

            List<SelectListItem> doctorList = new List<SelectListItem>();
            foreach (var doc in doctor)
            {
                SelectListItem name = new SelectListItem()
                {
                    Text = doc.GetElement(2).Value.AsString + " " + doc.GetElement(3).Value.AsString,
                    Value = doc.GetElement(2).Value.AsString + " " + doc.GetElement(3).Value.AsString
                };
                doctorList.Add(name);
            }

            List<string> paymentType = new List<string>(new[] { "Internal Client", "Direct Service", "Cooperate", "CT Adult", "CT Children" });
            var paymentTypeList = paymentType.Select(x => new SelectListItem() { Value = x, Text = x }).ToList();

            var filter4 = Builders<Examination>.Filter.Empty;
            var projection3 = Builders<Examination>.Projection.Include("Category");
            var category = _dbContext.Examinations.Find(filter4).Project(projection3).ToEnumerable();

            List<SelectListItem> categoryList = new List<SelectListItem>();
            foreach (var cat in category)
            {
                SelectListItem name = new SelectListItem()
                {
                    Text = cat.GetElement(1).Value.AsString,
                    Value = cat.GetElement(1).Value.AsString
                };
                categoryList.Add(name);
            }

            var labviewModel = new LabViewModel
            {
                LabRequest = ultraSound,
                CategoryList = categoryList,
                ClientIdList = clientList,
                DoctorIdList = doctorList,
                PaymentTypeList = paymentTypeList,
                Id = ultraSound.Id.ToString()
            };

            return View("Endoscopy", labviewModel);
        }

        public ActionResult EndoscopyDetails(string id)
        {
            var filter = Builders<LabRequest>.Filter.Eq("_id", ObjectId.Parse(id));

            var ultraSound = _dbContext.LabRequests.Find(filter).First();

            return View("EndoscopyDetails", ultraSound);
        }

        public ActionResult ECG()
        {
            var filter = Builders<Client>.Filter.Empty;
            var projection = Builders<Client>.Projection.Include("ClientId").Include("Surname").Include("OtherName");
            var clients = _dbContext.Clients.Find(filter).Project(projection).ToEnumerable();

            List<SelectListItem> clientList = new List<SelectListItem>();
            foreach (var cli in clients)
            {
                SelectListItem name = new SelectListItem()
                {
                    Text = cli.GetElement(1).Value.AsString,
                    Value = cli.GetElement(2).Value.AsString + " " + cli.GetElement(3).Value.AsString
                };
                clientList.Add(name);
            }

            var filter2 = Builders<Doctor>.Filter.Empty;
            var projection2 = Builders<Doctor>.Projection.Include("DoctorId").Include("Surname").Include("OtherName");
            var doctor = _dbContext.Doctors.Find(filter2).Project(projection2).ToEnumerable();

            List<SelectListItem> doctorList = new List<SelectListItem>();
            foreach (var doc in doctor)
            {
                SelectListItem name = new SelectListItem()
                {
                    Text = doc.GetElement(2).Value.AsString + " " + doc.GetElement(3).Value.AsString,
                    Value = doc.GetElement(2).Value.AsString + " " + doc.GetElement(3).Value.AsString
                };
                doctorList.Add(name);
            }

            List<string> paymentType = new List<string>(new[] { "Internal Client", "Direct Service", "Cooperate", "CT Adult", "CT Children" });
            var paymentTypeList = paymentType.Select(x => new SelectListItem() { Value = x, Text = x }).ToList();

            var filter3 = Builders<Examination>.Filter.Empty;
            var projection3 = Builders<Examination>.Projection.Include("Category");
            var category = _dbContext.Examinations.Find(filter3).Project(projection3).ToEnumerable();

            List<SelectListItem> categoryList = new List<SelectListItem>();
            foreach (var cat in category)
            {
                SelectListItem name = new SelectListItem()
                {
                    Text = cat.GetElement(1).Value.AsString,
                    Value = cat.GetElement(1).Value.AsString
                };
                categoryList.Add(name);
            }

            var viewModel = new LabViewModel
            {
                ClientIdList = clientList,
                CategoryList = categoryList,
                DoctorIdList = doctorList,
                PaymentTypeList = paymentTypeList
            };

            return View(viewModel);
        }

        //POST: Dashboard/AddCampaign
        [HttpPost]
        public ActionResult ECG(LabRequest labRequest, LabViewModel labViewModel)
        {
            if (labViewModel.Id.IsNullOrWhiteSpace())
            {
                labViewModel.LabRequest.RequestType = "ECG";
                labViewModel.LabRequest.TimeCreated = DateTime.Now;
                labViewModel.LabRequest.TimeUpdated = DateTime.Now;

                foreach (var exam in labViewModel.ExaminationTypeList)
                {
                    labViewModel.LabRequest.Amount += double.Parse(exam.Value);
                }

                _dbContext.LabRequests.InsertOne(labViewModel.LabRequest);

                return RedirectToAction("ECG");
            }
            else
            {
                labViewModel.LabRequest.Id = ObjectId.Parse(labViewModel.Id);
                labViewModel.LabRequest.TimeUpdated = DateTime.Now;

                foreach (var exam in labViewModel.ExaminationTypeList)
                {
                    labViewModel.LabRequest.Amount += double.Parse(exam.Value);
                }

                //Update Campaign
                var builder = Builders<LabRequest>.Filter;
                var filter = builder.Eq("_id", ObjectId.Parse(labViewModel.Id));
                _dbContext.LabRequests.ReplaceOne(filter, labViewModel.LabRequest, new UpdateOptions { IsUpsert = false });

                return RedirectToAction("ECG");
            }


        }

        public ActionResult EditECG(string id)
        {
            var filter = Builders<LabRequest>.Filter.Eq("_id", ObjectId.Parse(id));
            var ultraSound = _dbContext.LabRequests.Find(filter).First();

            var filter2 = Builders<Client>.Filter.Empty;
            var projection = Builders<Client>.Projection.Include("ClientId").Include("Surname").Include("OtherName");
            var clients = _dbContext.Clients.Find(filter2).Project(projection).ToEnumerable();

            List<SelectListItem> clientList = new List<SelectListItem>();
            foreach (var cli in clients)
            {
                SelectListItem name = new SelectListItem()
                {
                    Text = cli.GetElement(1).Value.AsString,
                    Value = cli.GetElement(2).Value.AsString + " " + cli.GetElement(3).Value.AsString
                };
                clientList.Add(name);
            }

            var filter3 = Builders<Doctor>.Filter.Empty;
            var projection2 = Builders<Doctor>.Projection.Include("DoctorId").Include("Surname").Include("OtherName");
            var doctor = _dbContext.Doctors.Find(filter3).Project(projection2).ToEnumerable();

            List<SelectListItem> doctorList = new List<SelectListItem>();
            foreach (var doc in doctor)
            {
                SelectListItem name = new SelectListItem()
                {
                    Text = doc.GetElement(2).Value.AsString + " " + doc.GetElement(3).Value.AsString,
                    Value = doc.GetElement(2).Value.AsString + " " + doc.GetElement(3).Value.AsString
                };
                doctorList.Add(name);
            }

            List<string> paymentType = new List<string>(new[] { "Internal Client", "Direct Service", "Cooperate", "CT Adult", "CT Children" });
            var paymentTypeList = paymentType.Select(x => new SelectListItem() { Value = x, Text = x }).ToList();

            var filter4 = Builders<Examination>.Filter.Empty;
            var projection3 = Builders<Examination>.Projection.Include("Category");
            var category = _dbContext.Examinations.Find(filter4).Project(projection3).ToEnumerable();

            List<SelectListItem> categoryList = new List<SelectListItem>();
            foreach (var cat in category)
            {
                SelectListItem name = new SelectListItem()
                {
                    Text = cat.GetElement(1).Value.AsString,
                    Value = cat.GetElement(1).Value.AsString
                };
                categoryList.Add(name);
            }

            var labviewModel = new LabViewModel
            {
                LabRequest = ultraSound,
                CategoryList = categoryList,
                ClientIdList = clientList,
                DoctorIdList = doctorList,
                PaymentTypeList = paymentTypeList,
                Id = ultraSound.Id.ToString()
            };

            return View("ECG", labviewModel);
        }

        public ActionResult ECGDetails(string id)
        {
            var filter = Builders<LabRequest>.Filter.Eq("_id", ObjectId.Parse(id));

            var ultraSound = _dbContext.LabRequests.Find(filter).First();

            return View("ECGDetails", ultraSound);
        }

        //GET: Dashboard/GenerateCodes
        public ActionResult GenerateCodes()
        {
            List<string> country = new List<string>();
            CultureInfo[] cInfoList = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            foreach (CultureInfo cInfo in cInfoList)
            {
                RegionInfo R = new RegionInfo(cInfo.LCID);
                if (!(country.Contains(R.EnglishName)))
                {
                    country.Add(R.EnglishName);
                }
            }
            if (!(country.Contains("Ghana")))
            {
                country.Add("Ghana");
            }
            country.Sort();

            // Build a List<SelectListItem>
            var countryList = country.Select(x => new SelectListItem() { Value = x, Text = x }).ToList();

            var viewModel = new CodeViewModel()
            {
                CountryList = countryList
            };

            return View(viewModel);
        }

        public ActionResult ExaminationTypeListView(string category,string paymentType = "InternalClient")
        {
            var builder = Builders<Examination>.Filter;
            var filter = builder.Eq("Category", category);
            var projection = Builders<Examination>.Projection.Include("ExaminationType").Include(paymentType);
            var examType = _dbContext.Examinations.Find(filter).Project(projection).ToEnumerable();

            List<SelectListItem> examTypeList = new List<SelectListItem>();
            foreach (var exam in examType)
            {
                SelectListItem examTypeName = new SelectListItem()
                {
                    Text = exam.GetElement(1).Value.AsString,
                    Value = exam.GetElement(2).Value.AsString
                };
                examTypeList.Add(examTypeName);
            }

            var viewModel = new LabViewModel()
            {
                ExaminationTypeList = examTypeList
            };

            return View(viewModel);
        }

        //[Route("Dashboard/CampaignListView/{country?}/{accountid?}")]
        public ActionResult CampaignListView(string country)
        {
            var builder = Builders<LabRequest>.Filter;
            var filter = builder.Eq("CampaignCountry", country) & builder.Eq("CampaignStatus", true);
            var projection = Builders<LabRequest>.Projection.Include("CampaignTitle");
            var campaigns = _dbContext.LabRequests.Find(filter).Project(projection).ToEnumerable();

            List<SelectListItem> campaignList = new List<SelectListItem>();
            foreach (var campaign in campaigns)
            {
                SelectListItem bname = new SelectListItem()
                {
                    Text = campaign.GetElement(1).Value.AsString,
                    Value = campaign.GetElement(0).Value.AsObjectId.ToString()
                };
                campaignList.Add(bname);
            }

            var campaignViewModel = new AdViewModel()
            {
                CampaignList = campaignList
            };

            return View(campaignViewModel);
        }

        [HttpPost]
        public ActionResult GenerateCodes(CampaignCode campaignCode, LabRequest campaign)
        {
            //var builder = Builders<LabRequest>.Filter;
            //var filter = builder.Eq("_id", ObjectId.Parse(campaignCode.CampaignId));
            //var query = _dbContext.LabRequests.Find(filter).FirstOrDefault();

            //if (query != null)
            //{
            //    try
            //    {
            //        var update = Builders<LabRequest>.Update.Set("CampaignCodeQty", query.CampaignCodeQty + campaign.CampaignCodeQty).CurrentDate("TimeUpdated");
            //        _dbContext.LabRequests.UpdateOne(filter, update);

            //        int codeQty = campaign.CampaignCodeQty;
            //        campaign.TimeCreated = DateTime.Now;
            //        campaign.TimeUpdated = DateTime.Now;
            //        campaign.CampaignStatus = true;

            //        for (int i = 0; i < codeQty; i++)
            //        {
            //            var codes = new CampaignCode();
            //            {
            //                codes.CampaignId = campaignCode.CampaignId;
            //                codes.CodeStatus = true;
            //                codes.TimeCreated = DateTime.Now;
            //                codes.Code = Guid.NewGuid().ToString().ToUpper().Substring(0, 11).Replace("-", string.Empty);
            //            };

            //            _dbContext.CampaignCodes.InsertOne(codes);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        // log or manage the exception
            //        throw ex;
            //    }

            //}
            return RedirectToAction("GenerateCodes");
        }

        public ActionResult CountryListView()
        {
            List<string> country = new List<string>();
            CultureInfo[] cInfoList = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            foreach (CultureInfo cInfo in cInfoList)
            {
                RegionInfo R = new RegionInfo(cInfo.LCID);
                if (!(country.Contains(R.EnglishName)))
                {
                    country.Add(R.EnglishName);
                }
            }
            if (!(country.Contains("Ghana")))
            {
                country.Add("Ghana");
            }
            country.Sort();

            // Build a List<SelectListItem>
            var countryList = country.Select(x => new SelectListItem() { Value = x, Text = x }).ToList();

            var countryViewModel = new AdViewModel()
            {
                CountryList = countryList
            };

            return View(countryViewModel);
        }

        public ActionResult BusinessListView(string country)
        {
            var builder = Builders<Client>.Filter;
            var filter = builder.Eq("Country", country);
            var projection = Builders<Client>.Projection.Include("ClientId").Include("FullName");
            var businesses = _dbContext.Clients.Find(filter).Project(projection).ToEnumerable();

            List<SelectListItem> businessList = new List<SelectListItem>();
            foreach (var business in businesses)
            {
                SelectListItem bname = new SelectListItem()
                {
                    Text = business.GetElement(2).Value.AsString,
                    Value = business.GetElement(1).Value.AsString
                };
                businessList.Add(bname);
            }

            var viewModel = new CodeViewModel()
            {
                BusinessList = businessList
            };

            return View(viewModel);
        }

        public ActionResult CodeCampaignListView(string accountid)
        {
            var builder = Builders<LabRequest>.Filter;
            var filter = builder.Eq("AccountId", accountid) & builder.Eq("CampaignStatus", true); ;
            var projection = Builders<LabRequest>.Projection.Include("CampaignTitle");
            var campaigns = _dbContext.LabRequests.Find(filter).Project(projection).ToEnumerable();

            List<SelectListItem> campaignList = new List<SelectListItem>();
            foreach (var campaign in campaigns)
            {
                SelectListItem bname = new SelectListItem()
                {
                    Text = campaign.GetElement(1).Value.AsString,
                    Value = campaign.GetElement(0).Value.AsObjectId.ToString()
                };
                campaignList.Add(bname);
            }

            var codeViewModel = new CodeViewModel()
            {
                CampaignList = campaignList
            };

            return View(codeViewModel);
        }

        public ActionResult GeneralAd()
        {
            List<string> country = new List<string>();
            CultureInfo[] cInfoList = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            foreach (CultureInfo cInfo in cInfoList)
            {
                RegionInfo R = new RegionInfo(cInfo.LCID);
                if (!(country.Contains(R.EnglishName)))
                {
                    country.Add(R.EnglishName);
                }
            }
            if (!(country.Contains("Ghana")))
            {
                country.Add("Ghana");
            }
            country.Sort();

            // Build a List<SelectListItem>
            var countryList = country.Select(x => new SelectListItem() { Value = x, Text = x }).ToList();

            //Create adType List
            List<string> addType = new List<string>(new[] { "Background-Image", "Background-Video(mute)", "Background-Audio", "PassItCode-Image", "PassItCode-Video", "PhoneNo-Image(Default)", "PhoneNo-Video(Default)" });
            var addTypeList = addType.Select(x => new SelectListItem() { Value = x, Text = x }).ToList();

            //Create videoHost List
            List<string> videoHost = new List<string>(new[] { "YouTube", "Vimeo", "Dailymotion" });
            var videoHostList = videoHost.Select(x => new SelectListItem() { Value = x, Text = x }).ToList();

            var generalAdView = new AdViewModel()
            {
                CountryList = countryList,
                AddTypeList = addTypeList,
                VideoHostList = videoHostList
            };
            return View(generalAdView);
        }

        [HttpPost]
        public ActionResult GeneralAd(AdInfo adInfo, AdViewModel adViewModel)
        {
            if (adViewModel.Id.IsNullOrWhiteSpace())
            {
                adInfo.AdCountry = adViewModel.AdInfo.AdCountry;
                adInfo.AdType = adViewModel.AdInfo.AdType;

                if (!adViewModel.ImageUrl.IsNullOrWhiteSpace())
                {
                    //Get The File Posted
                    HttpPostedFileBase image = Request.Files["ImageUrl"];
                    //Generate a filename to store the file on the server
                    if (image != null)
                    {
                        var fileName = Guid.NewGuid() + Path.GetFileName(image.FileName);
                        {
                            var imageTypes = new[] { "jpg", "jpeg", "png" };
                            var extension = Path.GetExtension(Path.GetFileName(image.FileName));
                            if (extension != null)
                            {
                                var fileExt = extension.Substring(1).ToLower();
                                if (imageTypes.Contains(fileExt))
                                {
                                    var imagePath = Path.Combine(Server.MapPath("~/assets/image"), fileName);

                                    // store the uploaded file on the file system
                                    image.SaveAs(imagePath);

                                    //Return final filepath to database
                                    adInfo.AdMedia = new BsonDocument
                                    {
                                        { "ImageUrl", "/assets/image/" + fileName },
                                        {"VideoUrl", BsonNull.Value},
                                        {"VideoHost", BsonNull.Value},
                                        {"AudioUrl", BsonNull.Value }
                                    };
                                }
                            }
                        }
                    }
                }
                else if (!adViewModel.AudioUrl.IsNullOrWhiteSpace())
                {
                    //Get The File Posted
                    HttpPostedFileBase audio = Request.Files["AudioUrl"];

                    if (audio != null)
                    {
                        var fileName = Guid.NewGuid() + Path.GetFileName(audio.FileName);
                        {
                            var audioTypes = new[] { "mp3" };
                            var extension = Path.GetExtension(Path.GetFileName(audio.FileName));
                            if (extension != null)
                            {
                                var fileExt = extension.Substring(1).ToLower();
                                if (audioTypes.Contains(fileExt))
                                {
                                    var imagePath = Path.Combine(Server.MapPath("~/assets/audio"), fileName);

                                    // store the uploaded file on the file system
                                    audio.SaveAs(imagePath);

                                    //Return final filepath to database
                                    adInfo.AdMedia = new BsonDocument
                                    {
                                        { "AudioUrl", "/assets/audio/" + fileName },
                                        {"VideoUrl", BsonNull.Value},
                                        {"VideoHost", BsonNull.Value},
                                        {"ImageUrl", BsonNull.Value }
                                    };
                                }
                            }
                        }
                    }
                }
                else if (!adViewModel.VideoUrl.IsNullOrWhiteSpace() && !adViewModel.VideoHost.IsNullOrWhiteSpace())
                {
                    adInfo.AdMedia = new BsonDocument
                    {
                        { "VideoUrl", adViewModel.VideoUrl },
                        { "VideoHost", adViewModel.VideoHost },
                        {"ImageUrl", BsonNull.Value },
                        {"AudioUrl", BsonNull.Value }
                    };
                }

                //Check if Add exists for same country and deactivate NB: BG-image and Bg-video are mutually exclusive
                if (adViewModel.AdInfo.AdType.Equals("Background-Image") ||
                    adViewModel.AdInfo.AdType.Equals("Background-Video(mute)"))
                {
                    var builder = Builders<AdInfo>.Filter;
                    var filter = builder.Type("CampaignId", BsonType.Null) &
                                 builder.Or(builder.Eq("AdType", "Background-Image"),
                                     builder.Eq("AdType", "Background-Video(mute)")) &
                                 builder.Eq("AdCountry", adViewModel.AdInfo.AdCountry) &
                                 builder.Eq("AdStatus", true);
                    var query = _dbContext.AdInfos.Find(filter).FirstOrDefaultAsync();
                    if (query != null)
                    {
                        try
                        {
                            var update = Builders<AdInfo>.Update.Set("AdStatus", false).CurrentDate("TimeUpdated");
                            _dbContext.AdInfos.UpdateOne(filter, update);
                        }
                        catch (Exception ex)
                        {
                            // log or manage the exception
                            throw ex;
                        }
                    }
                }
                else
                {
                    var builder = Builders<AdInfo>.Filter;
                    var filter = builder.Type("CampaignId", BsonType.Null) &
                                 builder.Eq("AdType", adViewModel.AdInfo.AdType) &
                                 builder.Eq("AdCountry", adViewModel.AdInfo.AdCountry) &
                                 builder.Eq("AdStatus", true);
                    var query = _dbContext.AdInfos.Find(filter).FirstOrDefaultAsync();
                    if (query != null)
                    {
                        try
                        {
                            var update = Builders<AdInfo>.Update.Set("AdStatus", false).CurrentDate("TimeUpdated");
                            _dbContext.AdInfos.UpdateOne(filter, update);
                        }
                        catch (Exception ex)
                        {
                            // log or manage the exception
                            throw ex;
                        }
                    }
                }


                //Insert the new Add.
                adInfo.AdStatus = adViewModel.AdInfo.AdStatus;
                adInfo.TimeCreated = DateTime.Now;
                adInfo.TimeUpdated = DateTime.Now;
                _dbContext.AdInfos.InsertOneAsync(adInfo);
            }
            else
            {
                adInfo.AdCountry = adViewModel.AdInfo.AdCountry;
                adInfo.AdType = adViewModel.AdInfo.AdType;

                if (!adViewModel.ImageUrl.IsNullOrWhiteSpace())
                {
                    //Get The File Posted
                    HttpPostedFileBase image = Request.Files["ImageUrl"];
                    //Generate a filename to store the file on the server
                    if (image != null)
                    {
                        var fileName = Guid.NewGuid() + Path.GetFileName(image.FileName);
                        {
                            var imageTypes = new[] {"jpg", "jpeg", "png"};
                            var extension = Path.GetExtension(Path.GetFileName(image.FileName));
                            if (extension != null)
                            {
                                var fileExt = extension.Substring(1).ToLower();
                                if (imageTypes.Contains(fileExt))
                                {
                                    var imagePath = Path.Combine(Server.MapPath("~/assets/image"), fileName);

                                    // store the uploaded file on the file system
                                    image.SaveAs(imagePath);

                                    //Return final filepath to database
                                    adInfo.AdMedia = new BsonDocument
                                    {
                                        {"ImageUrl", "/assets/image/" + fileName},
                                        {"VideoUrl", BsonNull.Value},
                                        {"VideoHost", BsonNull.Value},
                                        {"AudioUrl", BsonNull.Value }
                                    };
                                }
                            }
                        }
                    }
                }
                else if (!adViewModel.AudioUrl.IsNullOrWhiteSpace())
                {
                    //Get The File Posted
                    HttpPostedFileBase audio = Request.Files["AudioUrl"];

                    if (audio != null)
                    {
                        var fileName = Guid.NewGuid() + Path.GetFileName(audio.FileName);
                        {
                            var audioTypes = new[] { "mp3" };
                            var extension = Path.GetExtension(Path.GetFileName(audio.FileName));
                            if (extension != null)
                            {
                                var fileExt = extension.Substring(1).ToLower();
                                if (audioTypes.Contains(fileExt))
                                {
                                    var imagePath = Path.Combine(Server.MapPath("~/assets/audio"), fileName);

                                    // store the uploaded file on the file system
                                    audio.SaveAs(imagePath);

                                    //Return final filepath to database
                                    adInfo.AdMedia = new BsonDocument
                                    {
                                        { "AudioUrl", "/assets/audio/" + fileName },
                                        {"VideoUrl", BsonNull.Value},
                                        {"VideoHost", BsonNull.Value},
                                        {"ImageUrl", BsonNull.Value }
                                    };
                                }
                            }
                        }
                    }
                }
                else if (!adViewModel.VideoUrl.IsNullOrWhiteSpace() && !adViewModel.VideoHost.IsNullOrWhiteSpace())
                {
                    adInfo.AdMedia = new BsonDocument
                    {
                        { "VideoUrl", adViewModel.VideoUrl },
                        { "VideoHost", adViewModel.VideoHost },
                        {"ImageUrl", BsonNull.Value },
                        {"AudioUrl", BsonNull.Value }
                    };
                }

                //Update the Ad.
                adInfo.Id = ObjectId.Parse(adViewModel.Id);
                adInfo.TimeUpdated = DateTime.Now;
                var builder = Builders<AdInfo>.Filter;
                var filter = builder.Eq("_id", adInfo.Id);
                _dbContext.AdInfos.ReplaceOne(filter, adInfo, new UpdateOptions { IsUpsert = false });
            }

            return RedirectToAction("GeneralAd");
        }

        public ActionResult EditGeneralAd(string id)
        {
            var filter = Builders<AdInfo>.Filter.Eq("_id", ObjectId.Parse(id));
            var generalAd = _dbContext.AdInfos.Find(filter).First();

            List<string> country = new List<string>();
            CultureInfo[] cInfoList = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            foreach (CultureInfo cInfo in cInfoList)
            {
                RegionInfo R = new RegionInfo(cInfo.LCID);
                if (!(country.Contains(R.EnglishName)))
                {
                    country.Add(R.EnglishName);
                }
            }
            if (!(country.Contains("Ghana")))
            {
                country.Add("Ghana");
            }
            country.Sort();

            // Build a List<SelectListItem>
            var countryList = country.Select(x => new SelectListItem() { Value = x, Text = x }).ToList();

            //Create adType List
            List<string> addType = new List<string>(new[] { "Background-Image", "Background-Video(mute)", "Background-Audio", "PassItCode-Image", "PassItCode-Video", "PhoneNo-Image(Default)", "PhoneNo-Video(Default)" });
            var addTypeList = addType.Select(x => new SelectListItem() { Value = x, Text = x }).ToList();

            //Create videoHost List
            List<string> videoHost = new List<string>(new[] { "YouTube", "Vimeo", "Dailymotion" });
            var videoHostList = videoHost.Select(x => new SelectListItem() { Value = x, Text = x }).ToList();

            var generalAdView = new AdViewModel()
            {
                AdInfo = generalAd,
                CountryList = countryList,
                AddTypeList = addTypeList,
                VideoHostList = videoHostList,
                VideoUrl = generalAd.AdMedia.GetElement("VideoUrl").Value.ToString(),
                VideoHost = generalAd.AdMedia.GetElement("VideoHost").Value.ToString(),
                ImageUrl = generalAd.AdMedia.GetElement("ImageUrl").Value.ToString(),
                AudioUrl = generalAd.AdMedia.GetElement("AudioUrl").Value.ToString()
            };

            return View("GeneralAd",generalAdView);
        }

        public ActionResult CampaignAd()
        {
            //Create addType List
            List<string> addType = new List<string>(new[] { "PhoneNo-Image", "PhoneNo-Video" });
            var addTypeList = addType.Select(x => new SelectListItem() { Value = x, Text = x }).ToList();

            //Create videoHost List
            List<string> videoHost = new List<string>(new[] { "YouTube", "Vimeo", "Dailymotion" });
            var videoHostList = videoHost.Select(x => new SelectListItem() { Value = x, Text = x }).ToList();

            var campaignAdView = new AdViewModel()
            {
                AddTypeList = addTypeList,
                VideoHostList = videoHostList
            };
            return View(campaignAdView);
        }

        public ActionResult EditCampaignAd(string id)
        {
            var filter = Builders<AdInfo>.Filter.Eq("_id", ObjectId.Parse(id));
            var campAd = _dbContext.AdInfos.Find(filter).First();

            //Create addType List
            List<string> addType = new List<string>(new[] { "PhoneNo-Image", "PhoneNo-Video" });
            var addTypeList = addType.Select(x => new SelectListItem() { Value = x, Text = x }).ToList();

            //Create videoHost List
            List<string> videoHost = new List<string>(new[] { "YouTube", "Vimeo", "Dailymotion" });
            var videoHostList = videoHost.Select(x => new SelectListItem() { Value = x, Text = x }).ToList();

            var campaignAdView = new AdViewModel()
            {
                AdInfo = campAd,
                AddTypeList = addTypeList,
                VideoHostList = videoHostList,
                VideoUrl = campAd.AdMedia.GetElement("VideoUrl").Value.ToString(),
                VideoHost = campAd.AdMedia.GetElement("VideoHost").Value.ToString(),
                ImageUrl = campAd.AdMedia.GetElement("ImageUrl").Value.ToString(),
            };
            return View("CampaignAd",campaignAdView);
        }

        [HttpPost]
        public ActionResult CampaignAd(AdInfo adInfo, AdViewModel adViewModel)
        {
            if (adViewModel.Id.IsNullOrWhiteSpace())
            {
                adInfo.CampaignId = adViewModel.AdInfo.CampaignId;
                adInfo.AdCountry = adViewModel.AdInfo.AdCountry;
                adInfo.AdType = adViewModel.AdInfo.AdType;
                if (!adViewModel.ImageUrl.IsNullOrWhiteSpace())
                {
                    //Get The File Posted
                    HttpPostedFileBase image = Request.Files["ImageUrl"];
                    //Generate a filename to store the file on the server
                    if (image != null)
                    {
                        var imageTypes = new[] {"jpg", "jpeg", "png"};
                        var extension = Path.GetExtension(Path.GetFileName(image.FileName));
                        if (extension != null)
                        {
                            var fileExt = extension.Substring(1).ToLower();
                            if (imageTypes.Contains(fileExt))
                            {
                                var fileName = Guid.NewGuid() + Path.GetFileName(image.FileName);
                                var imagePath = Path.Combine(Server.MapPath("~/assets/image"), fileName);

                                // store the uploaded file on the file system
                                image.SaveAs(imagePath);

                                //Return final filepath to database
                                adInfo.AdMedia = new BsonDocument
                                {
                                    {"ImageUrl", "/assets/image/" + fileName},
                                    {"VideoUrl", BsonNull.Value},
                                    {"VideoHost", BsonNull.Value},
                                };
                            }
                        }
                    }
                }
                else if (!adViewModel.VideoUrl.IsNullOrWhiteSpace() && !adViewModel.VideoHost.IsNullOrWhiteSpace())
                {
                    adInfo.AdMedia = new BsonDocument
                    {
                        {"VideoUrl", adViewModel.VideoUrl},
                        {"VideoHost", adViewModel.VideoHost},
                        {"ImageUrl", BsonNull.Value},
                    };
                }


                //Check if Add exists for same country and deactivate
                var builder1 = Builders<AdInfo>.Filter;
                var filter1 = builder1.Eq("CampaignId", adViewModel.AdInfo.CampaignId) &
                              builder1.Eq("AdType", adViewModel.AdInfo.AdType) &
                              builder1.Eq("AdCountry", adViewModel.AdInfo.AdCountry) &
                              builder1.Eq("AdStatus", true);
                var query1 = _dbContext.AdInfos.Find(filter1).FirstOrDefaultAsync();
                if (query1 != null)
                {
                    try
                    {
                        var update = Builders<AdInfo>.Update.Set("AdStatus", false).CurrentDate("TimeUpdated");
                        _dbContext.AdInfos.UpdateOne(filter1, update);
                    }
                    catch (Exception ex)
                    {
                        // log or manage the exception
                        throw ex;
                    }
                }

                //Insert the new Add.
                adInfo.AdStatus = adViewModel.AdInfo.AdStatus;
                adInfo.TimeCreated = DateTime.Now;
                adInfo.TimeUpdated = DateTime.Now;
                var id = _dbContext.AdInfos.InsertOneAsync(adInfo);
            }
            else
            {
                adInfo.CampaignId = adViewModel.AdInfo.CampaignId;
                adInfo.AdCountry = adViewModel.AdInfo.AdCountry;
                adInfo.AdType = adViewModel.AdInfo.AdType;
                if (!adViewModel.ImageUrl.IsNullOrWhiteSpace())
                {
                    //Get The File Posted
                    HttpPostedFileBase image = Request.Files["ImageUrl"];
                    //Generate a filename to store the file on the server
                    if (image != null)
                    {
                        var imageTypes = new[] { "jpg", "jpeg", "png" };
                        var extension = Path.GetExtension(Path.GetFileName(image.FileName));
                        if (extension != null)
                        {
                            var fileExt = extension.Substring(1).ToLower();
                            if (imageTypes.Contains(fileExt))
                            {
                                var fileName = Guid.NewGuid() + Path.GetFileName(image.FileName);
                                var imagePath = Path.Combine(Server.MapPath("~/assets/image"), fileName);

                                // store the uploaded file on the file system
                                image.SaveAs(imagePath);

                                //Return final filepath to database
                                adInfo.AdMedia = new BsonDocument
                            {
                                { "ImageUrl", "/assets/image/" + fileName },
                                { "VideoUrl", BsonNull.Value },
                                { "VideoHost", BsonNull.Value },
                            };
                            }
                        }
                    }
                }
                else if (!adViewModel.VideoUrl.IsNullOrWhiteSpace() && !adViewModel.VideoHost.IsNullOrWhiteSpace())
                {
                    adInfo.AdMedia = new BsonDocument
                {
                    { "VideoUrl", adViewModel.VideoUrl },
                    { "VideoHost", adViewModel.VideoHost },
                    { "ImageUrl", BsonNull.Value },
                };
                }


                //Check if Add exists for same country and deactivate
                var builder1 = Builders<AdInfo>.Filter;
                var filter1 = builder1.Eq("CampaignId", adViewModel.AdInfo.CampaignId) & builder1.Eq("AdType", adViewModel.AdInfo.AdType) & builder1.Eq("AdCountry", adViewModel.AdInfo.AdCountry) &
                             builder1.Eq("AdStatus", true);
                var query1 = _dbContext.AdInfos.Find(filter1).FirstOrDefaultAsync();
                if (query1 != null)
                {
                    try
                    {
                        var update = Builders<AdInfo>.Update.Set("AdStatus", false).CurrentDate("TimeUpdated");
                        _dbContext.AdInfos.UpdateOne(filter1, update);
                    }
                    catch (Exception ex)
                    {
                        // log or manage the exception
                        throw ex;
                    }
                }

                //Update the Campaign Add.
                adInfo.Id = ObjectId.Parse(adViewModel.Id);
                adInfo.TimeUpdated = DateTime.Now;
                var builder = Builders<AdInfo>.Filter;
                var filter = builder.Eq("_id", adInfo.Id);
                _dbContext.AdInfos.ReplaceOne(filter, adInfo, new UpdateOptions { IsUpsert = false });
            }
            

            //Update Campaign with AdMedia
            /*if (id != null)
            {
                var builder = Builders<Campaign>.Filter;
                var filter = builder.Eq("_id", ObjectId.Parse(adInfo.CampaignId));
                var query = _dbContext.Campaigns.Find(filter).FirstOrDefault();

                if (query != null)
                {
                    var update = Builders<Campaign>.Update.Set("AdMedia", adInfo.AdMedia).CurrentDate("TimeUpdated");
                    _dbContext.Campaigns.UpdateOne(filter, update);
                }
            }*/
            return RedirectToAction("CampaignAd");
        }

        // GET: GenerateCards
        public ActionResult GenerateCards()
        {
            var generateCardsView = new CardViewModel();

            return View(generateCardsView);
        }

        public ActionResult GenerateCardsDetails(string id)
        {
            var filter = Builders<CodeCard>.Filter.Eq("_id", ObjectId.Parse(id));

            var codecard = _dbContext.CodeCard.Find(filter).First();

            return View("GenerateCardsDetails", codecard);
        }

        [HttpPost]
        public ActionResult GenerateCards(AdInfo adInfo, CardViewModel cardViewModel)
        {
            var fileName = "";
            int Quantity;
            bool qty = int.TryParse(cardViewModel.CardQuantity, out Quantity);
            adInfo.CampaignId = cardViewModel.AdInfo.CampaignId;
            adInfo.AdCountry = cardViewModel.AdInfo.AdCountry;
            if (!cardViewModel.ImageUrl.IsNullOrWhiteSpace())
            {
                //Get The File Posted
                HttpPostedFileBase image = Request.Files["ImageUrl"];
                //Generate a filename to store the file on the server
                if (image != null)
                {
                    var imageTypes = new[] { "jpg", "jpeg", "png" };
                    var extension = Path.GetExtension(Path.GetFileName(image.FileName));
                    if (extension != null)
                    {
                        var fileExt = extension.Substring(1).ToLower();
                        if (imageTypes.Contains(fileExt))
                        {
                            fileName = "ctemplate." + fileExt;
                            var imagePath = Path.Combine(Server.MapPath("~/assets/image"), fileName);

                            // store the uploaded file on the file system
                            image.SaveAs(imagePath);
                        }
                    }
                }
            }

            //Select All Codes
            var builder = Builders<CampaignCode>.Filter;
            var filter = builder.Eq("CampaignId", adInfo.CampaignId) & builder.Ne("Printed", true);
            var result = _dbContext.CampaignCodes.Find(filter).ToList();

            if (result.Count() >= Quantity && !Quantity.Equals(null))
            {
                //Grab Image Template
                FileInfo fileInfo = new FileInfo(Path.Combine(Server.MapPath("~/assets/image"), fileName));

                List<int> cardList = new List<int>();

                using (MagickImageCollection pdfCollection = new MagickImageCollection())
                {
                    for (int i = 0; i < Quantity; i = i + 8)
                    {
                        if (Quantity - i >= 8 || Quantity - i == 0)
                        {
                            cardList.Add(i);
                            MagickImage card = new MagickImage(fileInfo);
                            Drawables cDrawable = new Drawables();
                            cDrawable.FontPointSize(20);
                            cDrawable.FillColor(MagickColors.White);
                            //cDrawable.StrokeColor(MagickColors.White);
                            cDrawable.Font("Segoe UI");
                            cDrawable.TextAlignment(TextAlignment.Center);
                            cDrawable.TextInterwordSpacing(2);

                            //Write Code 1
                            cDrawable.Text(187, 172, result.ElementAt(i).Code);
                            cDrawable.Draw(card);
                            filter = builder.Eq("Code", result.ElementAt(i).Code);
                            var update1 = Builders<CampaignCode>.Update.Set("Printed", true).CurrentDate("TimeUpdated");
                            _dbContext.CampaignCodes.UpdateOne(filter, update1);

                            //Write Code 2
                            cDrawable.Text(513, 172, result.ElementAt(i + 1).Code);
                            cDrawable.Draw(card);
                            filter = builder.Eq("Code", result.ElementAt(i + 1).Code);
                            var update2 = Builders<CampaignCode>.Update.Set("Printed", true).CurrentDate("TimeUpdated");
                            _dbContext.CampaignCodes.UpdateOne(filter, update2);

                            //Write Code 3
                            cDrawable.Text(187, 384, result.ElementAt(i + 2).Code);
                            cDrawable.Draw(card);
                            filter = builder.Eq("Code", result.ElementAt(i + 2).Code);
                            var update3 = Builders<CampaignCode>.Update.Set("Printed", true).CurrentDate("TimeUpdated");
                            _dbContext.CampaignCodes.UpdateOne(filter, update3);

                            //Write Code 4
                            cDrawable.Text(513, 384, result.ElementAt(i + 3).Code);
                            cDrawable.Draw(card);
                            filter = builder.Eq("Code", result.ElementAt(i + 3).Code);
                            var update4 = Builders<CampaignCode>.Update.Set("Printed", true).CurrentDate("TimeUpdated");
                            _dbContext.CampaignCodes.UpdateOne(filter, update4);

                            //Write Code 5
                            cDrawable.Text(187, 600, result.ElementAt(i + 4).Code);
                            cDrawable.Draw(card);
                            filter = builder.Eq("Code", result.ElementAt(i + 4).Code);
                            var update5 = Builders<CampaignCode>.Update.Set("Printed", true).CurrentDate("TimeUpdated");
                            _dbContext.CampaignCodes.UpdateOne(filter, update5);

                            //Write Code 6
                            cDrawable.Text(513, 600, result.ElementAt(i + 5).Code);
                            cDrawable.Draw(card);
                            filter = builder.Eq("Code", result.ElementAt(i + 5).Code);
                            var update6 = Builders<CampaignCode>.Update.Set("Printed", true).CurrentDate("TimeUpdated");
                            _dbContext.CampaignCodes.UpdateOne(filter, update6);

                            //Write Code 7
                            cDrawable.Text(187, 812, result.ElementAt(i + 6).Code);
                            cDrawable.Draw(card);
                            filter = builder.Eq("Code", result.ElementAt(i + 6).Code);
                            var update7 = Builders<CampaignCode>.Update.Set("Printed", true).CurrentDate("TimeUpdated");
                            _dbContext.CampaignCodes.UpdateOne(filter, update7);

                            //Write Code 8
                            cDrawable.Text(513, 812, result.ElementAt(i + 7).Code);
                            cDrawable.Draw(card);
                            filter = builder.Eq("Code", result.ElementAt(i + 7).Code);
                            var update8 = Builders<CampaignCode>.Update.Set("Printed", true).CurrentDate("TimeUpdated");
                            _dbContext.CampaignCodes.UpdateOne(filter, update8);


                            //Write the final image
                            System.IO.Directory.CreateDirectory(
                                Server.MapPath("~/assets/image/" + cardViewModel.AdInfo.CampaignId));
                            card.Write(Path.Combine(
                                Server.MapPath("~/assets/image/" + cardViewModel.AdInfo.CampaignId), i + ".jpg"));
                        }
                        else
                        {
                            //int remainder = Quantity - i;

                            MagickImage card = new MagickImage(fileInfo);
                            Drawables cDrawable = new Drawables();
                            cDrawable.FontPointSize(20);
                            cDrawable.FillColor(MagickColors.White);
                            //cDrawable.StrokeColor(MagickColors.White);
                            cDrawable.Font("Segoe UI");
                            cDrawable.TextAlignment(TextAlignment.Center);
                            cDrawable.TextInterwordSpacing(2);

                            //Write Code 1
                            cDrawable.Text(157, 172, result.ElementAt(i).Code);
                            cDrawable.Draw(card);
                            filter = builder.Eq("Code", result.ElementAt(i).Code);
                            var update1 = Builders<CampaignCode>.Update.Set("Printed", true).CurrentDate("TimeUpdated");
                            _dbContext.CampaignCodes.UpdateOne(filter, update1);

                            if (++i < Quantity)
                            {
                                //Write Code 2
                                cDrawable.Text(513, 172, result.ElementAt(i + 1).Code);
                                cDrawable.Draw(card);
                                filter = builder.Eq("Code", result.ElementAt(i + 1).Code);
                                var update2 =
                                    Builders<CampaignCode>.Update.Set("Printed", true).CurrentDate("TimeUpdated");
                                _dbContext.CampaignCodes.UpdateOne(filter, update2);

                                if (++i < Quantity)
                                {
                                    //Write Code 3
                                    cDrawable.Text(187, 384, result.ElementAt(i + 2).Code);
                                    cDrawable.Draw(card);
                                    filter = builder.Eq("Code", result.ElementAt(i + 2).Code);
                                    var update3 =
                                        Builders<CampaignCode>.Update.Set("Printed", true).CurrentDate("TimeUpdated");
                                    _dbContext.CampaignCodes.UpdateOne(filter, update3);

                                    if (++i < Quantity)
                                    {
                                        //Write Code 4
                                        cDrawable.Text(513, 384, result.ElementAt(i + 3).Code);
                                        cDrawable.Draw(card);
                                        filter = builder.Eq("Code", result.ElementAt(i + 3).Code);
                                        var update4 =
                                            Builders<CampaignCode>.Update.Set("Printed", true)
                                                .CurrentDate("TimeUpdated");
                                        _dbContext.CampaignCodes.UpdateOne(filter, update4);

                                        if (i++ < Quantity)
                                        {
                                            //Write Code 5
                                            cDrawable.Text(187, 600, result.ElementAt(i + 4).Code);
                                            cDrawable.Draw(card);
                                            filter = builder.Eq("Code", result.ElementAt(i + 4).Code);
                                            var update5 =
                                                Builders<CampaignCode>.Update.Set("Printed", true)
                                                    .CurrentDate("TimeUpdated");
                                            _dbContext.CampaignCodes.UpdateOne(filter, update5);

                                            if (i++ < Quantity)
                                            {
                                                //Write Code 6
                                                cDrawable.Text(513, 600, result.ElementAt(i + 5).Code);
                                                cDrawable.Draw(card);
                                                filter = builder.Eq("Code", result.ElementAt(i + 5).Code);
                                                var update6 =
                                                    Builders<CampaignCode>.Update.Set("Printed", true)
                                                        .CurrentDate("TimeUpdated");
                                                _dbContext.CampaignCodes.UpdateOne(filter, update6);

                                                if (i++ < Quantity)
                                                {
                                                    //Write Code 7
                                                    cDrawable.Text(187, 812, result.ElementAt(i + 6).Code);
                                                    cDrawable.Draw(card);
                                                    filter = builder.Eq("Code", result.ElementAt(i + 6).Code);
                                                    var update7 =
                                                        Builders<CampaignCode>.Update.Set("Printed", true)
                                                            .CurrentDate("TimeUpdated");
                                                    _dbContext.CampaignCodes.UpdateOne(filter, update7);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            //Write the final image
                            System.IO.Directory.CreateDirectory(
                                Server.MapPath("~/assets/image/" + cardViewModel.AdInfo.CampaignId));
                            card.Write(Path.Combine(
                                Server.MapPath("~/assets/image/" + cardViewModel.AdInfo.CampaignId), i + ".jpg"));
                            cardList.Add(i);
                        }
                    }

                    //Create PDF from Images
                    for (int p = 0; p <= cardList.Max(); p++)
                    {
                        if (cardList.Contains(p))
                        {
                            if (
                                System.IO.File.Exists(
                                    Path.Combine(Server.MapPath("~/assets/image/" + cardViewModel.AdInfo.CampaignId),
                                        p + ".jpg")))
                            {
                                pdfCollection.Add(
                                    new MagickImage(
                                        Path.Combine(
                                            Server.MapPath("~/assets/image/" + cardViewModel.AdInfo.CampaignId),
                                            p + ".jpg")));
                            }
                        }

                    }

                    pdfCollection.Write(Path.Combine(
                        Server.MapPath("~/assets/image/" + cardViewModel.AdInfo.CampaignId), "CardImages.pdf"));
                }

                var generateCardsView = new CardViewModel()
                {
                    DownloadUrl = cardViewModel.AdInfo.CampaignId + "/CardImages.pdf",
                };

                //Insert into CodeCard
                var codeCard = new CodeCard();
                codeCard.CampaignId = cardViewModel.AdInfo.CampaignId;
                codeCard.Quantity = cardViewModel.CardQuantity;
                codeCard.TimeCreated = DateTime.Now;
                codeCard.TimeUpdated = DateTime.Now;
                _dbContext.CodeCard.InsertOneAsync(codeCard);

                return View(generateCardsView);
            }
            else
            {
                string errMsg = "You have only " + result.Count + " outstanding codes.";
                var generateCardsView = new CardViewModel()
                {
                    ErrorMsg = errMsg
                };
                return View(generateCardsView);
            }

            
        }

        // GET: Passitions
        public ActionResult Passiton()
        {
            return View();
        }

        public ActionResult PassitonDetails(string id)
        {
            var filter = Builders<Models.PassItOn>.Filter.Eq("_id", ObjectId.Parse(id));

            var business = _dbContext.PassItOns.Find(filter).First();

            return View("PassitonDetails", business);
        }

        public ActionResult CodeFailure()
        {
            return View();
        }

        public ActionResult CodeFailureDetails(string id)
        {
            var filter = Builders<CodeFailure>.Filter.Eq("_id", ObjectId.Parse(id));

            var business = _dbContext.CodeFailure.Find(filter).First();

            return View("CodeFailureDetails", business);
        }
    }
}